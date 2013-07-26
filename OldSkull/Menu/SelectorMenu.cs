using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle;
using OldSkull.Menu;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace OldSkull.Menu
{
    public class SelectorMenu : Entity
    {
        private MenuButton[] menuButtons;
        public int selected =0;
        public Effect effect;
        private bool axisDown;
        public string hAlign = "center";
        public string vAlign = "center";
        public bool Kill;
        private Vector2 LastPosition;

        private bool InputBlock=true;

        public SelectorMenu(string[] buttomImages, Action<MenuButton>[] buttomFunctions, Action<int> DefaultFunction = null, Effect effect = null, bool imageButton = true, int layer = 0)
            :base(0)
        {
            if (effect == null) this.effect = SelectorMenuEffects.Scale;
            else this.effect = effect;

            if (buttomImages.Length != buttomFunctions.Length)
                throw new System.ArgumentException("Arrays have different lengths");

            menuButtons = new MenuButton[buttomImages.Length];
            for (int i = 0; i < buttomImages.Length; i++)
			{
                if (imageButton)
                    menuButtons[i] = new MenuButton(this, new Image(OldSkullGame.Atlas[(string)buttomImages[i]]), (Action<MenuButton>)buttomFunctions[i], DefaultFunction, i, layer);
                else
                {
                    Text text = new Text(OldSkullGame.Font, (string)buttomImages[i], Vector2.Zero);

                    menuButtons[i] = new MenuButton(this, text, (Action<MenuButton>)buttomFunctions[i], DefaultFunction, i, layer);
                }
			}
        }

        public override void Added()
        {
            base.Added();

            UpdateVisibility();
            for (int i = 0; i < menuButtons.Count(); i++)
            {
                Scene.Add(menuButtons[i]);
                menuButtons[i].Depth = Depth;
            }

            AlignButtons();
            LastPosition = Position;
            updateButtons();
        }

        private void AlignButtons()
        {
            int nextY = 0;
            for (int i = 0; i < menuButtons.Count(); i++)
            {
                if (hAlign == "left")
                    menuButtons[i].image.Origin.X = 0;
                else
                    menuButtons[i].image.Origin.X = (int)menuButtons[i].image.Origin.X;

                menuButtons[i].X = (int)X;
                menuButtons[i].Y = (int)(Y + menuButtons[i].image.Height + nextY);

                nextY += menuButtons[i].image.Height;
            }
        }

        public override void Update()
        {
            if (Kill)
            {
                RemoveSelf();
                return;
            }
            base.Update();
            UpdateVisibility();

            if ((KeyboardInput.pressedInput("accept") || KeyboardInput.pressedInput("use")) && menuButtons.Length > 0 && !InputBlock)
            {
                menuButtons[selected].press();
            } else {
                InputBlock = false;
            }

            if (Math.Abs(KeyboardInput.yAxis)>0)
            {
                if (!axisDown)
                {
                    selected += (int)KeyboardInput.yAxis;
                    if (selected < 0) selected = menuButtons.Count() - 1;
                    else if (selected >= menuButtons.Count()) selected = 0;

                    updateButtons();
                }
                axisDown = true;
            }
            else axisDown = false;
        }

        private void UpdateVisibility()
        {
            for (int i = 0; i < menuButtons.Count(); i++)
            {
                menuButtons[i].Visible = Visible;
            }
        }
        
        public void updateButtons()
        {
            if (selected >= menuButtons.Count()) selected = menuButtons.Count()-1;
            if (selected < 0) selected = 0;

            for (int i = 0; i < menuButtons.Count(); i++)
            {
                if (i == selected) effect.selectFunction(menuButtons[i].image);
                else effect.deselectFunction(menuButtons[i].image);
            }
        }

        public void setColor(Color value)
        {

            for (int i = 0; i < menuButtons.Count(); i++)
            {
                menuButtons[i].image.Color = value;
            }
        }

        public override void Removed()
        {
            for (int i = 0; i < menuButtons.Count(); i++)
            {
                menuButtons[i].RemoveSelf();
            }
            base.Removed();
        }

        public override void Render()
        {
            if (LastPosition != Position)
            {
                AlignButtons();
                LastPosition = Position;
            }
            base.Render();
        }

        public void CenterHorizontal()
        {
            X = (int)(Engine.Instance.Screen.Width / 2);
        }

        public void CenterVertical()
        {
            Y = (int)(Engine.Instance.Screen.Height / 2);
        }
    }
}
