using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;
using Monocle;

namespace OldSkull
{
    public class SpriteData
    {
        private Atlas atlas;
        private Dictionary<string, XmlElement> sprites;

        public SpriteData(string filename, Atlas atlas)
        {
            this.atlas = atlas;

            XmlDocument xml = new XmlDocument();
            xml.Load(filename);
            sprites = new Dictionary<string, XmlElement>();
            foreach (var e in xml["sprites"])
                if (e is XmlElement)
                    sprites.Add((e as XmlElement).Attr("id"), e as XmlElement);
        }

        public XmlElement GetXML(string id)
        {
            return sprites[id];
        }

        public Sprite<string> GetSpriteString(string id)
        {
            XmlElement xml = sprites[id];

            Sprite<string> sprite = new Sprite<string>(atlas[xml.ChildText("Texture")], xml.ChildInt("FrameWidth"), xml.ChildInt("FrameHeight"));
            sprite.Origin = new Vector2(xml.ChildFloat("OriginX", 0), xml.ChildFloat("OriginY", 0));
            sprite.Position = new Vector2(xml.ChildFloat("X", 0), xml.ChildFloat("Y", 0));
            sprite.Color = xml.ChildHexColor("Color", Color.White);

            XmlElement anims = xml["Animations"];
            if (anims != null)
                foreach (XmlElement anim in anims.GetElementsByTagName("Anim"))
                    sprite.Add(anim.Attr("id"), anim.AttrFloat("delay", 0), anim.AttrBool("loop", true), Calc.ReadCSV(anim.Attr("frames")));

            return sprite;
        }


        public Sprite<int> GetSpriteInt(string id)
        {
            XmlElement xml = sprites[id];

            Sprite<int> sprite = new Sprite<int>(atlas[xml.ChildText("Texture")], xml.ChildInt("FrameWidth"), xml.ChildInt("FrameHeight"));
            sprite.Origin = new Vector2(xml.ChildFloat("OriginX", 0), xml.ChildFloat("OriginY", 0));
            sprite.Position = new Vector2(xml.ChildFloat("X", 0), xml.ChildFloat("Y", 0));
            sprite.Color = xml.ChildHexColor("Color", Color.White);

            XmlElement anims = xml["Animations"];
            if (anims != null)
                foreach (XmlElement anim in anims.GetElementsByTagName("Anim"))
                    sprite.Add(anim.AttrInt("id"), anim.AttrFloat("delay", 0), anim.AttrBool("loop", true), Calc.ReadCSV(anim.Attr("frames")));

            return sprite;
        }
    }
}
