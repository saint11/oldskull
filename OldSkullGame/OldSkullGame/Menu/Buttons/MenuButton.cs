using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle;

namespace OldSkull.Menu
{
    public class MenuButton : Entity
    {

        private Action<MenuButton> action;
        public Action<int> DefaultFunction;
        private int Index;
        public GraphicsComponent image { get; private set; }
        public SelectorMenu SelectorMenu { get; private set; }

        public MenuButton(SelectorMenu SelectorMenu, GraphicsComponent image, Action<MenuButton> action, Action<int> DefaultFunction, int Index, int layer)
            :base(layer)
        {
            this.SelectorMenu = SelectorMenu;
            this.image = image;
            this.Index = Index;
            image.CenterOrigin();
            Add(image);
            this.action = action;
            this.DefaultFunction = DefaultFunction;
        }

        public void press()
        {
            if (action!=null) action(this);
            if (DefaultFunction != null) DefaultFunction(Index);
        }

    }
}
