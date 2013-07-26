using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle;
using Microsoft.Xna.Framework;

namespace OldSkull.Menu
{
    public class Effect
    {
        public int duration;
        public float min;
        public float max;

        private Action<GraphicsComponent, Effect> inFunction;
        private Action<GraphicsComponent, Effect> outFunction;

        public Color selectedColor;
        public Color deselectedColor;
        public Color outline;

        public Effect(int duration, float min, float max, Action<GraphicsComponent, Effect> inFunction, Action<GraphicsComponent, Effect> outFunction)
        {
            this.duration = duration;
            this.min = min;
            this.max = max;

            this.inFunction = inFunction;
            this.outFunction = outFunction;
        }

        public void selectFunction ( GraphicsComponent image )
        {
            inFunction(image, this);
        }

        public void deselectFunction(GraphicsComponent image)
        {
            outFunction(image, this);
        }
    }
}
