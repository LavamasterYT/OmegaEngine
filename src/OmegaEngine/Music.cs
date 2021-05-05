using System;
using SF = SFML.Audio;

namespace OmegaEngine
{
    public class Music : IDisposable
    {
        public float Volume
        {
            get
            {
                return music.Volume;
            }
            set
            {
                music.Volume = value;
            }
        }

        public float Pitch
        {
            get
            {
                return music.Pitch;
            }
            set
            {
                music.Pitch = value;
            }
        }

        public bool Loop
        {
            get
            {
                return music.Loop;
            }
            set
            {
                music.Loop = value;
            }
        }

        private SF.Music music;

        public Music(string location)
        {
            music = new SF.Music(location);
        }

        public void Play()
        {
            music.Play();
        }

        public void Pause()
        {
            music.Pause();
        }

        public void Stop()
        {
            music.Stop();
        }

        public void Dispose()
        {
            music.Dispose();
        }

        ~Music()
        {
            music.Dispose();
        }
    }
}
