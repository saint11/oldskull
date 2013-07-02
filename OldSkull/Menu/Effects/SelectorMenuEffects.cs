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


        private static void ScaleUp(MenuButton button, Effect effect)
        {
            Tween.Scale(button.image, new Vector2(effect.max), effect.duration, Ease.BackInOut, Tween.TweenMode.Oneshot);
        }

        private static void ScaleDown(MenuButton button, Effect effect)
        {
            Tween.Scale(button.image, new Vector2(effect.min), effect.duration, Ease.BackInOut, Tween.TweenMode.Oneshot);
        }

        private static void FadeOut(MenuButton button, Effect effect)
        {
            Tween.Alpha(button.image, effect.min, effect.duration, Ease.CubeOut, Tween.TweenMode.Oneshot);
        }

        private static void FadeIn(MenuButton button, Effect effect)
        {
            Tween.Alpha(button.image, effect.max, effect.duration, Ease.CubeOut, Tween.TweenMode.Oneshot);
        }
    }
}
