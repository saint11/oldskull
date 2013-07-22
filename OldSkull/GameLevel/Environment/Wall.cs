using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle;
using Microsoft.Xna.Framework;


namespace OldSkull.GameLevel.Environment
{
    class Wall : Solid
    {

        public Wall(int X, int Y, int W, int H)
            : base(X,Y,W,H)
        {
            Image image = new Image(new Texture(W, H, Color.Black));
            Add(image);
        }
    }
}