using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monocle;
using OldSkull.Menu;
using OldSkull.GameLevel;

namespace OldSkull.Isle
{
    class MainMenu : Menu.MainMenu
    {
        private Entity title = new Entity(1);
        public override void Begin()
        {
            base.Begin();

            //Tittle Animation
            Image titleImage = new Image(OldSkullGame.Atlas["title"]);
            title.Add(titleImage);
            title.X = Engine.Instance.Screen.Width / 2 - titleImage.Width / 2;
            title.Y = -titleImage.Height;
            Add(title);
            Tween.Position(title, new Vector2(title.X, 10), 100, Ease.BackOut, Tween.TweenMode.Oneshot);

            Engine.Instance.Screen.ClearColor = Color.Black;

            SelectorMenu menu = new SelectorMenu(new string[] { "NEW", "CREDITS", "EXIT" }, new Action<MenuButton>[] { newGame, null, exitGame }, null, SelectorMenuEffects.ColorSwap(Color.Bisque,Color.Chocolate), false);
            menu.CenterHorizontal();
            menu.CenterVertical();
            Add(menu);
            Add(new GenericEntities.Bouncer(new Image(OldSkullGame.Atlas["logo"]), new Vector4(0, 60, Engine.Instance.Screen.Width, Engine.Instance.Screen.Height - 60)));

        }

        public void newGame(MenuButton opt)
        {
            OldSkullGame.Instance.Scene = PlatformerLevel.autoLoad("1");
        }

        public void exitGame(MenuButton opt)
        {
            Engine.Instance.Exit();
        }
    }
}
