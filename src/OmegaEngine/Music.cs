namespace OmegaEngine
{
    public struct Music
    {
        public SFML.Audio.Music music;

        public Music(string location, bool looping)
        {
            music = new SFML.Audio.Music(location)
            {
                Loop = looping
            };
        }
    }
}
