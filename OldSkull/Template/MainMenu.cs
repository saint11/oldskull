using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monocle;
using OldSkull.Menu;

namespace OldSkull.Template
{
    class MainMenu : Menu.MainMenu
    {
        private Entity title = new Entity(1);
        public override void Begin()
        {
            base.Begin();

            KeyboardInput.Add("accept", Microsoft.Xna.Framework.Input.Keys.Z);
            
            Add(new GenericEntities.Bouncer(new Image(GameDemo.Atlas["logo"]),new Vector4(0,60,Engine.Instance.Screen.Width,Engine.Instance.Screen.Height-60)));


            //Tittle Animation
            Image titleImage = new Image(GameDemo.Atlas["title"]);
            title.Add(titleImage);
            title.X = Engine.Instance.Screen.Width / 2 - titleImage.Width / 2;
            title.Y = -titleImage.Height;
            Add(title);
            Tween.Position(title, new Vector2(title.X, 10), 100, Ease.BackOut, Tween.TweenMode.Oneshot);
            Tween.Alpha(titleImage, 0.1f, 200, null, Tween.TweenMode.YoyoLooping);

            //Layer l = new Layer()
            //l.Effect.
        }
    }
}
