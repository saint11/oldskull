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
            SetLayer(0, new Layer());

            base.Begin();
            
            Add(new Utils.Bouncer(new Image(GameDemo.Atlas["logo"])));
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
