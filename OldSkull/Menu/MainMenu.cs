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
            SetLayer(0, new Layer());
            SetLayer(-1, new Layer());
        }

        public override void Update()
        {
            base.Update();
            KeyboardInput.Update();
        }

    }
}
