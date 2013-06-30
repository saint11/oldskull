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
        //static public SpriteData SpriteData { get; private set; }
        static public SpriteFont Font { get; private set; }
        static public Effect LightingEffect { get; private set; }
        static public EffectParameter LightingAlpha { get; private set; }

        static void Main(string[] args)
        {
            using (GameDemo demo = new GameDemo())
            {
                demo.Run();
            }
        }
        public GameDemo()
            : base(320, 240, 3f, "OldSkull Demo")
        {
           
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>

        protected override void LoadContent()
        {
            base.LoadContent();
            Atlas = new Atlas("Content/Atlas/atlas.xml", true);
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

    }
}
