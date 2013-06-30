using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Monocle
{
    public enum GameTags { Player = 0, Corpse, Solid, Actor, Arrow, Target, PushBlock, PlayerCollider, ExplosionCollider, Coin, Orb, Enemy, Climbable, LightSource, Outlined };

    public class Engine : Game
    {
        static public Engine Instance { get; private set; }
        static internal int TagAmount = Enum.GetNames(typeof(GameTags)).Length;

        public GraphicsDeviceManager Graphics { get; private set; }
        public Screen Screen { get; private set; }
        public float DeltaTime { get; private set; }

        private Scene scene;
        private Scene nextScene;
        private string windowTitle;
        private float framesPerSecond;
        private float changeFPSTo;
        private int fpsChangeDelay;

#if DEBUG
        public Commands Commands { get; private set; }
        private TimeSpan counterElapsed = TimeSpan.Zero;
        private int counterFrames = 0;
#endif

        public Engine(int width, int height, float framesPerSecond, string windowTitle)
        {
            this.windowTitle = windowTitle;

            Instance = this;
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Screen = new Screen(this, width, height);

            IsFixedTimeStep = true;
            FramesPerSecond = framesPerSecond;
        }

        public float FramesPerSecond
        {
            get { return framesPerSecond; }
            set
            {
                if (framesPerSecond != value)
                {
                    framesPerSecond = value;
                    TargetElapsedTime = TimeSpan.FromMilliseconds(Math.Floor(1000.0 / (double)framesPerSecond));
                    DeltaTime = 1.0f / framesPerSecond;
                    fpsChangeDelay = 0;
                }
            }
        }

        protected override void Initialize()
        {
            base.Initialize();

#if DEBUG
            Commands = new Commands();
#endif

            Input.Initialize();
            Screen.Initialize();
            Monocle.Draw.Init(GraphicsDevice);
            Graphics.DeviceReset += OnGraphicsReset;

            Window.Title = windowTitle;
        }

        private void OnGraphicsReset(object sender, EventArgs e)
        {
            if (scene != null)
                scene.HandleGraphicsReset();
            if (nextScene != null)
                nextScene.HandleGraphicsReset();
        }

        protected override void Update(GameTime gameTime)
        {
#if DEBUG
            //In Debug mode, press Left Shift + ESC to quit the game
            if (Input.Pressed(Keys.Escape) && Input.Check(Keys.LeftShift))
                this.Exit();

            if (Commands.Open)
            {
                Commands.UpdateOpen();
                Input.UpdateNoKeyboard();
            }
            else
            {
                Commands.UpdateClosed();
                Input.Update();
            }
#else
            Input.Update();
#endif            
 
            //FPS change counter
            if (fpsChangeDelay > 0)
            {
                fpsChangeDelay--;
                if (fpsChangeDelay == 0)
                    FramesPerSecond = changeFPSTo;
            }

            if (scene != null && scene.Active)
                scene.Update();

            if (scene != nextScene)
            {
                if (scene != null)
                    scene.End();
                CancelChangeFPS();
                scene = nextScene;
                if (scene != null)
                    scene.Begin();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(Screen.RenderTarget);
            GraphicsDevice.Clear(Screen.ClearColor);

            if (scene != null)
                scene.Render();

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Black);
            Screen.Render();

            base.Draw(gameTime);

#if DEBUG
            if (Commands.Open)
                Commands.Render();

            //Frame counter
            counterFrames++;
            counterElapsed += gameTime.ElapsedGameTime;
            if (counterElapsed > TimeSpan.FromSeconds(1))
            {
                Window.Title = windowTitle + " " + counterFrames.ToString() + " fps";
                counterFrames = 0;
                counterElapsed -= TimeSpan.FromSeconds(1);
            }
#endif
        }

        public Scene Scene
        {
            get { return scene; }
            set { nextScene = value; }
        }

        #region Change the FPS

        public void ChangeFPS(float newFPS, int frameDelay)
        {
            if (newFPS != framesPerSecond)
            {
                changeFPSTo = newFPS;
                fpsChangeDelay = frameDelay;
            }
        }

        public void CancelChangeFPS()
        {
            fpsChangeDelay = 0;
        }

        #endregion
    }
}
