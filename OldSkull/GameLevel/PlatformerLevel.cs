using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OldSkull.GameLevel
{
    public class PlatformerLevel : Scene
    {
        public enum GameMode { Quest };

        //Layers
        private Layer bgGameLayer;
        private Layer gameLayer;
        private Layer hudLayer;
        private Layer pauseLayer;

        //Layer Constants
        public static readonly int BG_GAME_LAYER = -3;
        public static readonly int GAMEPLAY_LAYER = 0;
        public static readonly int HUD_LAYER = 3;
        public static readonly int PAUSE_LAYER = 4;
        public static readonly int REPLAY_LAYER = 10;

        //LevelLoader Variables
        private int width;
        private int height;
        public Vector2 Gravity = new Vector2(0f,0.1f);

        //Lists
        public List<Entity> Solids {get;private set;}

        public PlatformerLevel(int width, int height)
        {
            this.width = width;
            this.height = height;

            SetLayer(BG_GAME_LAYER, bgGameLayer = new Layer());
            SetLayer(GAMEPLAY_LAYER, gameLayer = new Layer());
            SetLayer(HUD_LAYER, hudLayer = new Layer(BlendState.AlphaBlend, SamplerState.PointClamp, 0));
            SetLayer(PAUSE_LAYER, pauseLayer = new Layer(BlendState.AlphaBlend, SamplerState.PointClamp, 0));

            Solids = new List<Entity>();

            //Test Stuff
            PlatformerObject po = new PlatformerObject(new Vector2(20), new Vector2(20));
            po.Add(new Image(new Monocle.Texture(20, 20, Color.Brown)));
            Add(po);
        }

        internal void addWall(XmlElement e)
        {
            Environment.Wall wall = new Environment.Wall(int.Parse(e.Attr("x")), int.Parse(e.Attr("y")), int.Parse(e.Attr("w")), int.Parse(e.Attr("h")));
            Add(wall);
            Solids.Add(wall);
        }
    }
}
