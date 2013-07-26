using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using System.IO;
using Microsoft.Xna.Framework;

namespace Monocle
{
    public class SFX
    {
        public SoundEffect Data { get; private set; }
        public bool ObeysMasterPitch { get; private set; }

        public SFX(string filename, bool obeysMasterPitch = true)
            : this(obeysMasterPitch)
        {
#if DESKTOP
            FileStream stream = new FileStream(Audio.LOAD_PREFIX + filename + ".wav", FileMode.Open);
            Data = SoundEffect.FromStream(stream);
            stream.Close();
#elif OUYA
            Data = Engine.Instance.Content.Load<SoundEffect>(Audio.LOAD_PREFIX + filename + ".wav");      
#endif
        }

        internal SFX(bool obeysMasterPitch)
        {
            ObeysMasterPitch = obeysMasterPitch;
            Audio.pitchList.Add(this);
        }

        public virtual void Play(float panX = 160, float volume = 1)
        {    
            if (Audio.MasterVolume > 0)
            {
                AddToPlayedList(panX, volume);
                volume *= Audio.MasterVolume;
                Data.Play(volume, ObeysMasterPitch ? Audio.MasterPitch : 0, CalculatePan(panX));
            }
        }

        public virtual void Stop(bool addToList = true)
        {
            
        }

        public virtual void Pause()
        {
          
        }

        public virtual void Resume()
        {

        }

        internal virtual void OnPitchChange()
        {

        }

        public virtual void AddLoopingToList()
        {

        }

        static public float CalculatePan(float panX)
        {
            return MathHelper.Lerp(-.5f, .5f, panX / 320f);
        }

        static public float CalculateX(float pan)
        {
            return MathHelper.Lerp(0, 320, pan + .5f);
        }

        protected void AddToPlayedList(float panX, float volume)
        {
            if (Audio.SFXPlayedList != null)
                Audio.SFXPlayedList.Add(new SFXPlayData(this, panX, volume));
        }

        protected void AddToStoppedList()
        {
            if (Audio.SFXStoppedList != null)
                Audio.SFXStoppedList.Add(this);
        }
    }
}
