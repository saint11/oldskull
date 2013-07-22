using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle;

namespace OldSkull.GameLevel
{
    public class Solid : Entity
    {
        public Solid(int X, int Y, int W, int H)
            : base(PlatformerLevel.GAMEPLAY_LAYER)
        {
            this.X = X;
            this.Y = Y;

            Collider = new Hitbox(W, H);
            Tag(GameTags.Solid);
        }
           
    }
}
