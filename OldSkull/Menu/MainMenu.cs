using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle;

namespace OldSkull.Menu
{
    public class MainMenu : Scene
    {
        public override void Begin()
        {
            base.Begin();

            KeyboardInput.Add("accept",Microsoft.Xna.Framework.Input.Keys.Z);

            SetLayer(0, new Layer());
            Add(new Utils.Bouncer(new Image(GameDemo.Atlas["logo"])));
        }

        public override void Update()
        {
            base.Update();
            KeyboardInput.Update();
        }

    }
}
