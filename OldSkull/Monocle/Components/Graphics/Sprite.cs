using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Monocle
{
    public class Sprite<T> : Image
    {
        private const float DELTA_TIME = 1f / 60f;

        public Action<Sprite<T>> OnAnimationComplete;
        public Action<Sprite<T>> OnAnimate;
        public Action<Sprite<T>> OnFrameChange;

        public int FramesX { get; private set; }
        public int FramesY { get; private set; }
        public int AnimationFrame { get; private set; }
        public bool Playing { get; private set; }
        public T CurrentAnimID { get; private set; }
        public Rectangle[] FrameRects { get; private set; }
        public float Rate = 1;

        private Dictionary<T, SpriteAnimation> Animations; 
        private int currentFrame;
        private SpriteAnimation currentAnim;        
        private float timer;

        public Sprite(Texture texture, Rectangle? clipRect, int frameWidth, int frameHeight, int frameSep = 0)
            : base(texture, clipRect, true)
        {
            Initialize(frameWidth, frameHeight, frameSep);
        }

        public Sprite(Subtexture subTexture, Rectangle? clipRect, int frameWidth, int frameHeight, int frameSep = 0)
            : base(subTexture, clipRect, true)
        {
            Initialize(frameWidth, frameHeight, frameSep);
        }

        public Sprite(Texture texture, int frameWidth, int frameHeight, int frameSep = 0)
            : this(texture, null, frameWidth, frameHeight, frameSep)
        {

        }

        public Sprite(Subtexture subTexture, int frameWidth, int frameHeight, int frameSep = 0)
            : this(subTexture, null, frameWidth, frameHeight, frameSep)
        {
            
        }

        private void Initialize(int frameWidth, int frameHeight, int frameSep)
        {
            Animations = new Dictionary<T, SpriteAnimation>();

            //Get the amounts of frames
            {
                for (int i = 0; i <= ClipRect.Width - frameWidth; i += frameWidth + frameSep)
                    FramesX++;
                for (int i = 0; i <= ClipRect.Height - frameHeight; i += frameHeight + frameSep)
                    FramesY++;
            }

            //Build the frame rects
            {
                FrameRects = new Rectangle[FramesTotal];
                int x = 0, y = 0;

                for (int i = 0; i < FramesTotal; i++)
                {
                    FrameRects[i] = new Rectangle(ClipRect.X + x, ClipRect.Y + y, frameWidth, frameHeight);

                    if ((i + 1) % FramesX == 0)
                    {
                        x = 0;
                        y += frameHeight + frameSep;
                    }
                    else
                        x += frameWidth + frameSep;
                }
            }
        }

        public override void Update()
        {
            if (Playing && currentAnim.Delay > 0)
            {
                timer += DELTA_TIME * Rate;

                while (timer >= currentAnim.Delay)
                {
                    int oldFrame = currentFrame;
                    timer -= currentAnim.Delay;
                    AnimationFrame++;

                    if (AnimationFrame == currentAnim.Length)
                    {
                        if (currentAnim.Loop)
                        {
                            AnimationFrame = 0;
                            currentFrame = currentAnim[0];

                            if (OnAnimate != null)
                                OnAnimate(this);
                            if (OnFrameChange != null && currentFrame != oldFrame)
                                OnFrameChange(this);
                        }
                        else
                            Stop();

                        if (OnAnimationComplete != null)
                            OnAnimationComplete(this);
                    }
                    else
                    {
                        currentFrame = currentAnim[AnimationFrame];

                        if (OnAnimate != null)
                            OnAnimate(this);
                        if (OnFrameChange != null && currentFrame != oldFrame)
                            OnFrameChange(this);
                    }
                }
            }
        }

        public int CurrentFrame
        {
            get
            {
                return currentFrame;
            }

            set
            {
                if (Playing)
                    Stop();
                if (value != currentFrame)
                {
                    currentFrame = value;
                    if (OnFrameChange != null)
                        OnFrameChange(this);
                }
            }
        }

        public Rectangle CurrentClip
        {
            get
            {
                return FrameRects[currentFrame];
            }
        }

        public int FramesTotal
        {
            get
            {
                return FramesX * FramesY;
            }
        }

        public override int Width
        {
            get
            {
                return FrameRects[0].Width;
            }
        }

        public override int Height
        {
            get
            {
                return FrameRects[0].Height;
            }
        }

        public override void Render()
        {
            Draw.SpriteBatch.Draw(Texture.Texture2D, RenderPosition, CurrentClip, Color, Rotation, Origin, Scale * Zoom, Effects, 0);
        }

        public new void SwapSubtexture(Subtexture setTo, Rectangle? clipRect = null)
        {
            Rectangle old = ClipRect;

            Texture = setTo.Texture;
            ClipRect = clipRect ?? setTo.Rect;

            for (int i = 0; i < FrameRects.Length; i++)
            {
                FrameRects[i].X += ClipRect.X - old.X;
                FrameRects[i].Y += ClipRect.Y - old.Y;
            }
        }

        /*
         *  Animation definition
         */

        public void Add(T id, float delay, bool loop, params int[] frames)
        {
            var anim = new SpriteAnimation(delay, loop, frames);
            Animations.Add(id, anim);
        }

        public void Add(T id, float delay, params int[] frames)
        {
            Add(id, delay, true, frames);
        }

        public void Add(T id, int frame)
        {
            Add(id, 0, false, frame);
        }

        /*
         *  Playing animations
         */

        public void Play(T id, bool restart = false)
        {
            if (!Playing || !CurrentAnimID.Equals(id) || restart)
            {
                CurrentAnimID = id;
                currentAnim = Animations[id];

                AnimationFrame = 0;
                currentFrame = currentAnim[AnimationFrame];
                timer = 0;

                Playing = true;
            }
        }

        public void Play(T id, int startFrame, bool restart = false)
        {
            if (!Playing || !CurrentAnimID.Equals(id) || restart)
            {
                Play(id, true);
                AnimationFrame = startFrame;
                currentFrame = currentAnim[AnimationFrame];
            }
        }

        public void Stop()
        {
            AnimationFrame = 0;
            Playing = false;
        }

        /*
         *  Animation struct
         */

        private struct SpriteAnimation
        {
            public float Delay;
            public int[] Frames;
            public bool Loop;

            public SpriteAnimation(float delay, bool loop, int[] frames)
            {
                Delay = delay;
                Loop = loop;
                Frames = frames;
            }

            public int this[int i]
            {
                get { return Frames[i]; }
            }

            public int Length
            {
                get { return Frames.Length; }
            }
        }
    }
}
