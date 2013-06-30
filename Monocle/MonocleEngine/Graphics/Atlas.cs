using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using Microsoft.Xna.Framework.Graphics;

namespace Monocle
{
    public class Atlas : Texture
    {
        public Dictionary<string, Subtexture> SubTextures { get; private set; }

        private string xmlPath;

        public Atlas(string xmlPath, bool load)
        {
            XmlPath = xmlPath;

            XmlDocument xml = new XmlDocument();
            xml.Load(xmlPath);
            XmlElement atlas = xml["TextureAtlas"];

            ImagePath = Path.Combine(Path.GetDirectoryName(xmlPath), atlas.Attributes["imagePath"].Value);
            var subTextures = atlas.GetElementsByTagName("SubTexture");
            SubTextures = new Dictionary<string, Subtexture>(subTextures.Count);

            foreach (XmlElement subTexture in subTextures)
            {
                var a = subTexture.Attributes;
                SubTextures.Add(a["name"].Value, new Subtexture(
                    this,
                    Convert.ToInt32(a["x"].Value),
                    Convert.ToInt32(a["y"].Value),
                    Convert.ToInt32(a["width"].Value),
                    Convert.ToInt32(a["height"].Value))
                    );
            }

            if (load)
                Load();
        }

        public Subtexture this[string name]
        {
            get
            {
                if (!SubTextures.ContainsKey(name))
                    throw new Exception("SubTexture does not exist: " + name);
                return SubTextures[name];
            }
        }

        public string XmlPath
        {
            get { return xmlPath; }
            internal set
            {
                xmlPath = value;
#if DEBUG
                if (!File.Exists(xmlPath))
                    throw new Exception("File does not exist: " + xmlPath);
#endif
            }
        }
    }
}
