using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Monocle
{
    public class NumberText : GraphicsComponent
    {
        private SpriteFont font;
        private int value;
        private string prefix;
        private string drawString;

        private bool centered;

        public Action OnValueUpdate;

        public NumberText(SpriteFont font, string prefix, int value, bool centered = false)
            : base(false)
        {
            this.font = font;
            this.prefix = prefix;
            this.value = value;
            this.centered = centered;
            UpdateString();
        }

        public int Value
        {
            get
            {
                return value;
            }

            set
            {
                if (this.value != value)
                {
                    this.value = value;
                    UpdateString();
                    if (OnValueUpdate != null)
                        OnValueUpdate();
                }
            }
        }

        public void UpdateString()
        {
            drawString = prefix + value.ToString();

            if (centered)
                Origin = font.MeasureString(drawString) / 2;
        }

        public override void Render()
        {
            Draw.SpriteBatch.DrawString(font, drawString, RenderPosition, Color, Rotation, Origin, Scale * Zoom, Effects, 0);
        }

        public override int Width
        {
            get { return (int)font.MeasureString(drawString).X; }
        }

        public override int Height
        {
            get { return (int)font.MeasureString(drawString).Y; }
        }
    }
}
