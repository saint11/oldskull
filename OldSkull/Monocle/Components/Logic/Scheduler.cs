using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Monocle
{
    public class Scheduler : Component
    {
        private List<Action> actions;
        private List<int> counters;
        private List<int> startCounters;
        private int performing = -1;
        private int relativeMarker = 0;

        public Scheduler()
            : base(true, false)
        {
            actions = new List<Action>();
            counters = new List<int>();
            startCounters = new List<int>();
        }

        public override void Update()
        {
            for (performing = 0; performing < actions.Count; performing++)
            {
                counters[performing]--;
                if (counters[performing] == 0)
                {
                    Action action = actions[performing];

                    if (startCounters[performing] != 0)
                        counters[performing] = startCounters[performing];
                    else
                    {
                        actions.RemoveAt(performing);
                        counters.RemoveAt(performing);
                        startCounters.RemoveAt(performing);
                        performing--;
                    }

                    action();
                }
            }
            performing = -1;
        }

        public void ScheduleAction(Action action, int delay, bool looping = false)
        {
#if DEBUG
            if (action == null)
                throw new Exception("Cannot schedule a null Action!");
            if (delay == 0)
                throw new Exception("Cannot schedule an Action with a delay of zero!");
#endif
            actions.Add(action);
            counters.Add(delay);
            startCounters.Add(looping ? delay : 0);
        }

        public void UnscheduleAction(Action action)
        {
            for (int i = 0; i < actions.Count; i++)
            {
                if (actions[i] == action)
                {
                    actions.RemoveAt(i);
                    counters.RemoveAt(i);
                    startCounters.RemoveAt(i);
                    i--;

                    if (performing >= i)
                        performing--;
                }
            }
        }

        public void RelativeSchedule(Action action, int relativeDelay)
        {
            relativeMarker += relativeDelay;
            ScheduleAction(action, relativeMarker);
        }

        public void ResetRelativeMarker()
        {
            relativeMarker = 0;
        }

        public void RelativePad(int padFrames)
        {
            relativeMarker += padFrames;
        }

        public void ClearAllActions()
        {
            actions.Clear();
            counters.Clear();
            startCounters.Clear();
        }
    }
}
