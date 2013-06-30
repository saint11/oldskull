using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monocle
{
    public class Alarm : Component
    {
        public enum AlarmMode { Persist, Oneshot, Looping };

        public event Action OnComplete;

        public AlarmMode Mode { get; private set; }
        public int Duration { get; private set; }
        public int FramesLeft { get; private set; }

        public Alarm(AlarmMode mode, int duration = 1, bool start = false)
            : base(false, false)
        {
#if DEBUG
            if (duration < 1)
                throw new Exception("Alarm duration cannot be less than 1");
#endif

            Mode = mode;
            Duration = duration;

            if (start)
                Start();
        }

        public Alarm(AlarmMode mode, Action onComplete, int duration = 1, bool start = false)
            : this(mode, duration, start)
        {
            OnComplete += onComplete;
        }

        public override void Update()
        {
            FramesLeft--;
            if (FramesLeft == 0)
            {
                if (OnComplete != null)
                    OnComplete();

                if (Mode == AlarmMode.Looping)
                    Start();
                else if (Mode == AlarmMode.Oneshot)
                    RemoveSelf();
                else
                    Active = false;
            }
        }

        public void Start()
        {
            Active = true;
            FramesLeft = Duration;
        }

        public void Start(int duration)
        {
#if DEBUG
            if (duration < 1)
                throw new Exception("Alarm duration cannot be less than 1");
#endif

            Duration = duration;
            Start();
        }

        public void Stop()
        {
            Active = false;
        }

        static public Alarm Set(Entity entity, int duration, Action onComplete, AlarmMode alarmMode = AlarmMode.Oneshot)
        {
            Alarm alarm = new Alarm(alarmMode, onComplete, duration, true);
            entity.Add(alarm);
            return alarm;
        }
    }
}
