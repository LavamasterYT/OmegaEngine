using System;
using System.Collections.Generic;

namespace OmegaEngine.Example
{
    class Frogger : Game
    {
        #region Sprite Imports
        Sprite sCar1 = new Sprite("Assets/Frogger/car_1.png");
        Sprite sCar2 = new Sprite("Assets/Frogger/car_2.png");
        Sprite sCar3 = new Sprite("Assets/Frogger/car_3.png");
        Sprite sFrog = new Sprite("Assets/Frogger/frog.png");
        Sprite sGrass = new Sprite("Assets/Frogger/grass.png");
        Sprite sWater = new Sprite("Assets/Frogger/water.png");
        Sprite sRoad = new Sprite("Assets/Frogger/road.png");
        Sprite sLogCenter = new Sprite("Assets/Frogger/log_center.png");
        Sprite sLogLeft = new Sprite("Assets/Frogger/log_left.png");
        Sprite sLogRight = new Sprite("Assets/Frogger/log_right.png");
        Sprite sTruckP1 = new Sprite("Assets/Frogger/truck1.png");
        Sprite sTruckP2 = new Sprite("Assets/Frogger/truck2.png");
        #endregion

        List<int> BackgroundMap;

        int car1Tick = 0;
        int car2Tick = 0;
        int car3Tick = 0;

        int truckTick = 0;

        #region Vectors
        Vector2 vDimensions = new Vector2(14, 11);
        Vector2 vPlayer = new Vector2(0, 0);
        Vector2 vCar1 = new Vector2(13 * 16, 6 * 16);
        Vector2 vCar2 = new Vector2(13 * 16, 7 * 16);
        Vector2 vCar3 = new Vector2(13 * 16, 8* 16);
        //Vector2 vTruck = new Vector2(0, 9);
        #endregion

        public Frogger() : base(1120, 880, 224, 176, 60, true)
        {
            BackgroundColor = new RGBA(0, 0, 0);
        }

        public override void BeforeStart()
        {
            BackgroundMap = new List<int>() { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
            vPlayer = new Vector2((vDimensions.X / 2) * 16, (vDimensions.Y - 1) * 16);
            WindowTitle = "Frogger Game";
            RPC = new DiscordRPC();
            RPC.SetPresence();
        }

        public override bool Update()
        {
            car1Tick++;
            car2Tick++;
            car3Tick++;

            if (car1Tick >= 3)
            {
                vCar1.X -= 16;
                if (vCar1.X < 0)
                    vCar1.X = 13 * 16;
                car1Tick = 0;
            }
            if (car2Tick >= 2)
            {
                vCar2.X -= 16;
                if (vCar2.X < 0)
                    vCar2.X = 13 * 16;
                car2Tick = 0;
            }
            if (car3Tick >= 4)
            {
                vCar3.X -= 16;
                if (vCar3.X < 0)
                    vCar3.X = 13 * 16;
                car3Tick = 0;
            }

            return false;
        }

        public override void Draw()
        {
            for (int i = 0; i < BackgroundMap.Count; i++)
            {
                int x = i % 14;
                int y = i / 14;
                x *= 16;
                y *= 16;

                switch (BackgroundMap[i])
                {
                    case 1:
                        DrawSprite(sGrass, new Vector2(x, y));
                        break;
                    case 2:
                        DrawSprite(sRoad, new Vector2(x, y));
                        break;
                    case 3:
                        DrawSprite(sWater, new Vector2(x, y));
                        break;
                }
            }

            DrawSprite(sCar1, vCar1);
            DrawSprite(sCar2, vCar2);
            DrawSprite(sCar3, vCar3);
            DrawSprite(sFrog, vPlayer);

            base.Draw();
        }

        public override void KeyDown(SDL.SDL_Keycode key)
        {
            switch (key)
            {
                case SDL.SDL_Keycode.SDLK_w or SDL.SDL_Keycode.SDLK_UP:
                    vPlayer = new Vector2(vPlayer.X, vPlayer.Y - 16);
                    break;
                case SDL.SDL_Keycode.SDLK_s or SDL.SDL_Keycode.SDLK_DOWN:
                    vPlayer = new Vector2(vPlayer.X, vPlayer.Y + 16);
                    break;
                case SDL.SDL_Keycode.SDLK_a or SDL.SDL_Keycode.SDLK_LEFT:
                    vPlayer = new Vector2(vPlayer.X - 16, vPlayer.Y);
                    break;
                case SDL.SDL_Keycode.SDLK_d or SDL.SDL_Keycode.SDLK_RIGHT:
                    vPlayer = new Vector2(vPlayer.X + 16, vPlayer.Y);
                    break;
            }
        }
    }
}