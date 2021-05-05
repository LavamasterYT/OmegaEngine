using System;

namespace OmegaEngine
{
    public struct Font : IDisposable
    {
        public SFML.Graphics.Font LoadedFont;
        public RGBA Color;

        public Font(string location, RGBA col)
        {
            LoadedFont = new SFML.Graphics.Font(location);
            Color = col;
        }

        public void Dispose()
        {
            LoadedFont.Dispose();
        }
    }
}
