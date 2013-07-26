using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OldSkull.Graphics;
using Microsoft.Xna.Framework;
using OldSkull.GameLevel.Hud;

namespace OldSkull.GameLevel
{
    public class HudElement : PlatformLevelEntity
    {
        private Box CurrentBox;

        private bool Updated;

        public HudElement()
            : base(PlatformerLevel.HUD_LAYER)
        {
            
        }


        public void StartVBox()
        {
            StartBox(Box.BoxStyle.Vertical);
        }

        public void StartHBox()
        {
            StartBox(Box.BoxStyle.Horizontal);
        }

        private void StartBox(Box.BoxStyle BoxStyle)
        {
            Box NewBox = new Box(CurrentBox, BoxStyle);

            Add(NewBox);
            if (CurrentBox != null) CurrentBox.addChild(NewBox);
            CurrentBox = NewBox;

            Updated = true;
        }

        public void Close()
        {
            CurrentBox = CurrentBox.Father;
        }

        public override void Step()
        {
            base.Step();
            if (Updated && Components!=null)
            {
                foreach (Monocle.Component c in Components)
                {
                    if (c is Box)
                    {
                        ((Box)c).AlignChildren();
                    }
                }

                Updated = false;
            }
        }

        internal void AddProgressBar()
        {
            ProgressBar bar = new ProgressBar(50, 20, Color.Red, Color.RoyalBlue);
            CurrentBox.addChild(bar);
            Add(bar);

            Updated = true;
        }
    }
}
