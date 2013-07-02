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
    class PlatformerLevel : Scene
    {
        public enum GameMode { Quest };

        private XmlDocument xml;

        //Layers
        private Layer bgGameLayer;
        private Layer gameLayer;
        private Layer hudLayer;
        private Layer pauseLayer;

        //Layer Constants
        public const int BG_GAME_LAYER = -3;
        public const int GAMEPLAY_LAYER = 0;
        public const int HUD_LAYER = 3;
        public const int PAUSE_LAYER = 4;
        public const int REPLAY_LAYER = 10;

        public PlatformerLevel(XmlDocument levelXml)
        {
            xml = levelXml;

            SetLayer(BG_GAME_LAYER, bgGameLayer = new Layer());
            SetLayer(GAMEPLAY_LAYER, gameLayer = new Layer());
            SetLayer(HUD_LAYER, hudLayer = new Layer(BlendState.AlphaBlend, SamplerState.PointClamp, 0));
            SetLayer(PAUSE_LAYER, pauseLayer = new Layer(BlendState.AlphaBlend, SamplerState.PointClamp, 0));
        }
    }
}
