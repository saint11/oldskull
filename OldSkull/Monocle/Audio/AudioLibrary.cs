using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Audio;
using System.Diagnostics;

namespace Monocle
{
    public class AudioLibrary
    {
        public string AudioDirectory { get; private set; }
        public bool Loaded { get; private set; }

        private Dictionary<string, SoundEffect> sounds;

        public AudioLibrary(string audioDirectory, bool load = true)
        {
#if DEBUG
            if (!Directory.Exists(audioDirectory))
                throw new Exception("The directory does not exist!");
#endif

            AudioDirectory = audioDirectory;
            sounds = new Dictionary<string, SoundEffect>();

            if (load)
                Load();
        }

        public SoundEffect this[string index]
        {
            get
            {
                return sounds[index];
            }
        }

        public void Play(string sound, float volume = 1, float pitch = 0, float pan = 0)
        {
            sounds[sound].Play(volume, pitch, pan);
        }

        public void Load()
        {
#if DEBUG
            if (Loaded)
                throw new Exception("Audio Library is already loaded!");
#endif

            Loaded = true;

            string prefix = AudioDirectory + @"\";
            foreach (var file in Directory.EnumerateFiles(AudioDirectory, "*.wav", SearchOption.AllDirectories))
            {
                string name = file.Remove(0, prefix.Length);
                name = name.Remove(name.IndexOf(".wav"));

                FileStream stream = new FileStream(file, FileMode.Open);
                SoundEffect sound = SoundEffect.FromStream(stream);
                stream.Close();

                sounds.Add(name, sound);
            }
        }

        public void Unload()
        {
#if DEBUG
            if (!Loaded)
                throw new Exception("Audio Library is already unloaded!");
#endif

            Loaded = false;

            foreach (var kv in sounds)
                kv.Value.Dispose();
            sounds.Clear();
        }
    }
}
