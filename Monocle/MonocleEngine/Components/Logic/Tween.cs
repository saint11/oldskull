using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Monocle
{
    public class Tween : Component
    {
        public enum TweenMode { Persist, Oneshot, Looping, YoyoOneshot, YoyoLooping };

        public Action<Tween> OnUpdate;
        public Action<Tween> OnComplete;
        public Action<Tween> OnStart;

        public TweenMode Mode { get; private set; }
        public int Duration { get; private set; }
        public int FramesLeft { get; private set; }
        public float Percent { get; private set; }
        public float Eased { get; private set; }
        public bool Reverse { get; private set; }
        public Ease.Easer Easer { get; private set; }

        private bool startedReversed;

        public Tween(TweenMode mode, Ease.Easer easer = null, int duration = 1, bool start = false)
            : base(false, false)
        {
#if DEBUG
            if (duration < 1)
                throw new Exception("Tween duration cannot be less than 1");
#endif

            Mode = mode;
            Easer = easer;
            Duration = duration;

            Active = false;

            if (start)
                Start();
        }

        public override void Update()
        {
            FramesLeft--;

            //Update the percentage and eased percentage
            Percent = FramesLeft / (float)Duration;
            if (!Reverse)
                Percent = 1 - Percent;
            if (Easer != null)
                Eased = Easer(Percent);
            else
                Eased = Percent;

            //Update the tween
            if (OnUpdate != null)
                OnUpdate(this);

            //When finished...
            if (FramesLeft == 0)
            {
                if (OnComplete != null)
                    OnComplete(this);

                switch (Mode)
                {
                    case TweenMode.Persist:
                        Active = false;
                        break;

                    case TweenMode.Oneshot:
                        RemoveSelf();
                        break;

                    case TweenMode.Looping:
                        Start(Reverse);
                        break;

                    case TweenMode.YoyoOneshot:
                        if (Reverse == startedReversed)
                        {
                            Start(!Reverse);
                            startedReversed = !Reverse;
                        }
                        else
                            RemoveSelf();
                        break;

                    case TweenMode.YoyoLooping:
                        Start(!Reverse);
                        break;
                }
            }
        }

        public void Start(bool reverse = false)
        {
            startedReversed = Reverse = reverse;

            FramesLeft = Duration;
            Eased = Percent = Reverse ? 1 : 0;

            Active = true;

            if (OnStart != null)
                OnStart(this);
        }

        public void Start(int duration, bool reverse = false)
        {
#if DEBUG
            if (duration < 1)
                throw new Exception("Tween duration cannot be less than 1");
#endif

            Duration = duration;
            Start(reverse);
        }

        public void Stop()
        {
            Active = false;
        }

        static public Tween Set(Entity entity, int duration, Ease.Easer easer, Action<Tween> onUpdate, TweenMode tweenMode = TweenMode.Oneshot)
        {
            Tween tween = new Tween(tweenMode, easer, duration, true);
            tween.OnUpdate += onUpdate;
            entity.Add(tween);
            return tween;
        }

        static public Tween Position(Entity entity, Vector2 targetPosition, int duration, Ease.Easer easer, TweenMode tweenMode = TweenMode.Oneshot)
        {
            Vector2 startPosition = entity.Position;
            Tween tween = new Tween(tweenMode, easer, duration, true);
            tween.OnUpdate = (t) => { entity.Position = Vector2.Lerp(startPosition, targetPosition, t.Eased); };
            entity.Add(tween);
            return tween;
        }

        static public Tween Scale(GraphicsComponent image, Vector2 targetScale, int duration, Ease.Easer easer, TweenMode tweenMode = TweenMode.Oneshot)
        {
            Vector2 startScale = image.Scale;
            Tween tween = new Tween(tweenMode, easer, duration, true);
            tween.OnUpdate = (t) => { image.Scale = Vector2.Lerp(startScale, targetScale, t.Eased); };
            image.Entity.Add(tween);
            return tween;
        }

        static public Tween Alpha(GraphicsComponent image, float targetAlpha, int duration, Ease.Easer easer, TweenMode tweenMode = TweenMode.Oneshot)
        {
            Entity entity = image.Entity;
            float startAlpha = image.Color.A / 255;
            Tween tween = new Tween(tweenMode, easer, duration, true);
            tween.OnUpdate = (t) => { image.Color.A = (byte)Math.Round(MathHelper.Lerp(startAlpha, targetAlpha, t.Eased) * 255.0f); };
            entity.Add(tween);
            return tween;
        }

        public static Tween Position(GraphicsComponent image, Vector2 targetPosition, int duration, Ease.Easer easer, TweenMode tweenMode = TweenMode.Oneshot)
        {
            Vector2 startPosition = image.Position;
            Tween tween = new Tween(tweenMode, easer, duration, true);
            tween.OnUpdate = (t) => { image.Position = Vector2.Lerp(startPosition, targetPosition, t.Eased); };
            image.Entity.Add(tween);
            return tween;
        }
    }
}
