using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Collections;

namespace Monocle
{
    static public class Music
    {
        private const float MIX = .6f;

        static private float masterVolume = 1f;
        static private string currentSongName = "";
        static private Song currentSong;
        static private string nextSongName = "";
        static private Song nextSong;
        static private bool fading = false;
        static private float fadeVolume = 1f;
        static private bool stayFading = false;
        static private int switchTimer = 0;

        static internal void Initialize()
        {
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = MIX;
            MediaPlayer.Stop();
        }

        static internal void Update()
        {
            if (switchTimer > 0)
            {
                switchTimer--;
                if (switchTimer == 0)
                {
                    if (currentSong != null)
                    {
                        MediaPlayer.Stop();
                        currentSong.Dispose();
                    }

                    currentSong = nextSong;
                    currentSongName = nextSongName;
                    nextSong = null;
                    nextSongName = "";

                    fading = false;
                    MediaPlayer.Volume = MIX * masterVolume;
                    MediaPlayer.IsRepeating = true;
                    if (currentSong != null)
                        MediaPlayer.Play(currentSong);
                }
            }
            else if (stayFading)
            {
                fadeVolume = fadeVolume - .025f * Engine.TimeMult;
                MediaPlayer.Volume = MIX * masterVolume * fadeVolume;
            }
            else if (fading)
            {
                fadeVolume = fadeVolume - .025f * Engine.TimeMult;
                MediaPlayer.Volume = MIX * masterVolume * fadeVolume;
                if (fadeVolume <= 0)
                {
                    if (currentSong != null)
                    {
                        MediaPlayer.Stop();
                        currentSong.Dispose();
                    }

                    currentSong = nextSong;
                    currentSongName = nextSongName;
                    nextSong = null;
                    nextSongName = "";

                    fading = false;
                    switchTimer = 0;
                    MediaPlayer.Volume = MIX * masterVolume;
                    MediaPlayer.IsRepeating = true;
                    if (currentSong != null)
                        MediaPlayer.Play(currentSong);
                }
            }
        }

        static public void SetTrack(string name, bool startPlaying = true, bool looping = true)
        {
            if (name == currentSongName || Music.MasterVolume == 0)
                return;

            MediaPlayer.IsRepeating = looping;
            MediaPlayer.Volume = MIX * masterVolume;
            fadeVolume = 1f;
            fading = false;
            stayFading = false;
            switchTimer = 0;

            if (nextSong != null)
            {
                nextSong.Dispose();
                nextSong = null;
                nextSongName = "";
            }

            if (currentSong == null)
            {
                currentSongName = name;
                currentSong = LoadSong(name);

                if (startPlaying)
                    MediaPlayer.Play(currentSong);
            }
            else 
            {
                MediaPlayer.Stop();
                currentSong.Dispose();

                currentSongName = name;

                if (name != "")
                {
                    currentSong = LoadSong(name);
                    if (startPlaying)
                        MediaPlayer.Play(currentSong);
                }
            }
        }

        static public void QueueTrack(string name, int time)
        {
            if (nextSong != null)
            {
                nextSong.Dispose();
                nextSong = null;
                nextSongName = "";
                fading = false;
                stayFading = false;
            }

            switchTimer = time;
            nextSongName = name;
            if (nextSongName == "")
                nextSong = null;
            else
                nextSong = LoadSong(nextSongName);
        }

        static public void StartPlaying()
        {
            if (Music.MasterVolume != 0)
                MediaPlayer.Play(currentSong);
        }

        static public void Stop()
        {
            fading = false;
            switchTimer = 0;
            MediaPlayer.Stop();

            if (currentSong != null)
            {
                currentSong.Dispose();
                currentSong = null;
                currentSongName = "";
            }
        }

        static public void StayFade()
        {
            switchTimer = 0;
            stayFading = true;
        }

        static public void CancelStayFade()
        {
            stayFading = false;
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = MIX * masterVolume;
        }

        static public void FadeOutTo(string nextTrackName)
        {
            if (Music.MasterVolume == 0)
                return;

            if (nextTrackName == currentSongName)
            {
                fadeVolume = 1f;
                MediaPlayer.Volume = MIX * masterVolume;
                if (nextSong != null)
                {
                    nextSong.Dispose();
                    nextSong = null;
                    nextSongName = "";
                    fading = false;
                    stayFading = false;
                    switchTimer = 0;
                }
                return;
            }
            
            if (nextSong != null)
            {
                nextSong.Dispose();
                nextSong = null;
                nextSongName = "";
                fading = false;
                stayFading = false;
                switchTimer = 0;
            }

            fading = true;
            stayFading = false;
            fadeVolume = 1f;
            nextSongName = nextTrackName;
            if (nextTrackName == "")
                nextSong = null;
            else
                nextSong = LoadSong(nextSongName);
            MediaPlayer.IsRepeating = false;
        }

        static private Song LoadSong(string name)
        {
#if DESKTOP
            string path = Path.Combine(Engine.Instance.Content.RootDirectory, @"Music\", name + ".mp3");
            return Song.FromUri(name, new Uri(path, UriKind.Relative));
#else
            return Engine.Instance.Content.Load<Song>(@"Music\" + name);
#endif
        }

        static public float MasterVolume
        {
            get { return masterVolume; }
            set 
            { 
                masterVolume = value;
                MediaPlayer.Volume = MIX * value;
                if (value <= 0)
                {
                    MediaPlayer.Stop();

                    if (currentSong != null)
                        currentSong.Dispose();
                    currentSong = null;
                    currentSongName = "";
                }

            }
        }

        static public int MasterVolumeInt
        {
            get { return (int)Math.Round(masterVolume * 10); }
            set
            {
                MasterVolume = value / 10f;
            }
        }
    }
}
