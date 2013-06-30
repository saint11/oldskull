using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Monocle
{
    static public class Input
    {
        public const int MAX_GAMEPADS = 4;

        static public KeyboardState OldKeyboardState;
        static public KeyboardState CurrentKeyboardState;
        static public MouseState OldMouseState;
        static public MouseState CurrentMouseState;
        static public GamePadState[] OldGamepadStates = new GamePadState[MAX_GAMEPADS];
        static public GamePadState[] CurrentGamepadStates = new GamePadState[MAX_GAMEPADS];

        static private int[] rumbleFrames = new int[MAX_GAMEPADS];
        static private float[] rumbleStrengths = new float[MAX_GAMEPADS];

        static public event Action OnGamepadChangeDetected;
        static public bool GamepadsChanged { get; private set; }

        static internal void Initialize()
        {
            CurrentKeyboardState = Keyboard.GetState();
            CurrentMouseState = Mouse.GetState();
            for (int i = 0; i < MAX_GAMEPADS; i++)
                CurrentGamepadStates[i] = GamePad.GetState((PlayerIndex)i);
        }

        static internal void Update()
        {
            //Update keyboard state
            OldKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = Keyboard.GetState();

            UpdateUtil();
        }

#if DEBUG
        static internal void UpdateNoKeyboard()
        {
            //Update keyboard state
            OldKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = new KeyboardState(Keys.OemTilde);

            UpdateUtil();
        }
#endif

        static private void UpdateUtil()
        {
            //Update mouse state
            OldMouseState = CurrentMouseState;
            CurrentMouseState = Mouse.GetState();

            //Update rumbles and gamepad states
            bool connectionsChanged = false;
            for (int i = 0; i < MAX_GAMEPADS; i++)
            {
                OldGamepadStates[i] = CurrentGamepadStates[i];
                CurrentGamepadStates[i] = GamePad.GetState((PlayerIndex)i);

                if (OldGamepadStates[i].IsConnected != CurrentGamepadStates[i].IsConnected)
                    connectionsChanged = true;

                if (rumbleFrames[i] > 0)
                {
                    rumbleFrames[i]--;
                    if (rumbleFrames[i] == 0)
                    {
                        rumbleStrengths[i] = 0;
                        GamePad.SetVibration((PlayerIndex)i, 0, 0);
                    }
                }
            }

            if (connectionsChanged && OnGamepadChangeDetected != null)
            {
                GamepadsChanged = true;
                OnGamepadChangeDetected();
            }
            else
                GamepadsChanged = false;
        }

        #region Keyboard

        static public bool Check(Keys key)
        {
            return CurrentKeyboardState[key] == KeyState.Down;
        }

        static public bool Pressed(Keys key)
        {
            return CurrentKeyboardState[key] == KeyState.Down && OldKeyboardState[key] == KeyState.Up;
        }

        static public bool Released(Keys key)
        {
            return CurrentKeyboardState[key] == KeyState.Up && OldKeyboardState[key] == KeyState.Down;
        }

        static public bool Check(params Keys[] keys)
        {
            foreach (var key in keys)
                if (Check(key))
                    return true;
            return false;
        }

        static public bool Pressed(params Keys[] keys)
        {
            foreach (var key in keys)
                if (Pressed(key))
                    return true;
            return false;
        }

        static public bool Released(params Keys[] keys)
        {
            foreach (var key in keys)
                if (Released(key))
                    return true;
            return false;
        }

        #endregion

        #region Gamepad

        static public bool Check(int gamepadIndex, Buttons button)
        {
            return CurrentGamepadStates[gamepadIndex].IsButtonDown(button);
        }

        static public bool Pressed(int gamepadIndex, Buttons button)
        {
            return CurrentGamepadStates[gamepadIndex].IsButtonDown(button) && OldGamepadStates[gamepadIndex].IsButtonUp(button);
        }

        static public bool Released(int gamepadIndex, Buttons button)
        {
            return CurrentGamepadStates[gamepadIndex].IsButtonUp(button) && OldGamepadStates[gamepadIndex].IsButtonDown(button);
        }

        static public bool Check(int gamepadIndex, params Buttons[] buttons)
        {
            foreach (var button in buttons)
                if (Check(gamepadIndex, button))
                    return true;
            return false;
        }

        static public bool Pressed(int gamepadIndex, params Buttons[] buttons)
        {
            foreach (var button in buttons)
                if (Pressed(gamepadIndex, button))
                    return true;
            return false;
        }

        static public bool Released(int gamepadIndex, params Buttons[] buttons)
        {
            foreach (var button in buttons)
                if (Released(gamepadIndex, button))
                    return true;
            return false;
        }

        static public Vector2 CheckLeftStick(int gamepadIndex, float deadzone = .1f)
        {
            Vector2 axis = CurrentGamepadStates[gamepadIndex].ThumbSticks.Left;
            axis.Y *= -1;
            if (Math.Abs(axis.X) < deadzone)
                axis.X = 0;
            if (Math.Abs(axis.Y) < deadzone)
                axis.Y = 0;
            return axis;
        }

        static public Vector2 CheckRightStick(int gamepadIndex, float deadzone = .1f)
        {
            Vector2 axis = CurrentGamepadStates[gamepadIndex].ThumbSticks.Right;
            axis.Y *= -1;
            if (Math.Abs(axis.X) < deadzone)
                axis.X = 0;
            if (Math.Abs(axis.Y) < deadzone)
                axis.Y = 0;
            return axis;
        }

        static public Vector2 CheckDPad(int gamepadIndex)
        {
            return new Vector2(
                CurrentGamepadStates[gamepadIndex].DPad.Left == ButtonState.Pressed ? -1 : (CurrentGamepadStates[gamepadIndex].DPad.Right == ButtonState.Pressed ? 1 : 0),
                CurrentGamepadStates[gamepadIndex].DPad.Up == ButtonState.Pressed ? -1 : (CurrentGamepadStates[gamepadIndex].DPad.Down == ButtonState.Pressed ? 1 : 0)
            );
        }

        static public bool CheckConnected(int gamepadIndex)
        {
            return CurrentGamepadStates[gamepadIndex].IsConnected;
        }

        static public void Rumble(int gamepadIndex, float strength, int frames, bool alwaysReplace = false)
        {
            if (alwaysReplace || rumbleStrengths[gamepadIndex] < strength || (rumbleStrengths[gamepadIndex] == strength && rumbleFrames[gamepadIndex] < frames))
            {
                rumbleStrengths[gamepadIndex] = strength;
                rumbleFrames[gamepadIndex] = frames;

                GamePad.SetVibration((PlayerIndex)gamepadIndex, strength, strength);
            }
        }

        #endregion

        #region Mouse

        static public bool CheckMouseLeft
        {
            get
            {
                return CurrentMouseState.LeftButton == ButtonState.Pressed;
            }
        }

        static public bool PressedMouseLeft
        {
            get
            {
                return CurrentMouseState.LeftButton == ButtonState.Pressed && OldMouseState.LeftButton == ButtonState.Released;
            }
        }

        static public bool ReleasedMouseLeft
        {
            get
            {
                return CurrentMouseState.LeftButton == ButtonState.Released && OldMouseState.LeftButton == ButtonState.Pressed;
            }
        }

        static public bool CheckMouseRight
        {
            get
            {
                return CurrentMouseState.RightButton == ButtonState.Pressed;
            }
        }

        static public bool PressedMouseRight
        {
            get
            {
                return CurrentMouseState.RightButton == ButtonState.Pressed && OldMouseState.RightButton == ButtonState.Released;
            }
        }

        static public bool ReleasedMouseRight
        {
            get
            {
                return CurrentMouseState.RightButton == ButtonState.Released && OldMouseState.RightButton == ButtonState.Pressed;
            }
        }

        static public bool CheckMouseMiddle
        {
            get
            {
                return CurrentMouseState.MiddleButton == ButtonState.Pressed;
            }
        }

        static public bool PressedMouseMiddle
        {
            get
            {
                return CurrentMouseState.MiddleButton == ButtonState.Pressed && OldMouseState.MiddleButton == ButtonState.Released;
            }
        }

        static public bool ReleasedMouseMiddle
        {
            get
            {
                return CurrentMouseState.MiddleButton == ButtonState.Released && OldMouseState.MiddleButton == ButtonState.Pressed;
            }
        }

        static public int MouseWheel
        {
            get
            {
                return CurrentMouseState.ScrollWheelValue;
            }
        }

        static public int MouseWheelDelta
        {
            get
            {
                return CurrentMouseState.ScrollWheelValue - OldMouseState.ScrollWheelValue;
            }
        }

        static public float MouseX
        {
            get
            {
                return CurrentMouseState.X / Engine.Instance.Screen.Scale;
            }
        }

        static public float MouseY
        {
            get
            {
                return CurrentMouseState.Y / Engine.Instance.Screen.Scale;
            }
        }

        static public Vector2 MousePosition
        {
            get
            {
                return new Vector2(MouseX, MouseY);
            }
        }

        #endregion
    }
}
