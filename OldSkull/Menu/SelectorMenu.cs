using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle;
using OldSkull.Menu;

namespace OldSkull.Menu
{
    public class SelectorMenu : Entity
    {
        private MenuButton[] menuButtons;
        private int selected =0;
        public Effect effect;
        private bool axisDown;

        public SelectorMenu(string[] buttomImages, Action[] buttomFunctions, Effect effect = null)
            :base(0)
        {
            if (effect == null) this.effect = SelectorMenuEffects.Scale;
            else this.effect = effect;

            if (buttomImages.Length != buttomFunctions.Length)
                throw new System.ArgumentException("Arrays have different lengths");

            menuButtons = new MenuButton[buttomImages.Length];
            for (int i = 0; i < buttomImages.Length; i++)
			{
                menuButtons[i] = new MenuButton((string)buttomImages[i], (Action)buttomFunctions[i]);
			}
        }

        public override void Added()
        {
            base.Added();
            for (int i = 0; i < menuButtons.Count(); i++)
            {
                Scene.Add(menuButtons[i]);
                menuButtons[i].X = Engine.Instance.Screen.Width / 2 + X;
                menuButtons[i].Y = Engine.Instance.Screen.Height / 2 + i * menuButtons[i].image.Height + Y;
            }
            updateButtons();
        }

        public override void Update()
        {
            base.Update();
            if (KeyboardInput.pressedInput("accept"))
            {
                menuButtons[selected].press();
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

        private void updateButtons()
        {
            for (int i = 0; i < menuButtons.Count(); i++)
            {
                if (i == selected) effect.selectFunction(menuButtons[i].image);
                else effect.deselectFunction(menuButtons[i].image);
            }
        }
    }
}
