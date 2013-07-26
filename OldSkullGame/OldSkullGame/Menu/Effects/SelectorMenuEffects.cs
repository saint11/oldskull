using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle;
using OldSkull.Menu;
using Microsoft.Xna.Framework;

namespace OldSkull.Menu
{
    public abstract class SelectorMenuEffects
    {
        public static Effect Fade = new Effect(20, 0.5f, 1f, FadeIn, FadeOut);
        public static Effect Scale = new Effect(20, 0.85f, 1.2f, ScaleUp, ScaleDown);

        public static Effect ScaleYoYo = new Effect(10, 0.85f, 1.2f, ScaleUpYoYo, ScaleDownYoYo);
        public static Effect Outline = new Effect(10, 0.85f, 1.2f, OutlineIn, OutlineOut);


        private static void ScaleUp(GraphicsComponent image, Effect effect)
        {
            Tween.Scale(image, new Vector2(effect.max), effect.duration, Ease.BackInOut, Tween.TweenMode.Oneshot);
        }

        private static void ScaleDown(GraphicsComponent image, Effect effect)
        {
            Tween.Scale(image, new Vector2(effect.min), effect.duration, Ease.BackInOut, Tween.TweenMode.Oneshot);
        }

        private static void ScaleUpYoYo(GraphicsComponent image, Effect effect)
        {
            Tween.Scale( image, new Vector2( effect.max ), effect.duration, Ease.BackInOut, Tween.TweenMode.YoyoOneshot );
        }

        private static void ScaleDownYoYo(GraphicsComponent image, Effect effect)
        {
            Tween.Scale( image, new Vector2( effect.min ), effect.duration, Ease.BackInOut, Tween.TweenMode.YoyoOneshot );
        }

        private static void FadeOut(GraphicsComponent image, Effect effect)
        {
            Tween.Alpha(image, effect.min, effect.duration, Ease.CubeOut, Tween.TweenMode.Oneshot);
        }

        private static void FadeIn(GraphicsComponent image, Effect effect)
        {
            Tween.Alpha(image, effect.max, effect.duration, Ease.CubeOut, Tween.TweenMode.Oneshot);
        }

        internal static void OutlineIn(GraphicsComponent image, Effect effect)
        {
            image.outlined = true;
            image.outlineColor = effect.outline;
        }

        internal static void OutlineOut(GraphicsComponent image, Effect effect)
        {
            image.outlined = false;
        }

        public static Effect ColorSwap(Color Selected, Color NotSelected)
        {
            Effect effect = new Effect(10, 0.85f, 1.2f, SelectorMenuEffects.ColorIn, SelectorMenuEffects.ColorOut);
            effect.outline = Color.Black;
            effect.selectedColor = Selected;
            effect.deselectedColor = NotSelected;

            return effect;
        }

        internal static void ColorIn(GraphicsComponent image, Effect effect)
        {
            image.outlined = true;
            image.Color = effect.selectedColor;
            image.outlineColor = effect.outline;
        }

        internal static void ColorOut(GraphicsComponent image, Effect effect)
        {
            image.outlined = false;
            image.Color = effect.deselectedColor;
        }
    }
}
