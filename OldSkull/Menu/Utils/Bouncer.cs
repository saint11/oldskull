using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle;

namespace OldSkull.Menu.Utils
{
    class Bouncer : Entity
    {
        public Bouncer(Image image)
            :base(0)
        {
            Add(image);
            this.image = image;
        }

        public Image image;
        public float xSpeed=1f;
        public float ySpeed=1f;
        public override void Update()
        {
            base.Update();

            if (X > Engine.Instance.Screen.Width - image.Width || X<0) xSpeed *= -1;
            if (Y > Engine.Instance.Screen.Height - image.Height || Y<0) ySpeed *= -1;

            X += xSpeed;
            Y += ySpeed;
        }
    }
}
