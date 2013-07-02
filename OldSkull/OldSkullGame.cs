#region Using Statements
using Monocle;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.IO;
using System.Collections.Generic;
using System.Collections;
#endregion

namespace OldSkull
{
    
    public class OldSkullGame : Engine
    {
        static public Atlas Atlas { get; private set; }
        public const string Path = @"Assets\";

        static void Main(string[] args)
        {
            using (OldSkullGame demo = new OldSkullGame())
            {
                demo.Run();
            }
        }

        public OldSkullGame()
            : base(320, 240, 60f, "OldSkull Demo")
        {
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            Atlas = new Atlas("Assets/Content/Atlas/atlas.xml", true);
        }

        protected override void Initialize()
        {
            base.Initialize();
            Screen.Scale = 2f;

            KeyboardInput.InitDefaultInput();
            Scene = new Template.MainMenu();
        }
    }
}
