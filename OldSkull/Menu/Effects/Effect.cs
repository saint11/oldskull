using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle;

namespace OldSkull.Menu
{
    class Effect
    {
        public int duration;
        public float min;
        public float max;

        private Action<MenuButton, Effect> inFunction;
        private Action<MenuButton, Effect> outFunction;

        public Effect(int duration, float min, float max, Action<MenuButton, Effect> inFunction, Action<MenuButton, Effect> outFunction)
        {
            this.duration = duration;
            this.min = min;
            this.max = max;

            this.inFunction = inFunction;
            this.outFunction = outFunction;
        }

        public void selectFunction(MenuButton button)
        {
            inFunction(button, this);
        }

        public void deselectFunction(MenuButton button)
        {
            outFunction(button, this);
        }
    }
}
