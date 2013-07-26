using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace OldSkull.Menu
{
    public class MainMenu : Scene
    {
        public override void Begin()
        {
            base.Begin();
            Engine.Instance.Screen.ClearColor = Color.PowderBlue;
            SetLayer(1, new Layer(BlendState.NonPremultiplied, SamplerState.PointClamp));
            SetLayer(0, new Layer(BlendState.NonPremultiplied, SamplerState.PointClamp));
            SetLayer(-1, new Layer());

            //Add(new Graphics.ColorBackground(Color.Black,-1));
        }

        public override void Update()
        {
            base.Update();
            KeyboardInput.Update();
        }

    }
}
