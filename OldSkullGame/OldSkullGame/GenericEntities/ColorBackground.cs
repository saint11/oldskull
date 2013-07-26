using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle;
using Microsoft.Xna.Framework;

namespace OldSkull.GenericEntities
{
    class ColorBackground : Entity
    {
        public ColorBackground(Color color, int layer)
            :base(layer)
        {
            Image image = new Image(new Texture(Engine.Instance.Screen.Width, Engine.Instance.Screen.Height,color));
            Add(image);
        }
    }
}
