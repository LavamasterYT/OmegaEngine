using System;
using OmegaEngine;

namespace OmegaEngine.Example
{
    class Program
    {
        static void Main()
        {
            using (Game game = new Tetris())
                game.Start();
        }
    }
}
