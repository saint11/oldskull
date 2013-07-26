using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle;
using OldSkull;
using Microsoft.Xna.Framework;

namespace OldSkull.Graphics
{
    class TilableBackground :Entity
    {
        private Subtexture texture;
        private Rectangle area;

        public TilableBackground(string image, int layerIndex)
            : base(layerIndex)
        {
            texture = OldSkullGame.Atlas[image];
        }

        public override void Added()
        {
            base.Added();

            GameLevel.PlatformerLevel Level = (GameLevel.PlatformerLevel)Scene;
            area = new Rectangle(0, 0, Level.Width, Level.Height);
        }
        public override void  Render()
        {
 	        base.Render();
            Draw.TextureFill(texture, area);
            
        }

    }
}
