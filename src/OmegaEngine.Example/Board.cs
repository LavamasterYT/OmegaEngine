using System;

namespace OmegaEngine.Example
{
    public class Board
    {
        public RGBA[] PlayingBoard { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public Board(int width, int height)
        {
            PlayingBoard = new RGBA[width * height];
            for (int i = 0; i < PlayingBoard.Length; i++)
            {
                PlayingBoard[i] = new RGBA(0, 0, 0);
            }

            Width = width;
            Height = height;
        }

        public void Set(Vector2 pos, RGBA color)
        {
            Set(pos.X, pos.Y, color);
        }

        public RGBA Get(Vector2 pos)
        {
            return Get(pos.X, pos.Y);
        }

        public void Set(int x, int y, RGBA color)
        {
            try
            {
                PlayingBoard[x + Width * y] = color;
            }
            catch (Exception)
            {

            }
        }

        public RGBA Get(int x, int y)
        {
            try
            {
                return PlayingBoard[x + Width * y];
            }
            catch (Exception)
            {
                return new RGBA(0, 0, 0);
            }
        }

        public bool HasBlock(Vector2 pos)
        {
            return HasBlock(pos.X, pos.Y);
        }

        public bool HasBlock(int x, int y)
        {
            try
            {
                return !RGBA.IsEqual(PlayingBoard[x + Width * y], new RGBA(0, 0, 0));
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void RemoveRow(int y)
        {
            for (int x = 0; x < Width; x++)
            {
                Set(x, y, new RGBA(0, 0, 0));
            }
        }

        private void MoveRowDown(int y)
        {
            for (int x = 0; x < Width; x++)
            {
                Set(new Vector2(x, y + 1), Get(x, y));
                Set(new Vector2(x, y), new RGBA(0, 0, 0));
            }
        }

        public bool Update()
        {
            bool cleared = false;
            bool flag = true;

            do
            {
                bool filled = true;
                flag = true;

                for (int y = Height - 1; y >= 0; y--)
                {
                    filled = true;
                    for (int x = 0; x < Width; x++)
                    {
                        if (!HasBlock(x, y))
                        {
                            filled = false;
                            break;
                        }
                    }

                    if (filled)
                    {
                        cleared = true;
                        RemoveRow(y);
                        for (int ay = y - 1; ay >= 0; ay--)
                        {
                            MoveRowDown(ay);
                        }
                        y = y - 1;
                        flag = false;
                    }
                }
            }
            while (!flag);

            return cleared;
        }
    }
}
