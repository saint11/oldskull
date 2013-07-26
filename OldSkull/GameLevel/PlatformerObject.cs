using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle;
using Microsoft.Xna.Framework;

namespace OldSkull.GameLevel
{
    public class PlatformerObject : PlatformLevelEntity
    {
        //This entity should be abble to detect and act acordingly with
        //all kinds of Collision.
        //Ideally we want this kind of reaction to be turned on or off.

        public Vector2 Speed;
        public Vector2 MaxSpeed;
        public Vector2 Gravity;
        private Vector2 counter;
        protected Vector2 AirDamping;
        protected Vector2 GroundDamping;

        private PlatformerLevel Level;
        protected Boolean onGround = false;

        public Image image;

        public PlatformerObject(Vector2 Position, Vector2 size)
            :base(PlatformerLevel.GAMEPLAY_LAYER)
        {
            this.Position = Position;
            Collider = new Hitbox(size.X, size.Y,-size.X/2,-size.Y/2);
            Speed = new Vector2();
            MaxSpeed = new Vector2(5);
            AirDamping = new Vector2(0.98f, 1);
            GroundDamping = new Vector2(0.9f, 1);
        }
        public override void Added()
        {
            base.Added();
            //TODO: Check if its on a platformerLevel
            Level = (PlatformerLevel)Scene;
            Gravity = Level.Gravity;
        }   

        public override void Step()
        {
            base.Step();
            Speed += Gravity;
            
            if (onGround) Speed *= GroundDamping;
            else Speed *= AirDamping;
            
            LimitMaxSpeed();
            if (Math.Abs(Speed.Y)>0.5) onGround = false;
            Move(Speed, onCollideH, onCollideV);
        }

        private void LimitMaxSpeed()
        {
            if (Math.Abs(Speed.X) > MaxSpeed.X) Speed.X = MaxSpeed.X * Math.Sign(Speed.X);
            if (Math.Abs(Speed.Y) > MaxSpeed.Y) Speed.Y = MaxSpeed.Y * Math.Sign(Speed.Y);
        }

        protected virtual void onCollideH(Solid solid)
        {
            Speed.X = 0;
        }
        protected virtual void onCollideV(Solid solid)
        {
            if (Speed.Y > 0)
            {
                onGround = true;
            }

            Speed.Y = 0;
        }

        public void MoveH(float moveH, Action<Solid> onCollide = null)
        {
            counter.X += moveH;
            int move = (int)Math.Round(counter.X);

            if (move != 0)
            {
                Entity hit;
                int sign = Math.Sign(move);
                counter.X -= move;

                while (move != 0)
                {
                    if ((hit = CollideFirst(GameTags.Solid, X + sign, Y)) != null)
                    {
                        counter.X = 0;
                        if (onCollide != null)
                            onCollide(hit as Solid);
                        break;
                    }

                    X += sign;
                    move -= sign;
                }
            }
        }

        public void MoveV(float moveV, Action<Solid> onCollide = null)
        {
            counter.Y += moveV;
            int move = (int)Math.Round(counter.Y);

            if (move != 0)
            {
                Entity hit;
                int sign = Math.Sign(move);
                counter.Y -= move;

                while (move != 0)
                {
                    if ((hit = CollideFirst(GameTags.Solid, X, Y + sign)) != null)
                    {
                        counter.Y = 0;
                        if (onCollide != null)
                            onCollide(hit as Solid);
                        break;
                    }

                    Y += sign;
                    move -= sign;
                }
            }
        }

        public void Move(Vector2 amount, Action<Solid> onCollideH = null, Action<Solid> onCollideV = null)
        {
            MoveH(amount.X, onCollideH);
            MoveV(amount.Y, onCollideV);
        }
        private Boolean willCollide(Vector2 Position)
        {
            Boolean collided = false;
            Rectangle rect = new Rectangle((int)Position.X, (int)Position.Y, (int)Collider.Width, (int)Collider.Height);
            foreach (Entity e in Level.Solids)
            {
                if (e.Collider.Collide(rect))
                {
                    collided = true;
                    break;
                }
            }
            return collided;
        }

        private Boolean ComparePosition(Vector2 vec1, Vector2 vec2)
        {
            const float threshold = 0.1f;
            Boolean sameX = Math.Abs(vec1.X - vec2.X) <= threshold;
            Boolean sameY = Math.Abs(vec1.Y - vec2.Y) <= threshold;
            return (sameX && sameY);
        }

        #region Animation
        protected virtual void PlayAnim(string animation, bool restart = false)
        {
            if (image is Sprite<string>) ((Sprite<string>)image).Play(animation, restart);
        }

        protected void OnAnimComplete(Action<Sprite<string>> function)
        {
            if (image is Sprite<string>)
            {
                ((Sprite<string>)image).OnAnimationComplete = function;
            }
        }
        protected bool CheckAnim(string p)
        {
            if (image is Sprite<string>)
            {
                Sprite<string> anim = (Sprite<string>)image;
                return anim.CurrentAnimID == p;
            }
            else return false;
        }
        #endregion
    }
}
