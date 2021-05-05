using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace OmegaEngine.Example
{
    public class Tetris : Game
    {
        private List<Tetrimino> Pieces;
        private int curPiece;
        private Random ran = new Random();

        private Music background = new Music($"{AppDomain.CurrentDomain.BaseDirectory}\\Assets\\music.wav");
        private SoundEffect move = new SoundEffect($"{AppDomain.CurrentDomain.BaseDirectory}\\Assets\\move.wav");
        private SoundEffect rotate = new SoundEffect($"{AppDomain.CurrentDomain.BaseDirectory}\\Assets\\rotate.wav");
        private SoundEffect clear = new SoundEffect($"{AppDomain.CurrentDomain.BaseDirectory}\\Assets\\clear.wav");
        private SoundEffect fall = new SoundEffect($"{AppDomain.CurrentDomain.BaseDirectory}\\Assets\\fall.wav");
        private SoundEffect level = new SoundEffect($"{AppDomain.CurrentDomain.BaseDirectory}\\Assets\\level.wav");
        private Font gameFont = new Font($"{AppDomain.CurrentDomain.BaseDirectory}\\Assets\\game.ttf", new RGBA(255, 255, 255));

        private Vector2 curLocation;
        private Vector2 holdLocation;
        private Vector2 nextLocation;

        private int hold = -1;

        private bool hasHolded = false;
        private bool paused = false;

        private Board board;

        private int tick;
        private int levelFallTick;
        private int fallTick;
        private const int downFallTick = 3;

        private int linesCleared;

        private int _rotationIndex;
        private int previousRotation;

        private Queue<int> _shuffledPieces = new Queue<int>();
        private Queue<int> ShuffledPieces
        {
            get
            {
                if (_shuffledPieces.Count <= 0)
                    _shuffledPieces = ShuffleAndQueue();
                return _shuffledPieces;
            }
            set
            {
                _shuffledPieces = value;
            }
        }

        private int rotationIndex
        {
            get { return _rotationIndex; }
            set { _rotationIndex = value; if (_rotationIndex > 3) _rotationIndex = 0; else if (_rotationIndex < 0) _rotationIndex = 3; }
        }

        public void Reset()
        {
            tick = 0;
            levelFallTick = 60;
            fallTick = 60;
            linesCleared = 0;
            _rotationIndex = 0;
            previousRotation = 0;
            paused = false;
            hasHolded = false;
            curLocation = new Vector2(3, 0);
            holdLocation = new Vector2(5, 2);
            nextLocation = new Vector2(21, 2);
            curPiece = 0;
            Pieces = Helper.GetPieces();
            board = new Board(10, 20);
            curPiece = ShuffledPieces.Dequeue();
        }

        public Tetris() : base(29 * 25, 20 * 25, 29, 20, "Tetris", 60, true) { }

        public override void BeforeStart()
        {
            background.Loop = true;
            background.Play();
            Reset();
        }

        private Queue<int> ShuffleAndQueue()
        {
            List<int> indexes = new List<int>();

            while (true)
            {
                if (indexes.Count >= 7)
                    break;

                int index = ran.Next(0, Pieces.Count);
                if (indexes.Contains(index))
                    continue;
                else
                    indexes.Add(index);
            }

            Queue<int> result = new Queue<int>();

            foreach (var i in indexes)
                result.Enqueue(i);

            return result;
        }

        public override void Update()
        {
            if (paused)
                return;

            tick++;
            if (tick > fallTick)
            {
                tick = 0;
                var lowest = Vector2.Add(curLocation, Helper.GetLowestPoint(Pieces[curPiece], rotationIndex));
                if (lowest.Y >= 19)
                {
                    fall.Play();
                    PlaceAndSetPiece();
                }
                else if (board.HasBlock(lowest))
                {
                    board = new Board(10, 20);
                }
                else
                {
                    bool place = false;

                    foreach (var i in Pieces[curPiece].Rotations[rotationIndex])
                    {
                        if (board.HasBlock(Vector2.Add(Vector2.Add(i, curLocation), new Vector2(0, 1))))
                        {
                            place = true;
                        }
                    }

                    if (place)
                    {
                        fall.Play();
                        PlaceAndSetPiece();
                    }
                    else
                    {
                        curLocation = Vector2.Add(curLocation, new Vector2(0, 1));
                    }
                }
                UpdateBoard();
            }

            var currentPiece = Pieces[curPiece].Rotations[rotationIndex];
            if (Vector2.Add(currentPiece[0], curLocation).X < 0 || Vector2.Add(currentPiece[3], curLocation).X > 9)
                rotationIndex = previousRotation;

            foreach (var i in currentPiece)
            {
                if (board.HasBlock(Vector2.Add(i, curLocation)))
                {
                    rotationIndex = previousRotation;
                    break;
                }
            }

            currentPiece = Pieces[curPiece].Rotations[rotationIndex];

            for (int i = 0; i < board.PlayingBoard.Length; i++)
            {
                if (!RGBA.IsEqual(board.PlayingBoard[i], new RGBA(0, 0, 0)))
                {
                    DrawAt(new Vector2((i % board.Width) + 10, (i / board.Width)), board.PlayingBoard[i]);
                }
            }

            foreach (var i in currentPiece)
            {
                Vector2 t = Vector2.Add(i, curLocation);
                DrawAt(new Vector2(t.X + 10, t.Y), Pieces[curPiece].Color);
            }

            DrawTextAt(new Vector2(150, 20), 18, gameFont, "HOLD");
            DrawTextAt(new Vector2(550, 20), 18, gameFont, "NEXT");

            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    DrawAt(new Vector2(holdLocation.X + x, holdLocation.Y + y), board.BoardBackground);
                    DrawAt(new Vector2(nextLocation.X + x, nextLocation.Y + y), board.BoardBackground);
                }
            }

            Vector2 offset = new Vector2(-12, 0);
            try
            {
                int peek = ShuffledPieces.Peek();
                foreach (var i in Pieces[peek].Rotations[0])
                {
                    Vector2 t = Vector2.Add(i, nextLocation);
                    if (peek != 1)
                        DrawAt(new Vector2(t.X, t.Y), Pieces[peek].Color, offset);
                    else
                        DrawAt(new Vector2(t.X, t.Y), Pieces[peek].Color);
                }
            }
            catch (Exception) { }

            if (hold >= 0)
            {
                foreach (var i in Pieces[hold].Rotations[0])
                {
                    Vector2 t = Vector2.Add(i, holdLocation);
                    if (hold != 1)
                        DrawAt(new Vector2(t.X, t.Y), Pieces[hold].Color, offset);
                    else
                        DrawAt(new Vector2(t.X, t.Y), Pieces[hold].Color);
                }
            }

            DrawGhost();
        }

        public void DrawGhost()
        {
            return;

            Vector2 ghostPieceLocation = new Vector2(0, 0);
            int gy = 0;
            bool done = false;
            while (!done)
            {
                bool flag = false;

                var i = Helper.GetLowestPoint(Pieces[curPiece], rotationIndex);

                if (board.HasBlock(Vector2.Add(i, new Vector2(0, gy))))
                {
                    Console.WriteLine("Has block");
                    flag = true;
                }
                else if (Vector2.Add(i, new Vector2(0, gy)).Y >= 19)
                {
                    flag = true;
                }

                if (!flag)
                {
                    gy++;
                }
                else
                {
                    ghostPieceLocation = new Vector2(curLocation.X, gy);

                    done = true;
                    break;
                }
            }

            foreach (var i in Pieces[curPiece].Rotations[rotationIndex])
            {
                Vector2 t = Vector2.Add(i, ghostPieceLocation);
                DrawAt(new Vector2(t.X + 10, t.Y), new RGBA(30, 30, 30));
            }
        }

        public void PlaceAndSetPiece()
        {
            foreach (var i in Pieces[curPiece].Rotations[rotationIndex])
            {
                board.Set(Vector2.Add(i, curLocation), Pieces[curPiece].Color);
            }
            rotationIndex = 0;
            previousRotation = 0;
            curPiece = ShuffledPieces.Dequeue();
            curLocation = new Vector2(3, 0);
            hasHolded = false;
        }

        public override void JoystickMove(Joystick.Axis axis, float pos)
        {
            if (paused)
                return;

            Vector2 result;

            if (axis == Joystick.Axis.PovX)
            {
                if (pos == -100)
                {
                    move.Play();
                    result = Vector2.Add(curLocation, new Vector2(-1, 0));

                    var farLeftPos = Vector2.Add(result, Pieces[curPiece].Rotations[rotationIndex][0]);

                    if (farLeftPos.X < 0 ||
                        board.HasBlock(farLeftPos))
                        result = curLocation;
                    curLocation = result;
                }
                else if (pos == 100)
                {
                    move.Play();
                    result = Vector2.Add(curLocation, new Vector2(1, 0));

                    var farRightPos = Vector2.Add(result, Pieces[curPiece].Rotations[rotationIndex][3]);

                    if (farRightPos.X > 9 ||
                        board.HasBlock(farRightPos))
                        result = curLocation;
                    curLocation = result;
                }
            }
            else if (axis == Joystick.Axis.PovY)
            {
                if (pos == -100)
                {
                    fallTick = downFallTick;
                }
                else if (pos == 0)
                {
                    fallTick = levelFallTick;
                }
                else if (pos == 100)
                {
                    fall.Play();
                    while (true)
                    {
                        var lowest = Vector2.Add(curLocation, Helper.GetLowestPoint(Pieces[curPiece], rotationIndex));
                        if (lowest.Y >= 19)
                        {
                            break;
                        }
                        else
                        {
                            bool place = false;

                            foreach (var i in Pieces[curPiece].Rotations[rotationIndex])
                            {
                                if (board.HasBlock(Vector2.Add(Vector2.Add(i, curLocation), new Vector2(0, 1))))
                                {
                                    place = true;
                                }
                            }

                            if (place)
                            {
                                break;
                            }
                            else
                            {
                                curLocation = Vector2.Add(curLocation, new Vector2(0, 1));
                            }
                        }
                        UpdateBoard();
                    }
                    PlaceAndSetPiece();
                    UpdateBoard();
                    tick = 0;
                }
            }
        }

        public override void KeyDown(int keycode, bool isJoystick)
        {
            if (keycode == 36)
                paused = !paused;

            if (paused)
                return;

            Vector2 result;

            switch (keycode)
            {
                //R
                case 17:
                    Reset();
                    break;
                // Z
                case 25:
                case 0:
                    rotate.Play();
                    previousRotation = rotationIndex;
                    rotationIndex--;
                    break;
                // X
                case 23:
                case 1:
                    rotate.Play();
                    previousRotation = rotationIndex;
                    rotationIndex++;
                    break;
                // C
                case 2:
                case 4:
                    if (hasHolded)
                        break;
                    hasHolded = true;
                    rotationIndex = 0;
                    previousRotation = 0;
                    if (hold == -1)
                    {
                        hold = curPiece;
                        curPiece = ShuffledPieces.Dequeue();
                    }
                    else
                    {
                        int temp = hold;
                        hold = curPiece;
                        curPiece = temp;
                    }
                    curLocation = new Vector2(3, 0);
                    break;
                // Left
                case 71:
                    move.Play();
                    result = Vector2.Add(curLocation, new Vector2(-1, 0));

                    var farLeftPos = Vector2.Add(result, Pieces[curPiece].Rotations[rotationIndex][0]);

                    if (farLeftPos.X < 0 ||
                        board.HasBlock(farLeftPos) )
                        result = curLocation;
                    curLocation = result;
                    DrawGhost();
                    break;
                // Right
                case 72:
                    move.Play();
                    result = Vector2.Add(curLocation, new Vector2(1, 0));

                    var farRightPos = Vector2.Add(result, Pieces[curPiece].Rotations[rotationIndex][3]);

                    if (farRightPos.X > 9 ||
                        board.HasBlock(farRightPos))
                        result = curLocation;
                    curLocation = result;
                    DrawGhost();
                    break;
                // Down
                case 74:
                    fallTick = downFallTick;
                    break;
                // Up
                case 73:
                    fall.Play();
                    while (true)
                    {
                        var lowest = Vector2.Add(curLocation, Helper.GetLowestPoint(Pieces[curPiece], rotationIndex));
                        if (lowest.Y >= 19)
                        {
                            break;
                        }
                        else
                        {
                            bool place = false;

                            foreach (var i in Pieces[curPiece].Rotations[rotationIndex])
                            {
                                if (board.HasBlock(Vector2.Add(Vector2.Add(i, curLocation), new Vector2(0, 1))))
                                {
                                    place = true;
                                }
                            }

                            if (place)
                            {
                                break;
                            }
                            else
                            {
                                curLocation = Vector2.Add(curLocation, new Vector2(0, 1));
                            }
                        }
                        UpdateBoard();
                    }
                    PlaceAndSetPiece();
                    UpdateBoard();
                    break;
                // I
                case 8:
                    curPiece++;
                    if (curPiece >= Pieces.Count)
                        curPiece = 0;
                    break;
            }
        }

        public void UpdateBoard()
        {
            int lines = board.Update();

            if (lines > 0)
            {
                clear.Play();

                linesCleared += lines;

                if (linesCleared > 10)
                {
                    level.Play();
                    linesCleared = 0;
                    levelFallTick -= 7;
                    if (levelFallTick <= 3)
                        levelFallTick = 3;
                    if (fallTick != downFallTick)
                    {
                        fallTick = levelFallTick;
                    }
                }
            }
        }

        public override void KeyUp(int keycode, bool isJoystick)
        {
            if (paused)
                return;

            if (keycode == 74)
                fallTick = levelFallTick;
        }
    }
}
