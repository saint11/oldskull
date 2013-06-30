using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monocle
{
    public struct Particle
    {
        public ParticleType Type;

        public bool Active;
        public Color Color;
        public Vector2 Position;
        public Vector2 Speed;
        public int Size;
        public int Life;
        public int ColorSwitch;

        public void Update()
        {
            //Life
            Life--;
            if (Life == 0)
            {
                Active = false;
                return;
            }
            
            //Color switch
            if (ColorSwitch > 0)
            {
                ColorSwitch--;
                if (ColorSwitch == 0)
                {
                    if (Type.ColorSwitchLoop)
                        ColorSwitch = Type.ColorSwitch;

                    if (Color == Type.Color)
                        Color = Type.Color2;
                    else
                        Color = Type.Color;
                }
            }

            //Speed
            Position += Speed;
            Speed += Type.Acceleration;
            Speed *= Type.SpeedMultiplier;
        }

        public void Render()
        {
            if (Type.Source == null)
                Draw.SpriteBatch.Draw(Draw.Particle, RenderPosition, null, Color, 0, Vector2.One, Size * .5f, SpriteEffects.None, 0);
            else
                Draw.SpriteBatch.Draw(Type.Source.Texture2D, RenderPosition, Type.Source.Rect, Color, 0, new Vector2(Type.Source.Rect.Width/2, Type.Source.Rect.Height/2), Size * .5f, SpriteEffects.None, 0);
        }

        public Vector2 RenderPosition { get { return Calc.Floor(Position); } }
    }
}
