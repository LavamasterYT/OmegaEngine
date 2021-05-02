using System;
using System.Collections.Generic;

namespace OmegaEngine.Example
{
    public class Tetris : Game
    {
        private List<Tetrimino> Pieces;
        private int curPiece = 0;
        private Random ran = new Random();

        private Music background = new Music($"{AppDomain.CurrentDomain.BaseDirectory}\\Assets\\music.wav", true);
        private SoundEffect move = new SoundEffect($"{AppDomain.CurrentDomain.BaseDirectory}\\Assets\\move.wav");
        private SoundEffect rotate = new SoundEffect($"{AppDomain.CurrentDomain.BaseDirectory}\\Assets\\rotate.wav");
        private SoundEffect clear = new SoundEffect($"{AppDomain.CurrentDomain.BaseDirectory}\\Assets\\clear.wav");
        private SoundEffect fall = new SoundEffect($"{AppDomain.CurrentDomain.BaseDirectory}\\Assets\\fall.wav");

        private Vector2 curLocation = new Vector2(3, 0);

        private Board board;

        private int fallTick = 0;
        private int fallMaxTick = 15;

        private int _rotationIndex = 0;
        private int previousRotation = 0;

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

        public Tetris() : base(10 * 25, 20 * 25, 10, 20, "Tetris", 60, true) { }

        public override void BeforeStart()
        {
            Pieces = Helper.GetPieces();

            board = new Board(10, 20);

            curPiece = ShuffledPieces.Dequeue();

            PlayMusic(background);
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
            fallTick++;
            if (fallTick > fallMaxTick)
            {
                fallTick = 0;
                var lowest = Vector2.Add(curLocation, Helper.GetLowestPoint(Pieces[curPiece], rotationIndex));
                if (lowest.Y >= 19)
                {
                    PlaySFX(fall);
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
                        PlaySFX(fall);
                        PlaceAndSetPiece();
                    }
                    else
                    {
                        curLocation = Vector2.Add(curLocation, new Vector2(0, 1));
                    }
                }
                if (board.Update())
                    PlaySFX(clear);
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

            foreach (var i in currentPiece)
            {
                DrawAt(Vector2.Add(i, curLocation), Pieces[curPiece].Color);
            }

            for (int i = 0; i < board.PlayingBoard.Length; i++)
            {
                if (!RGBA.IsEqual(board.PlayingBoard[i], new RGBA(0, 0, 0)))
                {
                    DrawAt(new Vector2(i % board.Width, i / board.Width), board.PlayingBoard[i]);
                }
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
        }

        public override void KeyDown(int keycode, bool isJoystick)
        {
            Vector2 result;

            switch (keycode)
            {
                case 25:
                    PlaySFX(rotate);
                    previousRotation = rotationIndex;
                    rotationIndex--;
                    break;
                case 23:
                    PlaySFX(rotate);
                    previousRotation = rotationIndex;
                    rotationIndex++;
                    break;
                case 71:
                    PlaySFX(move);
                    result = Vector2.Add(curLocation, new Vector2(-1, 0));

                    var farLeftPos = Vector2.Add(result, Pieces[curPiece].Rotations[rotationIndex][0]);

                    if (farLeftPos.X < 0 ||
                        board.HasBlock(farLeftPos) )
                        result = curLocation;
                    curLocation = result;
                    break;
                case 72:
                    PlaySFX(move);
                    result = Vector2.Add(curLocation, new Vector2(1, 0));

                    var farRightPos = Vector2.Add(result, Pieces[curPiece].Rotations[rotationIndex][3]);

                    if (farRightPos.X > 9 ||
                        board.HasBlock(farRightPos))
                        result = curLocation;
                    curLocation = result;
                    break;
                case 74:
                    fallMaxTick = 3;
                    break;
                case 73:
                    PlaySFX(fall);
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
                        if (board.Update())
                            PlaySFX(clear);
                    }
                    PlaceAndSetPiece();
                    if (board.Update())
                        PlaySFX(clear);
                    break;
                case 8:
                    curPiece++;
                    if (curPiece >= Pieces.Count)
                        curPiece = 0;
                    break;
            }
        }

        public override void KeyUp(int keycode, bool isJoystick)
        {
            if (keycode == 74)
                fallMaxTick = 60;
        }
    }
}
