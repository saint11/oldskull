using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monocle
{
    public class IncrementTimer : Component
    {
        public int Value { get; private set; }

        private int counter = 0;
        private Action onComplete;
        private int duration;

        public IncrementTimer(int duration, Action onComplete)
            : base(true, false)
        {
            this.duration = duration;
            this.onComplete = onComplete;
        }

        public override void Update()
        {
            if (counter > 0)
            {
                counter--;
                if (counter == 0)
                {
                    if (onComplete != null)
                        onComplete();
                    Value = 0;
                }
            }
        }

        public void Add(int amount = 1)
        {
            Value += amount;
            counter = duration;
        }

        public void Resolve()
        {
            if (onComplete != null)
                onComplete();
            Value = 0;
            counter = 0;
        }

    }
}
