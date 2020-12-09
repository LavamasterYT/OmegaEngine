using System;
using System.Collections.Generic;
using System.Text;

namespace OmegaEngine.Example
{
    public struct Tetrimino
    {
        public RGBA Color { get; set; }

        public Vector2[][] Rotations { get; set; }
    }
}
