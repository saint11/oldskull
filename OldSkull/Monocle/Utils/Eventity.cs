using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Monocle
{
    public class Eventity : Entity
    {
        public event Action<Eventity> OnUpdate;
        public event Action<Eventity> OnRender;
        public event Action<Eventity> OnAdded;
        public event Action<Eventity> OnRemoved;
        public event Action<Eventity> OnHandleGraphicsReset;

        public Eventity(Vector2 position, int layerIndex = 0)
            : base(position, layerIndex)
        {

        }

        public Eventity(int layerIndex = 0)
            : base(layerIndex)
        {

        }

        public override void Added()
        {
            base.Added();
            if (OnAdded != null)
                OnAdded(this);
        }

        public override void Removed()
        {
            if (OnRemoved != null)
                OnRemoved(this);
            base.Removed();           
        }

        public override void Update()
        {
            base.Update();
            if (OnUpdate != null)
                OnUpdate(this);
        }

        public override void Render()
        {
            base.Render();
            if (OnRender != null)
                OnRender(this);
        }

        public override void HandleGraphicsReset()
        {
            base.HandleGraphicsReset();
            if (OnHandleGraphicsReset != null)
                OnHandleGraphicsReset(this);
        }
    }
}
