using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle;
using Microsoft.Xna.Framework;

namespace OldSkull.Graphics
{
    public class ProgressBar:GraphicsComponent
    {
        private Texture Full;
        public Rectangle FullClipRect;
        private Texture Empty;
        public Rectangle EmptyClipRect;
        private float value=0.5f;

        public ProgressBar(int Width,int Height, Color FullColor, Color EmptyColor)
            : base(false)
        {
            Full = new Texture(Width, Height, FullColor);
            FullClipRect = new Rectangle(0,0,Width/2, Height);
            Empty = new Texture(Width, Height, EmptyColor);
            EmptyClipRect = new Rectangle(0, 0, Width, Height);
        }

        public ProgressBar(string FullImage, string EmptyImage)
            : base(true)
        {
            Full = OldSkullGame.Atlas[FullImage].Texture;
            FullClipRect = new Rectangle(0, 0, Full.Width, Full.Height);
            Empty = OldSkullGame.Atlas[EmptyImage].Texture;
            EmptyClipRect = new Rectangle(0, 0, Empty.Width, Empty.Height);

        }

        public override void Render()
        {
            FullClipRect.Width = (int)(Full.Width * value);
            Draw.SpriteBatch.Draw(Empty.Texture2D, RenderPosition, EmptyClipRect, Color, Rotation, Origin, Scale * Zoom, Effects, 0);
            Draw.SpriteBatch.Draw(Full.Texture2D, RenderPosition, FullClipRect, Color, Rotation, Origin, Scale * Zoom, Effects, 0);
        }


        public override int Width
        {
            get { return Math.Max(EmptyClipRect.Width,FullClipRect.Width); }
        }

        public override int Height
        {
            get { return Math.Max(EmptyClipRect.Height, FullClipRect.Height); }
        }

        public void SetValue(float value)
        {
            this.value = value;
        }
    }
}
