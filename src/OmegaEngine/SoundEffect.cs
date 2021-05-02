using System;
using System.Collections.Generic;
using System.Text;

namespace OmegaEngine
{
    public struct SoundEffect
    {
        public SFML.Audio.Sound SFX;

        public SoundEffect(string location)
        {
            SFX = new SFML.Audio.Sound(new SFML.Audio.SoundBuffer(location));
        }
    }
}
