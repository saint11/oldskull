using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle;

namespace OldSkull.Menu.Buttons
{
    class MenuButton : Entity
    {
        
        private Action action;
        private Image image;
        public MenuButton(string imageName, Action action)
        {
            image = new Image(GameDemo.Atlas[imageName]);
            this.action = action;
        }

    }
}
