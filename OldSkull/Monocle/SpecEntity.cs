using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Monocle
{
    /*
     *  An Entity subclass that can only be added to one type of Scene which you specify.
     *  Covers the Entity Scene reference with one that returns it as the specified
     *  type, to avoid annoying casting everywhere.
     */

    public class SpecEntity<T> : Entity where T : Scene
    {
        public new T Scene { get; private set; }

        public SpecEntity(Vector2 position, int layerIndex = 0)
            : base(position, layerIndex)
        {

        }

        public SpecEntity(int layerIndex = 0)
            : this(Vector2.Zero, layerIndex)
        {

        }

        public override void Added()
        {
            base.Added();

#if DEBUG
            if (!(base.Scene is T))
                throw new Exception("SpecEntity added to incompatible Scene type");
#endif
            Scene = base.Scene as T;
        }

        public override void Removed()
        {
            base.Removed();
            Scene = null;
        }
    }
}
