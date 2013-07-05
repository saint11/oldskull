using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle;
using System.Xml;

namespace OldSkull.GameLevel
{
    public class PlatformerLevelLoader
    {

        public static PlatformerLevel load()
        {
            PlatformerLevel level;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(OldSkullGame.Path + @"Content\Level\1.oel");
            XmlElement levelMap = xmlDoc["level"];

            level = new PlatformerLevel(int.Parse(levelMap.Attr("width")), int.Parse(levelMap.Attr("height")));
            foreach (XmlElement e in levelMap["Solid"])
            {
                level.addWall(e);
            }

            return level;
        }
    }
}
