using System;
using System.Diagnostics;
using SFML.Audio;
using SFML.Graphics;
using SFML.Window;
using static SFML.Window.Joystick;

namespace OmegaEngine
{
    public abstract class Game : IDisposable
    {
        private uint framerate;
        private int pixelWidth;
        private int pixelHeight;
        private bool clearOnEachFrame;
        private RenderWindow window;
        RectangleShape pixel;

        #region private string WindowTitle;
        private string _windowTitle;
        public string WindowTitle
        {
            get { return _windowTitle; }
            set { if (window != null) window.SetTitle(value); _windowTitle = value; }
        }
        #endregion
        public readonly int WindowWidth;
        public readonly int WindowHeight;
        public readonly int GridWidth;
        public readonly int GridHeight;

        public RGBA BackgroundColor;

        public virtual void Update() { }
        public virtual void KeyDown(int keycode, bool isJoystick) { }
        public virtual void KeyUp(int keycode, bool isJoystick) { }
        public virtual void JoystickMove(Axis axis, float pos) { }
        public virtual void OnQuit() { }
        public virtual void BeforeStart() { }

        public Game(int width, int height, int gW, int gH, string title, uint framerate = uint.MaxValue, bool clear = true)
        {
            WindowTitle = title;

            if (width % gW != 0 || height % gH != 0)
            {
                Console.WriteLine("The window width/height is not compatible with the grid width/height. You may experience issues with some pixels being out of bounds.");
            }

            WindowWidth = width;
            WindowHeight = height;

            pixelHeight = height / gH;
            pixelWidth = width / gW;

            clearOnEachFrame = clear;

            BackgroundColor = new RGBA(0, 0, 0);

            GridWidth = gW;
            GridHeight = gH;

            this.framerate = framerate;

            pixel = new RectangleShape(new SFML.System.Vector2f(pixelWidth, pixelHeight));
        }

        public void PlayMusic(Music music)
        {
            music.music.Play();
        }

        public void PlaySFX(SoundEffect sound)
        {
            sound.SFX.Play();
        }

        public void Start()
        {
            window = new RenderWindow(new VideoMode((uint)WindowWidth, (uint)WindowHeight), WindowTitle, Styles.Close);

            window.KeyPressed += Window_KeyPressed;
            window.KeyReleased += Window_KeyReleased;
            window.JoystickButtonPressed += Window_JoystickButtonPressed;
            window.JoystickButtonReleased += Window_JoystickButtonReleased;
            window.JoystickMoved += Window_JoystickMoved;
            window.Closed += Window_Closed;

            if (framerate != uint.MaxValue)
                window.SetFramerateLimit(framerate);

            window.Clear(BackgroundColor.ToSFMLColor());
            BeforeStart();
            while (window.IsOpen)
            {
                window.DispatchEvents();

                if (clearOnEachFrame)
                    window.Clear(BackgroundColor.ToSFMLColor());

                Update();

                window.Display();
            }
        }

        #region Events
        private void Window_JoystickMoved(object sender, JoystickMoveEventArgs e)
        {
            JoystickMove(e.Axis, e.Position);
        }

        private void Window_JoystickButtonReleased(object sender, JoystickButtonEventArgs e)
        {
            KeyUp((int)e.Button, true);
        }

        private void Window_JoystickButtonPressed(object sender, JoystickButtonEventArgs e)
        {
            KeyDown((int)e.Button, true);
        }

        private void Window_KeyReleased(object sender, KeyEventArgs e)
        {
            KeyUp((int)e.Code, false);
        }

        private void Window_KeyPressed(object sender, KeyEventArgs e)
        {
            KeyDown((int)e.Code, false);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Quit();
        }
        #endregion

        public void DrawTextAt(Vector2 pos, uint fontSize, Font font, string text)
        {
            Text fontText = new Text(text, font.LoadedFont, fontSize);
            fontText.Position = new SFML.System.Vector2f(pos.X, pos.Y);
            window.Draw(fontText);
            fontText.Dispose();
        }

        public void DrawAt(Vector2 pos, RGBA color)
        {
            pixel.Position = new SFML.System.Vector2f(pos.X * pixelWidth, pos.Y * pixelHeight);
            pixel.FillColor = color.ToSFMLColor();
            window.Draw(pixel);
        }

        public void DrawSprite(Sprite sprite, Vector2 pos)
        {
            for (int i = 0; i < sprite.SpriteColors.Length; i++)
            {
                Vector2 colPos = sprite.GetVectorFromIndex(i);
                if (sprite.SpriteColors[i].A == 255)
                    DrawAt(Vector2.Add(colPos, pos), sprite.SpriteColors[i]);
            }
        }

        public void Quit()
        {
            if (window.IsOpen)
                window.Close();
            OnQuit();
        }

        #region Dispose Code
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                window.Dispose();
                pixel.Dispose();
            }
        }

        ~Game()
        {
            Dispose(false);
        }
        #endregion
    }
}
