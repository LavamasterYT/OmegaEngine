using System;

namespace OmegaEngine
{
    public struct Font : IDisposable
    {
        public IntPtr hWnd;
        public RGBA Color;

        public Font(string loc, int size, RGBA col)
        {
            Color = col;
            hWnd = SDL_ttf.TTF_OpenFont(loc, size);
        }

        public void Dispose()
        {
            SDL_ttf.TTF_CloseFont(hWnd);
        }
    }
}
