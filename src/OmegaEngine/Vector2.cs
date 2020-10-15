using System;

namespace OmegaEngine
{
    public struct Vector2
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Vector2(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Vector2 Add(Vector2 source, Vector2 input)
        {
            return new Vector2(source.X + input.X, source.Y + input.Y);
        }

        public static bool Matches(Vector2 x, Vector2 y)
        {
            return x.X == y.X && x.Y == y.Y;
        }

        public static Vector2 GenerateRandomVector(Random rngInstance, int max) => GenerateRandomVector(rngInstance, 0, max, 0, max);
        public static Vector2 GenerateRandomVector(Random rngInstance, int maxX, int maxY) => GenerateRandomVector(rngInstance, 0, maxX, 0, maxY);
        public static Vector2 GenerateRandomVector(Random rngInstance, int minX, int maxX, int minY, int maxY)
        {
            return new Vector2(rngInstance.Next(minX, maxX), rngInstance.Next(minY, maxY));
        }
    }
}
