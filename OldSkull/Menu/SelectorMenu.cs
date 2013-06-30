using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle;

namespace OldSkull.Menu
{
    class SelectorMenu : Entity
    {
        private List<Buttons.MenuButton> menuButtons;

        public SelectorMenu(List<String> buttomImages, List<Action> buttomFunctions)
            :base(0)
        {
            if (buttomImages.Count != buttomFunctions.Count)
                throw new System.ArgumentException("Lists have different lengths");

            for (int i = 0; i < buttomImages.Count; i++)
			{
                menuButtons.Add(new Buttons.MenuButton(buttomImages[i], buttomFunctions[i]));
			}
        }
    }
}
