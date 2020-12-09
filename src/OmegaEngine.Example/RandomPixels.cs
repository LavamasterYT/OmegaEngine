using System;

namespace OmegaEngine.Example
{
    public class RandomPixels : Game
    {
        Random rng = new Random();

        public RandomPixels() : base(1280, 720, 1280, 720, 60, true) { }

        public override void Draw()
        {
            RGBA color;
            for (int y = 0; y < 720; y++)
                for (int x = 0; x < 1280; x++)
                {
                    color = new RGBA((byte)rng.Next(0, 256), (byte)rng.Next(0, 256), (byte)rng.Next(0, 256));
                    DrawAt(new Vector2(x, y), color);
                }

            base.Draw();
        }

        public override bool Update()
        {
            return false;
        }
    }
}
