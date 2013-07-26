using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;

namespace Monocle
{
    static public class Audio
    {
        public const float SLOW_RATE = 1 / 2f;
#if DESKTOP
        public const string LOAD_PREFIX = Calc.LOADPATH + @"Content\Sfx\";
#elif OUYA
        public const string LOAD_PREFIX = @"SFX\";   
#endif
        static public List<SFXPlayData> SFXPlayedList = new List<SFXPlayData>();
        static public List<SFX> SFXStoppedList = new List<SFX>();

        static internal List<SFX> pitchList = new List<SFX>();
        static internal List<SFX> loopList = new List<SFX>();

        #region Master SFX Control

        static private float masterPitch = 0f;
        static public float MasterPitch
        {
            get { return masterPitch; }
            set
            {
                value = MathHelper.Clamp(value, -1, 1);
                if (masterPitch != value)
                {
                    masterPitch = value;
                    foreach (var s in pitchList)
                        if (s.ObeysMasterPitch)
                            s.OnPitchChange();
                }
            }
        }

        static public void SetPitchByGameRate(float rate)
        {
            if (rate < 1f)
                Audio.MasterPitch = (rate - SLOW_RATE) / (1 - SLOW_RATE) - 1f;
            else if (rate > 1f)
                Audio.MasterPitch = (rate - 1f) / 1.5f;
            else
                Audio.MasterPitch = 0;
        }

        static private float masterVolume = 1f;
        static public float MasterVolume
        {
            get { return masterVolume; }
            set { masterVolume = MathHelper.Clamp(value, 0, 1); }
        }

        static public int MasterVolumeInt
        {
            get { return (int)Math.Round(masterVolume * 10f); }
            set { MasterVolume = value / 10f; }
        }

        #endregion

        static public void Stop()
        {
            foreach (var s in pitchList)
                s.Stop();
        }

        static public void Pause()
        {
            foreach (var s in pitchList)
                s.Pause();
        }

        static public void Resume()
        {
            foreach (var s in pitchList)
                s.Resume();
        }

        static public void UpdateLoopsToList()
        {
            if (SFXPlayedList != null)
            {
                foreach (var loop in loopList)
                    loop.AddLoopingToList();
            }
        }

        static public void ClearLists()
        {
            SFXPlayedList.Clear();
            SFXStoppedList.Clear();
        }
    }

    public struct SFXPlayData
    {
        public SFX SFX;
        public float X;
        public float Volume;

        public SFXPlayData(SFX sfx, float x, float volume)
        {
            SFX = sfx;
            X = x;
            Volume = volume;
        }

        public void Play()
        {
            SFX.Play(X, Volume);
        }
    }
}
