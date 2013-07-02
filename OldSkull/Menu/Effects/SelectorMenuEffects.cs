using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle;
using OldSkull.Menu;
using Microsoft.Xna.Framework;

namespace OldSkull.Menu
{
    abstract class SelectorMenuEffects
    {
        public static Effect Fade = new Effect(20, 0.5f, 1f, FadeIn, FadeOut);
        public static Effect Scale = new Effect(20, 0.85f, 1.2f, ScaleUp, ScaleDown);

        public static Effect ScaleYoYo = new Effect( 10, 0.85f, 1.2f, ScaleUpYoYo, ScaleDownYoYo );


        private static void ScaleUp(Image image, Effect effect)
        {
            Tween.Scale(image, new Vector2(effect.max), effect.duration, Ease.BackInOut, Tween.TweenMode.Oneshot);
        }

        private static void ScaleDown(Image image, Effect effect)
        {
            Tween.Scale(image, new Vector2(effect.min), effect.duration, Ease.BackInOut, Tween.TweenMode.Oneshot);
        }

        private static void ScaleUpYoYo ( Image image, Effect effect )
        {
            Tween.Scale( image, new Vector2( effect.max ), effect.duration, Ease.BackInOut, Tween.TweenMode.YoyoOneshot );
        }

        private static void ScaleDownYoYo ( Image image, Effect effect )
        {
            Tween.Scale( image, new Vector2( effect.min ), effect.duration, Ease.BackInOut, Tween.TweenMode.YoyoOneshot );
        }

        private static void FadeOut(Image image, Effect effect)
        {
            Tween.Alpha(image, effect.min, effect.duration, Ease.CubeOut, Tween.TweenMode.Oneshot);
        }

        private static void FadeIn(Image image, Effect effect)
        {
            Tween.Alpha(image, effect.max, effect.duration, Ease.CubeOut, Tween.TweenMode.Oneshot);
        }
    }
}
