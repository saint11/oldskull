using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle;
using Microsoft.Xna.Framework;

namespace OldSkull.GameLevel
{
    public class PlayerObject : PlatformerObject
    {
        private static Vector2 MAX_SPEED = new Vector2(1.2f, 3);

        //Properties
        protected int CONTEXT_MENU_TIMER = 30;
        protected bool CAN_CROUCH = true;
        
        private string imageName;

        public int side {get; private set;}

        protected bool Crouching = false;


        protected bool LetGo = false;
        private int useKeyTimer = 0;
        private bool UsingItem = false;
        private int Invulnerable=0;

        protected bool JustTalked = false;

        public PlayerObject(Vector2 position, Vector2 size, string imageName="")
            : base(position, size)
        {
            Tag(GameTags.Player);
            this.imageName = imageName;
            AirDamping.X = 0.9f;
            GroundDamping.X = 0.9f;

            MaxSpeed = MAX_SPEED;
            if (imageName != "")
            {
                image = OldSkullGame.SpriteData.GetSpriteString(imageName);
                PlayAnim("idle");
            } else {
                image = new Image(new Texture((int)size.X, (int)size.Y, Color.AliceBlue));
                image.CenterOrigin();
             }
            Add(image);
            side = 1;
        }

        public override void Step()
        {
            base.Step();

            TrackPosition();
            UpdateColisions();
            UpdateControls();

            if (!onGround)
            {
                if (Speed.Y > 0)
                    PlayAnim("jumpDown");
                else if (Speed.Y < 0) PlayAnim("jumpUp");
            }

            Invulnerable--;
        }


        public virtual void DefaultUse()
        {
            
            
        }

        protected virtual void UpdateColisions()
        {
            
        }

        protected void TakeDamage(float damage, Vector2 source)
        {
            if (Invulnerable <= 0)
            {
                Invulnerable = 50;
                OnTakeDamage(damage, source);
            }
        }

        protected virtual void OnTakeDamage(float damage, Vector2 source)
        {
        }

        private void TrackPosition()
        {
            PlatformerLevel Level = (PlatformerLevel)Scene;
            if (X > Level.Width)
            {
                Level.OutOfBounds(PlatformerLevel.Side.Right);
            }
            else if (X < 0)
            {
                Level.OutOfBounds(PlatformerLevel.Side.Left);
            }
        }

        private void UpdateControls()
        {
            if (!UsingItem)
            {
                if (!Crouching)
                {
                    if (!HorizontalInput())
                    {
                        if (onGround && !Crouching && !CheckAnim("crouchOut"))
                            PlayAnim("idle");
                        Speed.X *= 0.9f;
                    }

                    if (KeyboardInput.pressedInput("jump"))
                    {
                        OnJump();
                    }
                    else if (!KeyboardInput.checkInput("jump") && (Speed.Y < 0))
                    {
                        Speed.Y *= 0.7f;
                    }


                    if (KeyboardInput.checkInput("use"))
                    {
                            useKeyTimer++;

                            if (useKeyTimer >= CONTEXT_MENU_TIMER)
                            {
                                OnUseHold();
                            }
                    }
                    else
                    {
                        if (useKeyTimer > 0 && useKeyTimer < CONTEXT_MENU_TIMER)
                        {
                            if (!JustTalked)
                            {
                                DefaultUse();
                            }
                            else
                                JustTalked = false;
                        }
                        else
                            JustTalked = false;
                        useKeyTimer = 0;
                    }

                }

                //Crouching and Pickup
                if (!onGround) Crouching = false;
                if (KeyboardInput.checkInput("down"))
                {
                    OnCrouching();
                }
                else
                {
                    LetGo = false;
                    if (Crouching)
                    {
                        OnStandUp();
                    }
                    else if (KeyboardInput.pressedInput("up"))
                    {
                        OnPressedUp();
                    }
                }
            }
        }

        protected virtual void OnJump()
        {
            if (onGround)
            {
                Speed.Y = -3.8f;
                Utils.Sounds.Play("jump");
            }
        }

        protected virtual void OnPressedUp()
        {

        }

        protected virtual void OnCrouching()
        {
            useKeyTimer = 0;
            if (!Crouching && !LetGo && onGround)
            {
                PlayAnim("crouchIn", true);
                Crouching = true;
            }
        }

        protected virtual void OnStandUp()
        {
            PlayAnim("crouchOut", true);
            OnAnimComplete(CompleteAnimation);
            Crouching = false;
        }

        protected virtual void OnUseHold()
        {
            UsingItem = true;
        }

        private bool HorizontalInput()
        {
            if (Math.Abs(KeyboardInput.xAxis) > 0)
            {
                Speed.X += KeyboardInput.xAxis * 0.2f;
                if (KeyboardInput.xAxis < 0)
                {
                    side = -1;
                    image.FlipX = true;
                }
                else
                {
                    image.FlipX = false;
                    side = 1;
                }

                if (onGround && !Crouching) PlayAnim("walk");

                return true;
            }
            return false;
        }

        public void stopUsing()
        {
            UsingItem = false;
        }

        public void CompleteAnimation(Sprite<String> sprite)
        {
            if (sprite.CurrentAnimID=="crouchOut") sprite.Play("idle", true);
        }

        public override void Render()
        {
            if (Invulnerable > 0 && Invulnerable % 10 < 5)
            {
                image.RenderFilled(Color.Chocolate);
            }
            else
            {
                base.Render();
            }
        }


        internal void OnWater()
        {
            MaxSpeed = new Vector2(MAX_SPEED.X / 2,MAX_SPEED.Y*0.8f);
        }

        internal void ExitWater()
        {
            MaxSpeed = MAX_SPEED;
        }

        public virtual void onDrowning()
        {
        }
    }
}
