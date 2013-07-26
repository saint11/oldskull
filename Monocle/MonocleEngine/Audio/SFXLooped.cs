using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace Monocle
{
    public class SFXLooped : SFX
    {
        public SoundEffectInstance Instance { get; private set; }
        public int Plays { get; private set; }

        private float setVolume;

        public SFXLooped(string filename, bool obeysMasterPitch = true)
            : base(filename, obeysMasterPitch)
        {
            Instance = Data.CreateInstance();
            Instance.IsLooped = true;

            Audio.loopList.Add(this);
        }

        public override void Play(float panX = 160, float volume = 1)
        {
            if (volume * Audio.MasterVolume > 0)
            {
                setVolume = volume;
                Instance.Volume = volume * Audio.MasterVolume;
                Instance.Pan = CalculatePan(panX);
                if (Instance.State != SoundState.Playing)
                    Instance.Play();
            }
        }

        public override void Stop(bool addToList = true)
        {
            if (Instance.State != SoundState.Stopped)
            {
                Instance.Stop();
                if (addToList)
                    AddToStoppedList();
            }
        }

        public override void Pause()
        {
            if (Instance.State == SoundState.Playing)
                Instance.Pause();
        }

        public override void Resume()
        {
            if (Instance.State == SoundState.Paused)
                Instance.Resume();
        }

        internal override void OnPitchChange()
        {
            Instance.Pitch = Audio.MasterPitch;
        }

        public void SetVolume(float volume)
        {
            setVolume = volume;
            Instance.Volume = volume * Audio.MasterVolume;
        }

        public override void AddLoopingToList()
        {
            if (Instance.State == SoundState.Playing)
                Audio.SFXPlayedList.Add(new SFXPlayData(this, SFX.CalculateX(Instance.Pan), setVolume));
        }
    }
}
