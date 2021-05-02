using System.Collections.Generic;

namespace OmegaEngine.Example
{
    public class Helper
    {
        public static Vector2 AlignToLeft(Vector2 input)
        {
            return Vector2.Add(input, new Vector2(input.X - input.X, input.Y));
        }

        public static Vector2 GetLowestPoint(Tetrimino tetrimino, int rotation)
        {
            Vector2 lowest = new Vector2(0, 0);
            foreach (var i in tetrimino.Rotations[rotation])
            {
                if (lowest.Y < i.Y)
                {
                    lowest = i;
                }
            }

            return lowest;
        }

        public static List<Tetrimino> GetPieces()
        {
            return new List<Tetrimino>
            {
                new Tetrimino
                {
                    Color = new RGBA(3, 215, 255),
                    Rotations = new Vector2[][]
                    {
                        new Vector2[] { new Vector2(2, 0), new Vector2(2, 1), new Vector2(2, 2), new Vector2(2, 3) },
                        new Vector2[] { new Vector2(0, 2), new Vector2(1, 2), new Vector2(2, 2), new Vector2(3, 2) },
                        new Vector2[] { new Vector2(1, 0), new Vector2(1, 1), new Vector2(1, 2), new Vector2(1, 3) },
                        new Vector2[] { new Vector2(0, 1), new Vector2(1, 1), new Vector2(2, 1), new Vector2(3, 1) }
                    }
                },
                new Tetrimino
                {
                    Color = new RGBA(255, 235, 54),
                    Rotations = new Vector2[][]
                    {
                        new Vector2[] { new Vector2(1, 1), new Vector2(2, 1), new Vector2(1, 2), new Vector2(2, 2) },
                        new Vector2[] { new Vector2(1, 1), new Vector2(2, 1), new Vector2(1, 2), new Vector2(2, 2) },
                        new Vector2[] { new Vector2(1, 1), new Vector2(2, 1), new Vector2(1, 2), new Vector2(2, 2) },
                        new Vector2[] { new Vector2(1, 1), new Vector2(2, 1), new Vector2(1, 2), new Vector2(2, 2) }
                    }
                },
                new Tetrimino
                {
                    Color = new RGBA(111, 255, 54),
                    Rotations = new Vector2[][]
                    {
                        new Vector2[] { new Vector2(1, 2), new Vector2(2, 1), new Vector2(2, 2), new Vector2(3, 1) },
                        new Vector2[] { new Vector2(2, 1), new Vector2(2, 2), new Vector2(3, 2), new Vector2(3, 3) },
                        new Vector2[] { new Vector2(1, 3), new Vector2(2, 2), new Vector2(2, 3), new Vector2(3, 2) },
                        new Vector2[] { new Vector2(1, 1), new Vector2(1, 2), new Vector2(2, 2), new Vector2(2, 3) }
                    }
                },
                new Tetrimino
                {
                    Color = new RGBA(105, 64, 255),
                    Rotations = new Vector2[][]
                    {
                        new Vector2[] { new Vector2(1, 2), new Vector2(2, 1), new Vector2(2, 2), new Vector2(3, 2) },
                        new Vector2[] { new Vector2(1, 2), new Vector2(2, 1), new Vector2(2, 2), new Vector2(2, 3) },
                        new Vector2[] { new Vector2(1, 2), new Vector2(2, 3), new Vector2(2, 2), new Vector2(3, 2) },
                        new Vector2[] { new Vector2(2, 1), new Vector2(2, 2), new Vector2(2, 3), new Vector2(3, 2) }
                    }
                },
                new Tetrimino
                {
                    Color = new RGBA(237, 31, 31),
                    Rotations = new Vector2[][]
                    {
                        new Vector2[] { new Vector2(1, 1), new Vector2(2, 1), new Vector2(2, 2), new Vector2(3, 2) },
                        new Vector2[] { new Vector2(2, 2), new Vector2(2, 3), new Vector2(3, 1), new Vector2(3, 2) },
                        new Vector2[] { new Vector2(1, 2), new Vector2(2, 2), new Vector2(2, 3), new Vector2(3, 3) },
                        new Vector2[] { new Vector2(1, 2), new Vector2(1, 3), new Vector2(2, 2), new Vector2(2, 1) }
                    }
                },
                new Tetrimino
                {
                    Color = new RGBA(237, 151, 31),
                    Rotations = new Vector2[][]
                    {
                        new Vector2[] { new Vector2(1, 2), new Vector2(2, 2), new Vector2(3, 1), new Vector2(3, 2) },
                        new Vector2[] { new Vector2(2, 1), new Vector2(2, 2), new Vector2(2, 3), new Vector2(3, 3) },
                        new Vector2[] { new Vector2(1, 2), new Vector2(1, 3), new Vector2(2, 2), new Vector2(3, 2) },
                        new Vector2[] { new Vector2(1, 1), new Vector2(2, 1), new Vector2(2, 2), new Vector2(2, 3) }
                    }
                },
                new Tetrimino
                {
                    Color = new RGBA(31, 55, 237),
                    Rotations = new Vector2[][]
                    {
                        new Vector2[] { new Vector2(1, 1), new Vector2(1, 2), new Vector2(2, 2), new Vector2(3, 2) },
                        new Vector2[] { new Vector2(2, 1), new Vector2(2, 2), new Vector2(2, 3), new Vector2(3, 1) },
                        new Vector2[] { new Vector2(1, 2), new Vector2(2, 2), new Vector2(3, 2), new Vector2(3, 3) },
                        new Vector2[] { new Vector2(1, 3), new Vector2(2, 1), new Vector2(2, 2), new Vector2(2, 3) }
                    }
                }
            };
        }
    }
}
