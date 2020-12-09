using System.Drawing;

namespace OmegaEngine
{
    public class Sprite
    {
        public Vector2 SpriteDimensions { get; set; }
        public RGBA[] SpriteColors { get; set; }

        public Sprite(string path)
        {
            Color col;
            Bitmap image = new Bitmap(path);
            SpriteDimensions = new Vector2(image.Width, image.Height);
            SpriteColors = new RGBA[SpriteDimensions.X * SpriteDimensions.Y];
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    int i = x + SpriteDimensions.X * y;

                    col = image.GetPixel(x, y);
                    //if (col.A == 255)
                        SpriteColors[i] = new RGBA(col.R, col.G, col.B, col.A);
                    //else
                        //SpriteColors[i] = new RGBA(0, 0, 0, 0);
                }
            }

            image.Dispose();
        }

        public Vector2 GetVectorFromIndex(int index)
        {
            return new Vector2(index % SpriteDimensions.X, index / SpriteDimensions.Y);
        }
    }
}