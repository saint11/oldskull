using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Monocle
{
    public class Screen
    {
        public Engine Engine { get; private set; }
        public GraphicsDeviceManager Graphics { get { return Engine.Graphics; } }
        public GraphicsDevice GraphicsDevice { get { return Engine.GraphicsDevice; } }
        public RenderTarget2D RenderTarget { get; private set; }

        public Color ClearColor = Color.Black;
        public SamplerState SamplerState = SamplerState.PointClamp;
        public Effect Effect = null;

        private Viewport viewport;
        private float scale = 1.0f;
        private Rectangle screenRect;
        private Rectangle drawRect;

        public Screen(Engine engine, int width, int height, float scale)
        {
            Engine = engine;
            screenRect = drawRect = new Rectangle(0, 0, width, height);
            viewport = new Viewport();
            viewport.Width = width;
            viewport.Height = height;
        }

        public void Initialize()
        {
            RenderTarget = new RenderTarget2D(GraphicsDevice, screenRect.Width, screenRect.Height, false, SurfaceFormat.Color, DepthFormat.Depth24);
            SetWindowSize(screenRect.Width, screenRect.Height);
        }

        public void Render()
        {
            GraphicsDevice.Viewport = viewport;

            Draw.SpriteBatch.Begin(SpriteSortMode.Texture, BlendState.Opaque, SamplerState, DepthStencilState.None, RasterizerState.CullNone, Effect);
            Draw.SpriteBatch.Draw(RenderTarget, drawRect, screenRect, Color.White);
            Draw.SpriteBatch.End();
        }

        public float Scale
        {
            get { return scale; }
            set
            {
                if (scale != value)
                {
                    scale = value;
                    drawRect.Width = viewport.Width = (int)(screenRect.Width * scale);
                    drawRect.Height = viewport.Height = (int)(screenRect.Height * scale);

                    if (IsFullscreen)
                        HandleFullscreenViewport();
                    else
                        SetWindowSize(viewport.Width, viewport.Height);
                }
            }
        }

        private void SetWindowSize(int width, int height)
        {
            Graphics.PreferredBackBufferWidth = width;
            Graphics.PreferredBackBufferHeight = height;
            Graphics.ApplyChanges();

            viewport.X = (width - viewport.Width) / 2;
            viewport.Y = (height - viewport.Height) / 2;
        }

        private void HandleFullscreenViewport()
        {
            viewport.X = (GraphicsDevice.DisplayMode.Width - viewport.Width) / 2;
            viewport.Y = (GraphicsDevice.DisplayMode.Height - viewport.Height) / 2;
        }

        public enum FullscreenMode { KeepScale, LargestScale, LargestIntegerScale };
        public void EnableFullscreen(FullscreenMode mode = FullscreenMode.LargestScale)
        {
            Graphics.IsFullScreen = true;
            Graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            Graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            Graphics.ApplyChanges();

            if (mode == FullscreenMode.LargestScale)
                Scale = Math.Min(GraphicsDevice.DisplayMode.Width / (float)screenRect.Width, GraphicsDevice.DisplayMode.Height / (float)screenRect.Height);
            else if (mode == FullscreenMode.LargestIntegerScale)
                Scale = (float)Math.Floor(Math.Min(GraphicsDevice.DisplayMode.Width / (float)screenRect.Width, GraphicsDevice.DisplayMode.Height / (float)screenRect.Height));

            HandleFullscreenViewport();
        }

        public void DisableFullscreen(float newScale)
        {
            Graphics.IsFullScreen = false;
            Scale = newScale;      
        }

        public void DisableFullscreen()
        {
            DisableFullscreen(scale);
        }

        public bool IsFullscreen { get { return Graphics.IsFullScreen; } }
        public Vector2 Size { get { return new Vector2(Width, Height); } }
        public int Width { get { return RenderTarget.Width; } }
        public int Height { get { return RenderTarget.Height; } }
        public Vector2 Center { get { return Size * 0.5f; } }
    }
}
