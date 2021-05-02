using System;

namespace OmegaEngine
{
    /// <summary>
    /// This struct has not been tested, so expect some issues
    /// </summary>
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
