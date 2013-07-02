using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle;
using Microsoft.Xna.Framework;


namespace OldSkull.GameLevel.Environment
{
    class Wall : Entity
    {

        public Wall(int X, int Y, int W, int H)
            : base(PlatformerLevel.GAMEPLAY_LAYER)
        {
            this.X = X;
            this.Y = Y;

            Collider = new Hitbox(W,H);
            Image image = new Image(new Texture(W, H, Color.Black));
            Add(image);
        }
    }
}