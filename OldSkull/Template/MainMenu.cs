using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle;
using OldSkull.Menu;
using OldSkull.Menu.Utils;

namespace OldSkull.Template
{
    class MainMenu : Menu.MainMenu
    {
        private Entity title = new Entity(-1);
        public override void Begin()
        {
            base.Begin();

            KeyboardInput.Add("accept", Microsoft.Xna.Framework.Input.Keys.Z);
            
            Add(new Bouncer(new Image(GameDemo.Atlas["logo"])));

            Image titleImage = new Image(GameDemo.Atlas["title"]);
            title.Add(titleImage);
            title.X = Engine.Instance.Screen.Width / 2 - titleImage.Width / 2;
            Add(title);
        }
    }
}
