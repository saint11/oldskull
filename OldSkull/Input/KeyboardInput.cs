using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace OldSkull
{
    using Input = Monocle.Input;

    public abstract class KeyboardInput
    {
        
        public struct KeyAction
        {
            public String name;
            public Keys key;

            public Boolean check;
            public Boolean pressed;
        }

        private static List<KeyAction> keyList = new List<KeyAction>();

        public static Boolean checkInput(String name)
        {
            foreach (KeyAction k in keyList)
            {
                if (k.name == name)
                {
                    return k.check;
                }
            }
            return false;
        }
        public static Boolean pressedInput(String name)
        {
            foreach (KeyAction k in keyList)
            {
                if (k.name == name)
                {
                    return k.pressed;
                }
            }
            return false;
        }
        public static void Add(String name, Keys key)
        {
            //TODO: Check for existing key.
            KeyAction newKey = new KeyAction();
            newKey.name = name;
            newKey.key = key;
            keyList.Add(newKey);
        }



        public static void Update()
        {
            for (int i = 0; i < keyList.Count; i++) // Loop through List with for
            {
                KeyAction k = keyList[i];
                k.check = Input.Check(k.key);
                k.pressed = Input.Pressed(k.key);

                keyList[i] = k;
            }
        }
    }
}
