﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monocle
{
    public class Image : GraphicsComponent
    {
        public Texture Texture { get; protected set; }
        public Rectangle ClipRect;

        public Image(Texture texture, Rectangle? clipRect = null)
            : this(texture, clipRect, false)
        {

        }

        public Image(Subtexture subTexture, Rectangle? clipRect = null)
            : this(subTexture, clipRect, false)
        {

        }

        internal Image(Texture texture, Rectangle? clipRect, bool active)
            : base(active)
        {
            Texture = texture;
            ClipRect = clipRect ?? texture.Rect;
        }

        internal Image(Subtexture subTexture, Rectangle? clipRect, bool active)
            : base(active)
        {
            Texture = subTexture.Texture;

            if (clipRect.HasValue)
                ClipRect = subTexture.GetAbsoluteClipRect(clipRect.Value);
            else
                ClipRect = subTexture.Rect;
        }

        public override void Render()
        {
            Draw.SpriteBatch.Draw(Texture.Texture2D, RenderPosition, ClipRect, Color, Rotation, Origin, Scale * Zoom, Effects, 0);
        }

        public override int Width
        {
            get { return ClipRect.Width; }
        }

        public override int Height
        {
            get { return ClipRect.Height; }
        }

        public void SwapSubtexture(Subtexture subtexture, Rectangle? clipRect = null)
        {
            Texture = subtexture.Texture;
            ClipRect = clipRect ?? subtexture.Rect;
        }
    }
}
