using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Monocle;

namespace OldSkull.Menu.Utils
{
    public class Bouncer : Entity
    {
        public Vector4 playArea;

        public Bouncer(Image image, Vector4 area = new Vector4())
            :base(0)
        {
            Add(image);
            this.image = image;
            if (area.X == 0 && area.Y == 0 && area.W == 0 && area.Z == 0) 
                area = new Vector4(0, 0, Engine.Instance.Screen.Width, Engine.Instance.Screen.Height);
            playArea = area;
        }

        public Image image;
        public float xSpeed=1f;
        public float ySpeed=1f;
        public override void Update()
        {
            base.Update();

            if (!KeyboardInput.checkInput("accept"))
            {
                if (X > playArea.Z - image.Width || X < playArea.X) xSpeed *= -1;
                if (Y > playArea.W - image.Height || Y<playArea.Y) ySpeed *= -1;

                X += xSpeed;
                Y += ySpeed;
            }
        }
    }
}
