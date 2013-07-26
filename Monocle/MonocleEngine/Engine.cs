using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Runtime;
#if OUYA
using Ouya.Console.Api;
#endif

namespace Monocle
{
    public enum GameTags { Player = 0, Corpse, Solid, Actor, Arrow, JumpPad, Target, Chain, PlayerCollider, PlayerCollectible, ExplosionCollider, Orb, Enemy, Climbable, Brambles, Dummy, LightSource, PlayerOnlySolid, NotArrowsSolid, Ice, Hat, Granite };

    public class Engine : Game
    {
        public const float FRAME_RATE = 60f;
        static private readonly TimeSpan SixtyDelta = TimeSpan.FromSeconds(1.0 / FRAME_RATE);
        private const float FULL_DELTA = 1f / 60f;
        private const float CLAMP_ADD = FULL_DELTA * .25f;

        static public Engine Instance { get; private set; }
        static internal int TagAmount = Enum.GetNames(typeof(GameTags)).Length;
        static public float TimeMult { get; private set; }
        static public float LastTimeMult { get; private set; }
        static public float DeltaTime { get; private set; }
        static public float ActualDeltaTime { get; private set; }
        static public float TimeRate = 1f;
#if OUYA
        static public OuyaFacade OuyaFacade;
#endif

        public GraphicsDeviceManager Graphics { get; private set; }
        public Screen Screen { get; private set; }

        private Scene scene;
        private Scene nextScene;
        private string windowTitle;

#if DEBUG 
#if DESKTOP
        public Commands Commands { get; private set; }
#elif OUYA
        public string FrameRateDisplay { get; private set; }
#endif
        private TimeSpan counterElapsed = TimeSpan.Zero;
        private int counterFrames = 0;
#endif

        public Engine(int width, int height, float scale, string windowTitle)
        {
            this.windowTitle = windowTitle;

            Instance = this;
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = Calc.LOADPATH + "Content";
            IsMouseVisible = false;
            Screen = new Screen(this, width, height, scale);

#if DESKTOP
            IsFixedTimeStep = false;
#elif OUYA
            IsFixedTimeStep = false;
#endif
            TargetElapsedTime = TimeSpan.FromSeconds(1.0 / FRAME_RATE);
            //GCSettings.LatencyMode = GCLatencyMode.LowLatency;
        }

        protected override void Initialize()
        {
            base.Initialize();
#if DEBUG
#if DESKTOP
            Commands = new Commands();
#elif OUYA
            FrameRateDisplay = "";
#endif
#endif
            Music.Initialize();
            Input.Initialize();
            Screen.Initialize();
            Monocle.Draw.Init(GraphicsDevice);
            Graphics.DeviceReset += OnGraphicsReset;
#if DESKTOP
            Window.Title = windowTitle;
#endif
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
            //Calculate delta time stuff   
            LastTimeMult = TimeMult;
            ActualDeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds * TimeRate;

            if (IsFixedTimeStep)
            {
                TimeMult = (60f / FRAME_RATE) * TimeRate;
                DeltaTime = (1f / FRAME_RATE) * TimeRate;
            }
            else
            {
                DeltaTime = MathHelper.Clamp(
                    ActualDeltaTime,
                    FULL_DELTA * TimeRate - CLAMP_ADD,
                    FULL_DELTA * TimeRate + CLAMP_ADD
                );
                TimeMult = DeltaTime / FULL_DELTA;
            }

#if DEBUG && DESKTOP
            if (Commands.Open)
                Input.UpdateNoKeyboard();
            else
                Input.Update();
#else
            Input.Update();
#endif            

            Music.Update();
            if (scene != null && scene.Active)
                scene.Update();

#if DEBUG && DESKTOP
            if (Commands.Open)
                Commands.UpdateOpen();
            else
                Commands.UpdateClosed();
#endif

            if (scene != nextScene)
            {
                if (scene != null)
                    scene.End();
                scene = nextScene;
                OnSceneTransition();
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

            //if (scene != null)
              //  scene.PostRender();

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Black);
            Screen.Render();

            //if (scene != null)
              //  scene.PostScreen();

            base.Draw(gameTime);

#if DEBUG
#if DESKTOP
            //Debug console
            if (Commands.Open)
                Commands.Render();

            //Frame counter
            counterFrames++;
            counterElapsed += gameTime.ElapsedGameTime;
            if (counterElapsed > TimeSpan.FromSeconds(1))
            {
                Window.Title = windowTitle + " " + counterFrames.ToString() + " fps - " + (GC.GetTotalMemory(true) / 1048576f).ToString("F") + " MB";
                counterFrames = 0;
                counterElapsed -= TimeSpan.FromSeconds(1);
            }
#elif OUYA
            //Frame counter
            counterFrames++;
            counterElapsed += gameTime.ElapsedGameTime;
            if (counterElapsed > TimeSpan.FromSeconds(1))
            {
                FrameRateDisplay = counterFrames.ToString() + " fps";
                counterFrames = 0;
                counterElapsed -= TimeSpan.FromSeconds(1);
            }

            if (FrameRateDisplay != "")
            {
                Monocle.Draw.SpriteBatch.Begin();
                Monocle.Draw.TextJustify(Monocle.Draw.DefaultFont, FrameRateDisplay, new Vector2(4, 4), Color.White, Vector2.Zero);
                Monocle.Draw.SpriteBatch.End();
            }
#endif
#endif
        }

        public Scene Scene
        {
            get { return scene; }
            set { nextScene = value; }
        }

        protected virtual void OnSceneTransition()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            Audio.Stop();
            Music.Stop();
            base.OnExiting(sender, args);
        }
    }
}
