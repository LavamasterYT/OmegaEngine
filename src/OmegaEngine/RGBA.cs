using System;

namespace OmegaEngine
{
    public struct RGBA
    {
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
        public byte A { get; set; }

        public RGBA(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
            A = 255;
        }

        public RGBA(byte r, byte g, byte b, byte a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public SDL.SDL_Color ToSDL_Color()
        {
            return new SDL.SDL_Color()
            {
                r = R,
                g = G,
                b = B,
                a = A
            };
        }

        public static bool IsEqual(RGBA x, RGBA y)
        {
            return x.R == y.R && x.G == y.G && x.B == y.B;
        }
    }
}
