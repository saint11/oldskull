using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle;


namespace OldSkull.Graphics
{
    class Tileset: Entity
    {
        Tilemap image;
        string csv;
        Subtexture texture;
        public Tileset(int layerIndex,string csv,Subtexture texture)
            : base(layerIndex)
        {
            this.csv = csv;
            this.texture = texture;
        }

        public override void Added()
        {
            base.Added();
            GameLevel.PlatformerLevel Level = (GameLevel.PlatformerLevel)Scene;
            image = new Tilemap(Level.Width, Level.Height);

            image.SetTileset(texture,16,16);

            image.BeginTiling();
            image.LoadCSV(csv);
            image.EndTiling();
            Add(image);
        }

    }
}
