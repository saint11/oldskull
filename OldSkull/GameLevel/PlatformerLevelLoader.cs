using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle;
using System.Xml;

namespace OldSkull.GameLevel
{
    class PlatformerLevelLoader
    {

        public static void load()
        {
            PlatformerLevel level;
            XmlDocument levelMap = new XmlDocument();
            levelMap.Load(OldSkullGame.Path + @"Content\Level\1.oel");
            level = new PlatformerLevel(levelMap);

            Engine.Instance.Scene = level;
        }
    }
}
