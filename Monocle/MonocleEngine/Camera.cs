﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monocle
{
    public class Camera
    {
        private Matrix matrix = Matrix.Identity;
        private Matrix inverse = Matrix.Identity;
        private bool changed;

        private Vector2 position = Vector2.Zero;
        private Vector2 zoom = Vector2.One;
        private Vector2 origin = Vector2.Zero;
        private float angle = 0;

        public Viewport Viewport;

        public Camera()
        {
            Viewport = new Viewport();
            Viewport.Width = Engine.Instance.Screen.Width;
            Viewport.Height = Engine.Instance.Screen.Height;
            UpdateMatrices();
        }

        public override string ToString()
        {
            return "Camera:\n\tViewport: { " + Viewport.X + ", " + Viewport.Y + ", " + Viewport.Width + ", " + Viewport.Height + 
                " }\n\tPosition: { " + position.X + ", " + position.Y + 
                " }\n\tOrigin: { " + origin.X + ", " + origin.Y +
                " }\n\tZoom: { " + zoom.X + ", " + zoom.Y + 
                " }\n\tAngle: " + angle;
        }

        private void UpdateMatrices()
        {
            matrix = Matrix.Identity *
                    Matrix.CreateTranslation(new Vector3(-new Vector2((int)position.X, (int)position.Y), 0)) *
                    Matrix.CreateRotationZ(angle) *
                    Matrix.CreateScale(new Vector3(zoom, 1)) *
                    Matrix.CreateTranslation(new Vector3(new Vector2((int)origin.X, (int)origin.Y), 0));

            inverse = Matrix.Invert(matrix);

            changed = false;
        }

        public Matrix Matrix
        {
            get
            {
                if (changed)
                    UpdateMatrices();
                return matrix;
            }
        }

        public Matrix Inverse
        {
            get
            {
                if (changed)
                    UpdateMatrices();
                return inverse;
            }
        }

        public Vector2 Position
        {
            get { return position; }
            set
            {
                changed = true;
                position = value;
            }
        }

        public Vector2 Origin
        {
            get { return origin; }
            set
            {
                changed = true;
                origin = value;
            }
        }

        public float X
        {
            get { return position.X; }
            set
            {
                changed = true;
                position.X = value;
            }
        }

        public float Y
        {
            get { return position.Y; }
            set
            {
                changed = true;
                position.Y = value;
            }
        }

        public float Zoom
        {
            get { return zoom.X; }
            set
            {
                changed = true;
                zoom.X = zoom.Y = value;
            }
        }

        public float Angle
        {
            get { return angle; }
            set
            {
                changed = true;
                angle = value;
            }
        }

        /*
         *  Utils
         */

        public void CenterOrigin()
        {
            origin = new Vector2((float)Viewport.Width / 2, (float)Viewport.Height / 2);
            changed = true;
        }

        public void RoundPosition()
        {
            position.X = (float)Math.Round(position.X);
            position.Y = (float)Math.Round(position.Y);
            changed = true;
        }

        public Vector2 GlobalToCamera(Vector2 position)
        {
            return Vector2.Transform(position, Inverse);
        }

        public Vector2 CameraToGlobal(Vector2 position)
        {
            return Vector2.Transform(position, Matrix);
        }

        public void Approach(Vector2 position, float ease)
        {
            Position += (position - Position) * ease;
        }

        public void Approach(Vector2 position, float ease, float maxDistance)
        {
            Vector2 move = (position - Position) * ease;
            if (move.Length() > maxDistance)
                Position += Vector2.Normalize(move) * maxDistance;
            else
                Position += move;
        }
    }
}