using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Monocle
{
    static public class Ease
    {
        public delegate float Easer(float t);

        static public readonly Easer QuadIn = (float t) => { return t * t; };
        static public readonly Easer QuadOut = (float t) => { return 1 - QuadIn(1 - t); };
        static public readonly Easer QuadInOut = (float t) => { return (t <= 0.5f) ? QuadIn(t * 2) / 2 : QuadOut(t * 2 - 1) / 2 + 0.5f; };
        static public readonly Easer CubeIn = (float t) => { return t * t * t; };
        static public readonly Easer CubeOut = (float t) => { return 1 - CubeIn(1 - t); };
        static public readonly Easer CubeInOut = (float t) => { return (t <= 0.5f) ? CubeIn(t * 2) / 2 : CubeOut(t * 2 - 1) / 2 + 0.5f; };
        static public readonly Easer BackIn = (float t) => { return t * t * (2.70158f * t - 1.70158f); };
        static public readonly Easer BackOut = (float t) => { return 1 - BackIn(1 - t); };
        static public readonly Easer BackInOut = (float t) => { return (t <= 0.5f) ? BackIn(t * 2) / 2 : BackOut(t * 2 - 1) / 2 + 0.5f; };
        static public readonly Easer ExpoIn = (float t) => { return (float)Math.Pow(2, 10 * (t - 1)); };
        static public readonly Easer ExpoOut = (float t) => { return 1 - ExpoIn(t); };
        static public readonly Easer ExpoInOut = (float t) => { return t < .5f ? ExpoIn(t * 2) / 2 : ExpoOut(t * 2) / 2; };
        static public readonly Easer SineIn = (float t) => { return -(float)Math.Cos(MathHelper.PiOver2 * t) + 1; };
        static public readonly Easer SineOut = (float t) => { return (float)Math.Sin(MathHelper.PiOver2 * t); };
        static public readonly Easer SineInOut = (float t) => { return -(float)Math.Cos(MathHelper.Pi * t) / 2f + .5f; };
    }
}
