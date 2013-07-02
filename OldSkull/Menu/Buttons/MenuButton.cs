using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle;

namespace OldSkull.Menu
{
    class MenuButton : Entity
    {
        
        private Action action;
        public Image image { get; private set; }

        public MenuButton(string imageName, Action action)
            :base(1)
        {
            image = new Image(OldSkullGame.Atlas[imageName]);
            image.CenterOrigin();
            Add(image);
            this.action = action;
        }

        public void press()
        {
            if (action!=null) action();
        }

    }
}
