using System;
using System.Diagnostics;
using static OmegaEngine.SDL;

namespace OmegaEngine
{
    public abstract class Game : IDisposable
    {
        private int framerate;
        private int framesRendered;
        private int pixelWidth;
        private int pixelHeight;
        private bool clearOnEachFrame;
        private Stopwatch frameWatch;
        private RGBA[] screenBuffer;
        private RGBA[] previousScreenBuffer;
        public DiscordRPC RPC;

        // SDL handles
        private IntPtr windowHWND = IntPtr.Zero;
        private IntPtr rendererHWND = IntPtr.Zero;


        public string WindowTitle = "";
        public readonly int WindowWidth;
        public readonly int WindowHeight;
        public readonly int GridWidth;
        public readonly int GridHeight;

        public RGBA BackgroundColor;

        // Methods

        /// <summary>
        /// RETURN TRUE IF QUITTING, ELSE FALSE
        /// </summary>
        /// <returns>RETURN TRUE IF QUITTING, ELSE FALSE</returns>
        public abstract bool Update();
        public virtual void KeyDown(SDL_Keycode key) { }
        public virtual void KeyUp(SDL_Keycode key) { }
        public virtual void OnQuit() { }
        public virtual void OnSDLEvent(SDL_Event sdlEvent) { }
        public virtual void BeforeStart() { }

        public Game(int width, int height, int gW, int gH, int fr, bool clear = true)
        {
            framerate = 1000 / fr;

            if (SDL_Init(SDL_INIT_EVERYTHING) < 0)
            {
                Console.WriteLine($"Unable to initialize SDL: {SDL_GetError()}");
            }

            if (SDL_ttf.TTF_Init() < 0)
            {
                Console.WriteLine($"Unable to initialize SDL_ttf: {SDL_GetError()}");
            }

            if (width % gW != 0 || height % gH != 0)
            {
                Console.WriteLine("The window width/height is not compatible with the grid width/height. You may experience issues with some pixels being out of bounds.");
            }

            WindowWidth = width;
            WindowHeight = height;

            pixelHeight = height / gH;
            pixelWidth = width / gW;

            clearOnEachFrame = clear;

            frameWatch = new Stopwatch();

            framesRendered = 0;

            BackgroundColor = new RGBA(0, 0, 0);

            GridWidth = gW;
            GridHeight = gH;

            screenBuffer = new RGBA[gW * gH];
            previousScreenBuffer = new RGBA[gW * gH];

            for (int i = 0; i < screenBuffer.Length; i++)
            {
                screenBuffer[i] = BackgroundColor;
                previousScreenBuffer[i] = BackgroundColor;
            }
        }

        public void Start()
        {
            //SDL_CreateWindowAndRenderer(WindowWidth, WindowHeight, SDL_WindowFlags.SDL_WINDOW_SHOWN, out windowHWND, out rendererHWND);

            windowHWND = SDL_CreateWindow(WindowTitle, 1920 / 2 - WindowWidth / 2, 1080 / 2 - WindowHeight / 2, WindowWidth, WindowHeight, SDL_WindowFlags.SDL_WINDOW_SHOWN);
            rendererHWND = SDL_CreateRenderer(windowHWND, -1, SDL_RendererFlags.SDL_RENDERER_ACCELERATED);

            if (windowHWND == IntPtr.Zero || windowHWND == null)
            {
                Console.WriteLine($"Unable to create window: {SDL_GetError()}");
                return;
            }

            if (rendererHWND == IntPtr.Zero || rendererHWND == null)
            {
                Console.WriteLine($"Unable to create renderer: {SDL_GetError()}");
                return;
            }

            SDL_SetRenderDrawColor(rendererHWND, BackgroundColor.R, BackgroundColor.G, BackgroundColor.B, BackgroundColor.A);
            SDL_RenderClear(rendererHWND);
            BeforeStart();
            frameWatch.Start();
            while (true)
            {
                while (SDL_PollEvent(out SDL_Event sdlEvent) != 0)
                {
                    switch (sdlEvent.type)
                    {
                        case SDL_EventType.SDL_KEYDOWN:
                            KeyDown(sdlEvent.key.keysym.sym);
                            break;
                        case SDL_EventType.SDL_KEYUP:
                            KeyUp(sdlEvent.key.keysym.sym);
                            break;
                        case SDL_EventType.SDL_QUIT:
                            OnQuit();
                            Quit();
                            return;
                    }
                    OnSDLEvent(sdlEvent);
                }

                if (Update())
                {
                    Quit();
                    return;
                }    
                Draw();
                SDL_RenderPresent(rendererHWND);
                SDL_Delay((uint)framerate);

                int fps = -1;
                framesRendered++;

                if (frameWatch.ElapsedMilliseconds >= 1000)
                {
                    fps = framesRendered;
                    frameWatch.Restart();
                    framesRendered = 0;
                }

                if (fps != -1)
                    SDL_SetWindowTitle(windowHWND, $"{WindowTitle} - {fps} fps");

                RPC?.Invoke();
            }
        }

