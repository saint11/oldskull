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

        private Action<Image, Effect> inFunction;
        private Action<Image, Effect> outFunction;

        public Effect ( int duration, float min, float max, Action<Image, Effect> inFunction, Action<Image, Effect> outFunction )
        {
            this.duration = duration;
            this.min = min;
            this.max = max;

            this.inFunction = inFunction;
            this.outFunction = outFunction;
        }

        public void selectFunction ( Image image )
        {
            inFunction(image, this);
        }

        public void deselectFunction ( Image image )
        {
            outFunction(image, this);
        }
    }
}
