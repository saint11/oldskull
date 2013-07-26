using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle;

namespace OldSkull.GameLevel.Hud
{
    class Box : GraphicsComponent
    {
        public Box Father { get; private set; }
        private int _Width = 0;
        private int _Height = 0;

        private List<GraphicsComponent> Children;

        public enum BoxStyle { Horizontal, Vertical };
        private BoxStyle Style;

        public Box(Box Father, BoxStyle Style)
            :base(false)
        {
            this.Father = Father;
            if (Father == null)
            {
                _Width = Engine.Instance.Screen.Width;
                _Height = Engine.Instance.Screen.Height;
            }

            this.Style = Style;
            Children = new List<GraphicsComponent>();
        }

        internal void addChild(GraphicsComponent Child)
        {
            Children.Add(Child);
            Child.X = X;
            Child.Y = Y;
        }

        internal void AlignChildren()
        {
            //float TotalSize=0;
            float TotalArea= Style == BoxStyle.Horizontal ? Width : Height;

            /*foreach (GraphicsComponent child in Children)
            {
                TotalSize += Style == BoxStyle.Horizontal ? child.Width : child.Height;
            }*/
            
            for (int i = Children.Count - 1; i >= 0; i--)
            {
                GraphicsComponent child = Children[i];
                if (Style == BoxStyle.Horizontal)
                {
                    if (Children.Count>1)
                        child.X = X + (i * ((TotalArea - child.Width) / (Children.Count - 1)) + child.Width / 2);
                    else
                        child.X = X + (TotalArea/2);

                    child.Origin.X = child.Width / 2;
                }
                else
                {
                    if (Children.Count > 1)
                        child.Y = Y + (i * ((TotalArea - child.Height) / (Children.Count - 1)) + child.Height / 2);
                     else
                        child.Y = Y + (TotalArea/2);

                    child.Origin.Y = child.Height / 2;
                }

                if (child is Box)
                {
                    Box b = (Box)child;
                    b._Width = Style == BoxStyle.Horizontal ? (int)(TotalArea/(Children.Count)) : Width;
                    b._Height = Style == BoxStyle.Horizontal ? Height : (int)(TotalArea / (Children.Count));
                }
            }
            
        }



        public override int Width
        {
            get { return _Width; }
        }

        public override int Height
        {
            get { return _Height; }
        }

    }
}

