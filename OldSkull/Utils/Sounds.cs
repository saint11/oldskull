using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle;
using System.Collections;

namespace OldSkull.Utils
{
    public static class Sounds
    {
        static public bool Loaded;

        static public Dictionary<string,SFX> sounds;

        static public void Load(string[] soundList)
        {
            sounds = new Dictionary<string, SFX>();

            for (int i = 0; i < soundList.Length; i++)
            {
                sounds.Add(soundList[i].ToUpper(),new SFX(soundList[i]));
            }
        }

        static public SFX Play(string soundName,float volume=1)
        {
            SFX sfx = sounds[soundName.ToUpper()];
            sfx.Play(160,volume);
            return sfx;
        }

    }
}
