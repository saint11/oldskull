using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle;
using Microsoft.Xna.Framework;

namespace OldSkull.GameLevel
{
    class PlatformerObject : Entity
    {
        //This entity should be abble to detect and act acordingly with
        //all kinds of Collision.
        //Ideally we want this kind of reaction to be turned on or off.

        public Vector2 Speed;
        public Vector2 Gravity;

        private PlatformerLevel Level;

        public PlatformerObject(Vector2 position, Vector2 size)
            :base(PlatformerLevel.GAMEPLAY_LAYER)
        {
            Position = position;
            Collider = new Hitbox(size.X, size.Y);
            Speed = new Vector2();
        }
        public override void Added()
        {
            base.Added();
            //TODO: Check if its on a platformerLevel
            Level = (PlatformerLevel)Scene;
            Gravity = Level.Gravity;
        }

        public override void Update()
        {
            base.Update();

            updatePosition();
            Speed += Gravity;
        }

        private void updatePosition()
        {
            Vector2 target = Position + Speed;

            Boolean fail = false;

            //for (int i = 0; i < length; i++)
			//{
			    
			//}

            foreach (Entity e in Level.Solids)
            {
                if (e.Collider != null)
                {
                    
                }
            }

            if (!fail) Position = target;
        }
    }
}
