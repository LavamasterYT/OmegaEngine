using SFML.Audio;

namespace OmegaEngine
{
    public class SoundEffect
    {
        public float Volume
        {
            get
            {
                return SFX.Volume;
            }
            set
            {
                SFX.Volume = value;
            }
        }

        public float Pitch
        {
            get
            {
                return SFX.Pitch;
            }
            set
            {
                SFX.Pitch = value;
            }
        }

        public bool Loop
        {
            get
            {
                return SFX.Loop;
            }
            set
            {
                SFX.Loop = value;
            }
        }

        public Sound SFX;

        public SoundEffect(string location)
        {
            SFX = new Sound(new SoundBuffer(location));
        }

        public void Play()
        {
            SFX.Play();
        }

        public void Pause()
        {
            SFX.Pause();
        }

        public void Stop()
        {
            SFX.Stop();
        }

        public void Dispose()
        {
            SFX.Dispose();
        }

        ~SoundEffect()
        {
            SFX.Dispose();
        }
    }
}
