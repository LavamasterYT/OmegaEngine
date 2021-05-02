using System;

namespace OmegaEngine
{
    /// <summary>
    /// TODO: Replace this with a simpler color struct
    /// </summary>
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

        public SFML.Graphics.Color ToSFMLColor()
        {
            return new SFML.Graphics.Color()
            {
                R = R,
                G = G,
                B = B,
                A = A
            };
        }

        public static bool IsEqual(RGBA x, RGBA y)
        {
            return x.R == y.R && x.G == y.G && x.B == y.B;
        }
    }
}
