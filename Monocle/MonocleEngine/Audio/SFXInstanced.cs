using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace Monocle
{
    public class SFXInstanced : SFX
    {
        public SoundEffectInstance[] Instances { get; private set; }

        public SFXInstanced(string filename, int instances = 2, bool obeysMasterPitch = true)
            : base(filename, obeysMasterPitch)
        {
            Instances = new SoundEffectInstance[instances];
            for (int i = 0; i < instances; i++)
                Instances[i] = Data.CreateInstance();
        }

        public override void Play(float panX = 160, float volume = 1)
        {
            if (Audio.MasterVolume <= 0)
                return;
            AddToPlayedList(panX, volume);
            volume *= Audio.MasterVolume;

            SoundEffectInstance toPlay = null;
            foreach (var i in Instances)
            {
                toPlay = i;
                if (i.State == SoundState.Stopped)
                    break;
            }

            toPlay.Pan = CalculatePan(panX);
            toPlay.Volume = volume;
            if (toPlay.State != SoundState.Stopped)
                toPlay.Stop();
            toPlay.Play();
        }

        public override void Stop(bool addToList = true)
        {
            bool stopped = false;
            foreach (var i in Instances)
            {
#if DESKTOP
                if (i.State != SoundState.Stopped)
                {
                    stopped = true;
                    i.Stop();
                }
#elif OUYA
                stopped = true;
                i.Stop();
#endif
            }

            if (addToList && stopped)
                AddToStoppedList();
        }

        public override void Pause()
        {
            foreach (var i in Instances)
#if DESKTOP
                if (i.State == SoundState.Playing)
                    i.Pause();
#else
                i.Stop(false);
#endif
        }

        public override void Resume()
        {
#if DESKTOP
            foreach (var i in Instances)
                if (i.State == SoundState.Paused)
                    i.Resume();
#endif
        }

        internal override void OnPitchChange()
        {
            foreach (var i in Instances)
                i.Pitch = Audio.MasterPitch;
        }
    }
}
