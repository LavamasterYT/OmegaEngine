using System;
using OmegaEngine;

namespace OmegaEngine.Example
{
    class Program
    {
        static void Main()
        {
            using (RandomPixels game = new RandomPixels())
                game.Start();
            using (Frogger game = new Frogger())
                game.Start();
        }
    }
}
