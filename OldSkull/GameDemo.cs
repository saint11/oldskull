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
    
    public class GameDemo : Engine
    {
        static public Atlas Atlas { get; private set; }

        static void Main(string[] args)
        {
            using (GameDemo demo = new GameDemo())
            {
                demo.Run();
            }
        }

        public GameDemo()
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
