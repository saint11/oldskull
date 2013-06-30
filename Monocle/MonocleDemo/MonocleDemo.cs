using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace MonocleDemo
{
    class MonocleDemo : Engine
    {
        public static Atlas Atlas;

        static void Main(string[] args)
        {
            using (MonocleDemo demo = new MonocleDemo())
            {
                
                demo.Run();
            }
        }

        public MonocleDemo()
            : base(1280, 720, 60, "Demo")
        {

        }

        protected override void LoadContent()
        {
            base.LoadContent();
            Atlas = new Atlas("Content/Graphics/graphics_1.xml", true);
        }

        protected override void Initialize()
        {
            base.Initialize();
        }
    }
}
