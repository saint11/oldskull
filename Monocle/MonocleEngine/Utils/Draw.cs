using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Monocle
{
    public static class Draw
    {
        static public SpriteBatch SpriteBatch { get; private set; }
        static public Texture2D Pixel { get; private set; }
        static public Texture2D Particle { get; private set; }
        static public SpriteFont DefaultFont { get; private set; }

        static private Rectangle rect;

        static internal void Init(GraphicsDevice graphicsDevice)
        {
            SpriteBatch = new SpriteBatch(graphicsDevice);
            Pixel = new Texture2D(graphicsDevice, 1, 1);
            Pixel.SetData<Color>(new Color[1] { Color.White });
            Particle = new Texture2D(graphicsDevice, 2, 2);
            Particle.SetData<Color>(new Color[4] { Color.White, Color.White, Color.White, Color.White });
            DefaultFont = Engine.Instance.Content.Load<SpriteFont>("../MonocleDefault");
        }

        static public void SetTarget(Canvas canvas)
        {
            Engine.Instance.GraphicsDevice.SetRenderTarget(canvas.RenderTarget2D);
        }

        static public void ResetTarget()
        {
            Engine.Instance.GraphicsDevice.SetRenderTarget(Engine.Instance.Screen.RenderTarget);
        }

        static public void Clear(Color color)
        {
            Engine.Instance.GraphicsDevice.Clear(color);
        }

        static public void Clear()
        {
            Engine.Instance.GraphicsDevice.Clear(Color.Transparent);
        }

        static public void Line(Vector2 start, Vector2 end, Color color)
        {
            LineAngle(start, Calc.Angle(start, end), Vector2.Distance(start, end), color);
        }

        static public void Line(float x1, float y1, float x2, float y2, Color color)
        {
            Line(new Vector2(x1, y1), new Vector2(x2, y2), color);
        }

        static public void LineAngle(Vector2 start, float angle, float length, Color color)
        {
            SpriteBatch.Draw(Pixel, start, null, color, angle, Vector2.Zero, new Vector2(length, 1), SpriteEffects.None, 0);
        }

        static public void LineAngle(float startX, float startY, float angle, float length, Color color)
        {
            LineAngle(new Vector2(startX, startY), angle, length, color);
        }

        static public void Rect(float x, float y, float width, float height, Color color)
        {
            rect.X = (int)x;
            rect.Y = (int)y;
            rect.Width = (int)width;
            rect.Height = (int)height;
            SpriteBatch.Draw(Pixel, rect, color);
        }

        static public void Rect(Rectangle rect, Color color)
        {
            Draw.rect = rect;
            SpriteBatch.Draw(Pixel, rect, color);
        }

        static public void HollowRect(float x, float y, float width, float height, Color color)
        {
            rect.X = (int)x;
            rect.Y = (int)y;
            rect.Width = (int)width;
            rect.Height = 1;

            SpriteBatch.Draw(Pixel, rect, color);

            rect.Y += (int)height - 1;

            SpriteBatch.Draw(Pixel, rect, color);

            rect.Y -= (int)height - 1;
            rect.Width = 1;
            rect.Height = (int)height;

            SpriteBatch.Draw(Pixel, rect, color);

            rect.X += (int)width - 1;

            SpriteBatch.Draw(Pixel, rect, color);
        }

        static public void Text(SpriteFont font, string text, Vector2 position, Color color)
        {
            Draw.SpriteBatch.DrawString(font, text, Calc.Floor(position), color);
        }

        static public void Text(SpriteFont font, string text, Vector2 position, Color color, Vector2 origin, Vector2 scale, float rotation)
        {
            Draw.SpriteBatch.DrawString(font, text, Calc.Floor(position), color, rotation, origin, scale, SpriteEffects.None, 0);
        }

        static public void TextJustify(SpriteFont font, string text, Vector2 position, Color color, Vector2 justify)
        {
            Vector2 origin = font.MeasureString(text);
            origin.X *= justify.X;
            origin.Y *= justify.Y;

            Draw.SpriteBatch.DrawString(font, text, Calc.Floor(position), color, 0, origin, 1, SpriteEffects.None, 0);
        }

        static public void TextJustify(SpriteFont font, string text, Vector2 position, Color color, float scale, Vector2 justify)
        {
            Vector2 origin = font.MeasureString(text);
            origin.X *= justify.X;
            origin.Y *= justify.Y;
            Draw.SpriteBatch.DrawString(font, text, Calc.Floor(position), color, 0, origin, scale, SpriteEffects.None, 0);
        }

        static public void TextCentered(SpriteFont font, string text, Vector2 position, Color color)
        {
            Text(font, text, position - font.MeasureString(text) * .5f, color);
        }

        static public void TextCentered(SpriteFont font, string text, Vector2 position, Color color, Vector2 scale, float rotation)
        {
            Text(font, text, position, color, font.MeasureString(text) * .5f, scale, rotation);
        }

        static public void TextRight(SpriteFont font, string text, Vector2 position, Color color)
        {
            Vector2 origin = font.MeasureString(text);
            origin.Y /= 2f;

            Text(font, text, position - origin, color);
        }

        static public void TextRight(SpriteFont font, string text, Vector2 position, Color color, Vector2 scale, float rotation)
        {
            Vector2 origin = font.MeasureString(text);
            origin.Y /= 2f;

            Text(font, text, position, color, origin, scale, rotation);
        }

        static public void Texture(Texture texture, Vector2 position, Color color)
        {
            SpriteBatch.Draw(texture.Texture2D, Calc.Floor(position), null, color);
        }

        static public void Texture(Texture texture, Rectangle clipRect, Vector2 position, Color color)
        {
            SpriteBatch.Draw(texture.Texture2D, Calc.Floor(position), clipRect, color);
        }

        static public void Texture(Subtexture subTexture, Vector2 position, Color color)
        {
            SpriteBatch.Draw(subTexture.Texture.Texture2D, Calc.Floor(position), subTexture.Rect, color);
        }

        static public void Texture(Subtexture subTexture, Vector2 position, Color color, Vector2 origin, float scale, float rotation)
        {
            SpriteBatch.Draw(subTexture.Texture.Texture2D, Calc.Floor(position), subTexture.Rect, color, rotation, origin, scale, SpriteEffects.None, 0);
        }

        static public void Texture(Subtexture subTexture, Rectangle clipRect, Vector2 position, Color color)
        {
            SpriteBatch.Draw(subTexture.Texture.Texture2D, Calc.Floor(position), subTexture.GetAbsoluteClipRect(clipRect), color);
        }

        static public void TextureCentered(Subtexture subTexture, Vector2 position, Color color)
        {
            SpriteBatch.Draw(subTexture.Texture.Texture2D, Calc.Floor(position), subTexture.Rect, color, 0, new Vector2(subTexture.Rect.Width / 2, subTexture.Rect.Height / 2), 1, SpriteEffects.None, 0);
        }

        static public void TextureCentered(Subtexture subTexture, Rectangle clipRect, Vector2 position, Color color)
        {
            SpriteBatch.Draw(subTexture.Texture.Texture2D, Calc.Floor(position), subTexture.GetAbsoluteClipRect(clipRect), color, 0, new Vector2(clipRect.Width / 2, clipRect.Height / 2), 1, SpriteEffects.None, 0);
        }

        static public void TextureCentered(Subtexture subTexture, Vector2 position, Color color, float scale, float rotation)
        {
            SpriteBatch.Draw(subTexture.Texture.Texture2D, Calc.Floor(position), subTexture.Rect, color, rotation, new Vector2(subTexture.Rect.Width/2, subTexture.Rect.Height/2), scale, SpriteEffects.None, 0);
        }

        static public void TextureCentered(Subtexture subTexture, Vector2 position, Color color, Vector2 scale, float rotation)
        {
            SpriteBatch.Draw(subTexture.Texture.Texture2D, Calc.Floor(position), subTexture.Rect, color, rotation, new Vector2(subTexture.Rect.Width / 2, subTexture.Rect.Height / 2), scale, SpriteEffects.None, 0);
        }

        static public void TextureCentered(Subtexture subTexture, Rectangle clipRect, Vector2 position, Color color, float scale, float rotation)
        {
            SpriteBatch.Draw(subTexture.Texture.Texture2D, Calc.Floor(position), subTexture.GetAbsoluteClipRect(clipRect), color, rotation, new Vector2(clipRect.Width / 2, clipRect.Height / 2), scale, SpriteEffects.None, 0);
        }

        static public void SineTextureV(Texture texture, Rectangle clipRect, Vector2 position, Vector2 origin, Vector2 scale, float rotation, Color color, SpriteEffects effects, float sineCounter, float amplitude = 2, int sliceSize = 2, float sliceAdd = MathHelper.TwoPi / 8)
        {
            position = Calc.Floor(position);
            Rectangle clip = clipRect;
            clip.Height = sliceSize;

            int num = 0;
            while (clip.Y < clipRect.Y + clipRect.Height)
            {
                Vector2 add = new Vector2((float)Math.Round(Math.Sin(sineCounter + sliceAdd * num) * amplitude), sliceSize * num);
                Draw.SpriteBatch.Draw(texture.Texture2D, position, clip, color, rotation, origin - add, scale, effects, 0);

                num++;
                clip.Y += sliceSize;
                clip.Height = Math.Min(sliceSize, clipRect.Y + clipRect.Height - clip.Y);
            }
        }

        static public void SineTextureV(Subtexture subtexture, Vector2 position, Vector2 origin, Vector2 scale, float rotation, Color color, SpriteEffects effects, float sineCounter, float amplitude = 2, int sliceSize = 2, float sliceAdd = MathHelper.TwoPi / 8)
        {
            SineTextureV(subtexture.Texture, subtexture.Rect, position, origin, scale, rotation, color, effects, sineCounter, amplitude, sliceSize, sliceAdd);
        }

        static public void SineTextureH(Texture texture, Rectangle clipRect, Vector2 position, Vector2 origin, Vector2 scale, float rotation, Color color, SpriteEffects effects, float sineCounter, float amplitude = 2, int sliceSize = 2, float sliceAdd = MathHelper.TwoPi / 8)
        {
            position = Calc.Floor(position);
            Rectangle clip = clipRect;
            clip.Width = sliceSize;

            int num = 0;
            while (clip.X < clipRect.X + clipRect.Width)
            {
                Vector2 add = new Vector2(sliceSize * num, (float)Math.Round(Math.Sin(sineCounter + sliceAdd * num) * amplitude));
                Draw.SpriteBatch.Draw(texture.Texture2D, position, clip, color, rotation, origin - add, scale, effects, 0);

                num++;
                clip.X += sliceSize;
                clip.Width = Math.Min(sliceSize, clipRect.X + clipRect.Width - clip.X);
            }
        }

        static public void SineTextureH(Subtexture subtexture, Vector2 position, Vector2 origin, Vector2 scale, float rotation, Color color, SpriteEffects effects, float sineCounter, float amplitude = 2, int sliceSize = 2, float sliceAdd = MathHelper.TwoPi / 8)
        {
            SineTextureH(subtexture.Texture, subtexture.Rect, position, origin, scale, rotation, color, effects, sineCounter, amplitude, sliceSize, sliceAdd);
        }

        static public void TextureFill(Texture texture, Rectangle clipRect, Rectangle fillArea)
        {
            Rectangle currentDraw = Rectangle.Empty;

            for (currentDraw.X = fillArea.X; currentDraw.X < fillArea.X + fillArea.Width; currentDraw.X += clipRect.Width)
            {
                for (currentDraw.Y = fillArea.Y; currentDraw.Y < fillArea.Y + fillArea.Height; currentDraw.Y += clipRect.Height)
                {
                    currentDraw.Width = Math.Min(fillArea.X + fillArea.Width - currentDraw.X, clipRect.Width);
                    currentDraw.Height = Math.Min(fillArea.Y + fillArea.Height - currentDraw.Y, clipRect.Height);
                    SpriteBatch.Draw(texture.Texture2D, currentDraw, clipRect, Color.White);
                }
            }
        }

        //TODO: FIX THIS
        static public void TextureFill(Texture texture, Rectangle clipRect, Rectangle fillArea, int offsetX, int offsetY)
        {
            offsetX %= clipRect.Width;
            offsetY %= clipRect.Height;

            Rectangle currentDraw = Rectangle.Empty;
            Rectangle currentClip = clipRect;
            currentClip.X = clipRect.X + offsetX;
            currentClip.Width -= offsetX;

            for (currentDraw.X = fillArea.X; currentDraw.X < fillArea.X + fillArea.Width; currentDraw.X += clipRect.Width)
            {
                currentClip.Y = clipRect.Y + offsetY;
                currentClip.Height = clipRect.Height - offsetY;

                for (currentDraw.Y = fillArea.Y; currentDraw.Y < fillArea.Y + fillArea.Height; currentDraw.Y += clipRect.Height)
                {
                    currentDraw.Width = Math.Min(fillArea.X + fillArea.Width - currentDraw.X, currentClip.Width);
                    currentDraw.Height = Math.Min(fillArea.Y + fillArea.Height - currentDraw.Y, currentClip.Height);
                    SpriteBatch.Draw(texture.Texture2D, currentDraw, currentClip, Color.White);

                    currentClip.Y = clipRect.Y;
                    currentClip.Height = clipRect.Height;
                }

                currentClip.X = clipRect.X;
                currentClip.Width = clipRect.Width;
            }
        }

        static public void TextureFill(Subtexture subTexture, Rectangle fillArea, int offsetX, int offsetY)
        {
            TextureFill(subTexture.Texture, subTexture.Rect, fillArea, offsetX, offsetY);
        }

        static public void TextureFill(Subtexture subTexture, Rectangle fillArea)
        {
            TextureFill(subTexture.Texture, subTexture.Rect, fillArea);
        }
    }
}
