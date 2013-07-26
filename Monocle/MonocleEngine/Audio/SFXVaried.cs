using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using System.IO;

namespace Monocle
{
    public class SFXVaried : SFX
    {
        public SoundEffect[] Datas { get; private set; }

        private int[] chooseFrom;

        public SFXVaried(string filename, int amount, bool obeysMasterPitch = true)
            : base(obeysMasterPitch)
        {
            Datas = new SoundEffect[amount];

            for (int i = 0; i < amount; i++)
            {
#if DESKTOP
                FileStream stream = new FileStream(Audio.LOAD_PREFIX + filename + GetSuffix(i + 1) + ".wav", FileMode.Open);
                Datas[i] = SoundEffect.FromStream(stream);
                stream.Close();
#elif OUYA
                Datas[i] = Engine.Instance.Content.Load<SoundEffect>(Audio.LOAD_PREFIX + filename + GetSuffix(i + 1) + ".wav");
#endif
            }
        }

        private string GetSuffix(int num)
        {
            return "_" + (num < 10 ? "0" + num.ToString() : num.ToString());
        }

        public override void Play(float panX = 160, float volume = 1)
        {
            if (Audio.MasterVolume <= 0)
                return;
            AddToPlayedList(panX, volume);
            volume *= Audio.MasterVolume;

            int rand;
            if (chooseFrom == null)
            {
                rand = Calc.Random.Next(Datas.Length);
                chooseFrom = new int[Datas.Length - 1];
            }
            else
                rand = Calc.Random.Choose(chooseFrom);

            Datas[rand].Play(volume, ObeysMasterPitch ? Audio.MasterPitch : 0, CalculatePan(panX));

            int num = 0;
            for (int i = 0; i < Datas.Length; i++)
            {
                if (i != rand)
                {
                    chooseFrom[num] = i;
                    num++;
                }
            }

            
        }
    }
}
