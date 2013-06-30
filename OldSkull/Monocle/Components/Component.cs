using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monocle
{
    public class Component
    {
        public Entity Entity { get; internal set; }
        public bool MarkedForRemoval { get; internal set; }
        public bool Active;
        public bool Visible;

        public Component(bool active, bool visible)
        {
            Active = active;
            Visible = visible;
        }

        public virtual void Added()
        {

        }

        public virtual void Removed()
        {

        }

        public virtual void Update()
        {

        }

        public virtual void Render()
        {

        }

        public virtual void HandleGraphicsReset()
        {

        }

        public void RemoveSelf()
        {
            if (Entity != null)
                Entity.Remove(this);
        }

        public Scene Scene
        {
            get { return Entity != null ? Entity.Scene : Engine.Instance.Scene; }
        }
    }
}