        public virtual void Draw()
        {
            if (clearOnEachFrame)
            {
                for (int i = 0; i < screenBuffer.Length; i++)
                {
                    if (!RGBA.IsEqual(screenBuffer[i], previousScreenBuffer[i]))
                    {
                        SDL_SetRenderDrawColor(rendererHWND, screenBuffer[i].R, screenBuffer[i].G, screenBuffer[i].B, screenBuffer[i].A);

                        int xInitPos = pixelWidth * (i % GridWidth);
                        int xDestPos = xInitPos + pixelWidth;
                        int yInitPos = pixelHeight * (i / GridWidth);
                        int yDestPos = yInitPos + pixelHeight;

                        for (int x = xInitPos; x < xDestPos; x++)
                        {
                            for (int y = yInitPos; y < yDestPos; y++)
                            {
                                SDL_RenderDrawPoint(rendererHWND, x, y);
                            }
                        }
                    }
                }

                Array.Copy(screenBuffer, previousScreenBuffer, screenBuffer.Length);
                for (int i = 0; i < screenBuffer.Length; i++)
                {
                    screenBuffer[i] = BackgroundColor;
                }
            }
        }

        public void DrawTextAt(Vector2 pos, Vector2 size, Font font, string text)
        {
            if (font.hWnd == IntPtr.Zero)
            {
                Console.WriteLine("Unable to open font: " + SDL_GetError());
                Quit();
                return;
            }
            var surMes = SDL_ttf.TTF_RenderText_Solid(font.hWnd, text, font.Color.ToSDL_Color());
            var mes = SDL_CreateTextureFromSurface(rendererHWND, surMes);
            SDL_Rect rect = new SDL_Rect()
            {
                x = pos.X,
                y = pos.Y,
                h = size.Y,
                w = size.X
            };
            SDL_RenderCopy(rendererHWND, mes, IntPtr.Zero, ref rect);
            SDL_FreeSurface(surMes);
            SDL_DestroyTexture(mes);
        }

        public void DrawAt(Vector2 pos, RGBA color)
        {
            if (pos.X >= GridWidth || pos.X < 0 || pos.Y >= GridHeight || pos.Y < 0)
            {
                return;
            }
            screenBuffer[pos.X + GridWidth * pos.Y] = color;
            return;

            //SDL_SetRenderDrawColor(rendererHWND, color.R, color.G, color.B, color.A);

            int xInitPos = pixelWidth * pos.X;
            int xDestPos = xInitPos + pixelWidth;
            int yInitPos = pixelHeight * pos.Y;
            int yDestPos = yInitPos + pixelHeight;

            for (int x = xInitPos; x < xDestPos; x++)
            {
                for (int y = yInitPos; y < yDestPos; y++)
                {
                    //SDL_RenderDrawPoint(rendererHWND, x, y);
                }
            }
        }

        public string GetLastError()
        {
            return SDL_GetError();
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
            SDL_DestroyWindow(windowHWND);
            SDL_DestroyRenderer(rendererHWND);
            SDL_Quit();
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
                windowHWND = IntPtr.Zero;
                rendererHWND = IntPtr.Zero;
                frameWatch.Reset();
            }
        }

        ~Game()
        {
            Dispose(false);
        }
        #endregion
    }
}
