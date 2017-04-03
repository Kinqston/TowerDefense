using Android.OS;
using Android.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using TowerDefense;

namespace Arrow
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private SpriteFont font;
        private GameProcess GameProcess = new GameProcess();
        enum GameState
        {
            MainMenu,
            Game,
        }
        GameState CurrentGameState = GameState.MainMenu;
        Mob Mob;

        Button btnClassic;
        Button btnDethmatch;
        Button btnRetry;
        Button btnResume;
        Button btnMenu;
        Button btnShop;
        Button btnShop_win_lose;
        Button btnPause;
        Button btnExit;
        Button btnContinue;
        Button btnUpgrate;
        Button btnBallista;
        Button btnHard;

        Button btn1_agility;
        Button btn3_agility;
        Button btn5_agility;

        Button btn1_strength;
        Button btn3_strength;
        Button btn5_strength;

        Texture2D WinTexture;
        Texture2D LoseTexture;

        public int score;

        bool dead;

        public int common = 10;
        public int mob_vishlo;
        public static Random random;

        private List<Mob> mobs = new List<Mob>();
        private float _timer;
        private float _timer_arrow;

        Texture2D TowerTexture;
        Texture2D PauseTexture;
        Texture2D Game_background;
        Texture2D Win;
        Rectangle TowerRectangle;
        Vector2 TowerOriginal;
        Vector2 TowerPosition;
        float TowerRotation;
        private const float tangVelocity = 4f;
        private Vector2 sight;
        List<Arrow> arrows = new List<Arrow>();

        private int ScreenWidth;
        private int ScreenHeigth;
        private bool Create;
        private bool Create_1;
        private int random_spawn;
        private Rectangle Rectangle_spawn;

        public static float Dx = 1f;
        public static float Dy = 1f;
        public static double scale_x = 1;
        public static double scale_y = 1;
        private static int NominalWidth = 800;
        private static int NominalHeight = 444;
        private static float NominalWidthCounted;
        private static float NominalHeightCounted;
        private static int CurrentWidth;
        private static int CurrentHeigth;
        private static float deltaY = 0;
        private static float deltaY_1 = 0;
        public static float YTopBorder;
        public static float YBottomBorder;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            var metric = new Android.Util.DisplayMetrics();
            Activity.WindowManager.DefaultDisplay.GetMetrics(metric);
            // установка параметров экрана          
            graphics.PreferredBackBufferWidth = metric.WidthPixels;
            graphics.PreferredBackBufferHeight = metric.HeightPixels;
           
            CurrentWidth = graphics.PreferredBackBufferWidth;
            CurrentHeigth = graphics.PreferredBackBufferHeight;
            
            graphics.IsFullScreen = true;
            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;         
            random = new Random();
            UpdateScreenAttributies();
        }

        public void UpdateScreenAttributies()
        {
            Dx = (float)CurrentWidth / NominalWidth;
            Dy = (float)CurrentHeigth / NominalHeight;

            scale_x = CurrentWidth / NominalWidth;
            scale_y = CurrentHeigth / NominalHeight;

            NominalHeightCounted = CurrentHeigth / Dx;
            NominalWidthCounted = CurrentWidth / Dx;

            int check = Math.Abs(CurrentHeigth - CurrentWidth / 16 * 9);
            if (check > 10)
                deltaY = (float)check / 2; // недостающее расстояние до 16:9 по п оси Y (в абсолютных координатах)
            deltaY_1 = -(CurrentWidth / 16 * 10 - CurrentWidth / 16 * 9) / 2f;

            YTopBorder = -deltaY / Dx; // координата точки в левом верхнем углу (в вируальных координатах)
            YBottomBorder = NominalHeight + (180); // координата точки в нижнем верхнем углу (в виртуальных координатах)
        }
        public static float AbsoluteX(float x)
        {
            return x * Dx;
        }
        public static float AbsoluteY(float y)
        {
            return y * Dx;
        }
        public static int AbsoluteScaleX(int x)
        {
            return (int)Math.Round(x * scale_x);
        }
        public static int AbsoluteScaleY(int y)
        {
            return (int)Math.Round(y * scale_y);
        }
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            ScreenWidth = GraphicsDevice.Viewport.Width;
            ScreenHeigth = GraphicsDevice.Viewport.Height;

            Console.WriteLine(ScreenWidth);
            Console.WriteLine(ScreenHeigth);

            spriteBatch = new SpriteBatch(GraphicsDevice);

            WinTexture = Content.Load<Texture2D>("WinTexture");
            LoseTexture = Content.Load<Texture2D>("LoseTexture");
            PauseTexture = Content.Load<Texture2D>("BackgroundPause");
            Game_background = Content.Load<Texture2D>("Game_background");
            Win = Content.Load<Texture2D>("Win");

            btnPause = new Button(Content.Load<Texture2D>("Pause"), graphics.GraphicsDevice);
            btnPause.setPosition(new Vector2(ScreenWidth-btnPause.texture.Width-25, 0));

            btnResume = new Button(Content.Load<Texture2D>("Resume"), graphics.GraphicsDevice);
            

            btnRetry = new Button(Content.Load<Texture2D>("Retry"), graphics.GraphicsDevice);
            btnRetry.setPosition(new Vector2(550 , ScreenHeigth - ScreenHeigth / 4));

            btnContinue = new Button(Content.Load<Texture2D>("Continue"), graphics.GraphicsDevice);
            btnContinue.setPosition(new Vector2(550*Dx , ScreenHeigth - ScreenHeigth / 4));

            btnMenu = new Button(Content.Load<Texture2D>("Menu"), graphics.GraphicsDevice);
            btnMenu.setPosition(new Vector2(150*Dx, ScreenHeigth -ScreenHeigth/4));         

            btnShop_win_lose = new Button(Content.Load<Texture2D>("Shop"), graphics.GraphicsDevice);
            btnShop_win_lose.setPosition(new Vector2(350 * Dx, ScreenHeigth - ScreenHeigth / 4));

            btnHard = new Button(Content.Load<Texture2D>("Hard"), graphics.GraphicsDevice);
            btnHard.setPosition(new Vector2(ScreenWidth / 2 - btnHard.texture.Width / 4-20, AbsoluteScaleY(150)));

            btnBallista = new Button(Content.Load<Texture2D>("ballista"), graphics.GraphicsDevice);
            btnBallista.setPosition(new Vector2(ScreenWidth / 2 - btnBallista.texture.Width / 4-20, AbsoluteScaleY(200)));

            btnClassic = new Button(Content.Load<Texture2D>("Classic"), graphics.GraphicsDevice);
            btnClassic.setPosition(new Vector2(ScreenWidth/2-btnClassic.texture.Width/4, AbsoluteScaleY(100)));

            btnDethmatch = new Button(Content.Load<Texture2D>("Dethmatch"), graphics.GraphicsDevice);
            btnDethmatch.setPosition(new Vector2(ScreenWidth/2-btnDethmatch.texture.Width/4, AbsoluteScaleY(250)));

            btnShop = new Button(Content.Load<Texture2D>("Shop"), graphics.GraphicsDevice);
            btnShop.setPosition(new Vector2(ScreenWidth / 2 - btnShop.texture.Width / 4, AbsoluteScaleY(300)));

            btnUpgrate = new Button(Content.Load<Texture2D>("Upgrate"), graphics.GraphicsDevice);
            btnUpgrate.setPosition(new Vector2(ScreenWidth / 2 - btnUpgrate.texture.Width / 4-20, AbsoluteScaleY(350)));

            btnExit = new Button(Content.Load<Texture2D>("Exit"), graphics.GraphicsDevice);
            btnExit.setPosition(new Vector2(ScreenWidth / 2 - btnExit.texture.Width / 4, AbsoluteScaleY(400)));

            TowerTexture = Content.Load<Texture2D>("Tower");
            TowerPosition = new Vector2(TowerTexture.Width/2, ScreenHeigth / 2);

            btn1_agility = new Button(Content.Load<Texture2D>("+1"), graphics.GraphicsDevice);
            btn1_agility.setPosition(new Vector2(ScreenWidth / 2, 150));

            btn3_agility = new Button(Content.Load<Texture2D>("+3"), graphics.GraphicsDevice);
            btn3_agility.setPosition(new Vector2(ScreenWidth / 2 +100, 150));

            btn5_agility = new Button(Content.Load<Texture2D>("+5"), graphics.GraphicsDevice);
            btn5_agility.setPosition(new Vector2(ScreenWidth / 2 + 200, 150));

            btn1_strength = new Button(Content.Load<Texture2D>("+1"), graphics.GraphicsDevice);
            btn1_strength.setPosition(new Vector2(ScreenWidth / 2, 250));

            btn3_strength = new Button(Content.Load<Texture2D>("+3"), graphics.GraphicsDevice);
            btn3_strength.setPosition(new Vector2(ScreenWidth / 2 + 100, 250));

            btn5_strength = new Button(Content.Load<Texture2D>("+5"), graphics.GraphicsDevice);
            btn5_strength.setPosition(new Vector2(ScreenWidth / 2 + 200, 250));

            font = Content.Load<SpriteFont>("Font");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// 
        public void NewGame()
        {
            mob_vishlo = 0;
            mobs = new List<Mob>();
            arrows = new List<Arrow>();
        }
        protected override void Update(GameTime gameTime)
        {    
            foreach (Arrow bullet in arrows)
            {
                foreach (Mob onemob in mobs)
                {
                    if (bullet.Rectangle.Intersects(onemob.Rectangle)&&(bullet.isVisible==true))
                    {
                        onemob.HP = onemob.HP - GameProcess.Arrow_strength;
                        if (onemob.HP <= 0)
                        {
                            onemob.isDead = true;
                            if(GameProcess.IsClassic)
                                GameProcess.Score+=10;
                            if((GameProcess.IsBallista)||(GameProcess.IsHard))
                                GameProcess.Score+=5;
                            if (GameProcess.IsDethmatch)
                                GameProcess.Score+=2;
                        }
                        if (!GameProcess.IsBallista)
                            bullet.isVisible = false;
                    }
                }
            }
            for (int j = 0; j < mobs.Count; j++)
            {
                if (mobs[j].isDead == true)
                {
                    mobs.RemoveAt(j);
                    j--;
                }
            }
           
            TouchCollection touchCollection = TouchPanel.GetState();
            if (!GameProcess.IsGame)                          // Нет функционала магазина
            {
                if (GameProcess.IsWin)                                       //Win
                {
                    if (btnMenu.isClicked == true)
                    {
                        GameProcess.IsWin = false;
                        GameProcess.IsClassic = false;
                        GameProcess.IsHard = false;
                        GameProcess.IsBallista = false;
                        GameProcess.IsDethmatch = false;
                        NewGame();
                        GameProcess.ReadScore();                        
                    }
                    if (btnContinue.isClicked == true)
                    {
                        NewGame();
                        if (GameProcess.IsClassic)
                            GameProcess.Classic();
                        if (GameProcess.IsHard)
                            GameProcess.Hard();
                        if (GameProcess.IsBallista)
                            GameProcess.Ballista();
                        if (GameProcess.IsDethmatch)                     
                            GameProcess.Dethmatch();                                             
                    }
                    btnMenu.Update(touchCollection, gameTime);
                    btnContinue.Update(touchCollection, gameTime);
                    btnShop_win_lose.Update(touchCollection, gameTime);
                }
                else
                {
                    if (GameProcess.IsLose)                                       //Lose
                    {
                        if (btnMenu.isClicked == true)
                        {
                            GameProcess.IsLose = false;
                            GameProcess.IsClassic = false;
                            GameProcess.IsHard = false;
                            GameProcess.IsBallista = false;
                            GameProcess.IsDethmatch = false;
                            GameProcess.ReadScore();
                        }
                        if (btnRetry.isClicked == true)
                        {
                            if (GameProcess.IsClassic)
                                GameProcess.Classic();
                            if (GameProcess.IsHard)
                                GameProcess.Hard();
                            if (GameProcess.IsBallista)
                                GameProcess.Ballista();
                            if (GameProcess.IsDethmatch)
                                GameProcess.Dethmatch();
                        }
                        btnRetry.Update(touchCollection,gameTime);
                        btnMenu.Update(touchCollection, gameTime);
                        btnShop_win_lose.Update(touchCollection, gameTime);
                    }
                    else                                                           
                    {
                        if (GameProcess.IsUpgrate)                                    //улучшения
                        {
                            btnUpgrate.isClicked = false;
                            if (btnMenu.isClicked)
                            {
                                GameProcess.IsUpgrate = false;
                            }
                            if ((btn1_strength.isClicked)&(GameProcess.MaxScore >= 500))
                            {                             
                                GameProcess.Upgrate(1,500,"strength");
                                GameProcess.MaxScore = GameProcess.MaxScore - 500;
                                GameProcess.Arrow_strength = GameProcess.Arrow_strength + 1;
                                UpgrateButton();                            
                            }
                            if ((btn3_strength.isClicked) & (GameProcess.MaxScore >= 900))
                            {
                                GameProcess.Upgrate(3, 900, "strength");
                                GameProcess.MaxScore = GameProcess.MaxScore - 900;
                                GameProcess.Arrow_strength = GameProcess.Arrow_strength + 3;
                                UpgrateButton();
                            }
                            if ((btn5_strength.isClicked) & (GameProcess.MaxScore >= 1300))
                            {
                                GameProcess.Upgrate(5, 1300, "strength");
                                GameProcess.MaxScore = GameProcess.MaxScore - 1300;
                                GameProcess.Arrow_strength = GameProcess.Arrow_strength + 5;
                                UpgrateButton();
                            }
                            if ((btn1_agility.isClicked) & (GameProcess.MaxScore >= 500))
                            {
                                GameProcess.Upgrate(1, 500, "agility");
                                GameProcess.MaxScore = GameProcess.MaxScore - 500;
                                GameProcess.speedArrow = GameProcess.speedArrow + 0.1f;
                                UpgrateButton();
                            }
                            if ((btn3_agility.isClicked) & (GameProcess.MaxScore >= 900))
                            {
                                GameProcess.Upgrate(3, 900, "agility");
                                GameProcess.MaxScore = GameProcess.MaxScore - 900;
                                GameProcess.speedArrow = GameProcess.speedArrow + 0.3f;
                                UpgrateButton();
                            }
                            if ((btn5_agility.isClicked) & (GameProcess.MaxScore >= 1300))
                            {
                                GameProcess.Upgrate(5, 1300, "agility");
                                GameProcess.MaxScore = GameProcess.MaxScore - 1300;
                                GameProcess.speedArrow = GameProcess.speedArrow + 0.5f;
                                UpgrateButton();
                            }
                            btn1_agility.Update(touchCollection, gameTime);
                            btn3_agility.Update(touchCollection, gameTime);
                            btn5_agility.Update(touchCollection, gameTime);
                            btn1_strength.Update(touchCollection, gameTime);
                            btn3_strength.Update(touchCollection, gameTime);
                            btn5_strength.Update(touchCollection, gameTime);                           
                            btnMenu.setPosition(new Vector2(100*Dx, ScreenHeigth - ScreenHeigth / 5));
                            btnMenu.Update(touchCollection, gameTime);
                        }
                        else
                        {
                            btnMenu.setPosition(new Vector2(150 * Dx, ScreenHeigth - ScreenHeigth / 4));
                            btnMenu.isClicked = false;                      
                            if (btnClassic.isClicked == true)                       //меню
                            {
                                GameProcess = new GameProcess();                             
                                GameProcess.Classic();
                                btnMenu.isClicked = false;
                                btnRetry.isClicked = false;
                                btnContinue.isClicked = false;
                            }
                            if (btnHard.isClicked == true)
                            {
                                GameProcess = new GameProcess();
                                GameProcess.Hard();
                                btnRetry.isClicked = false;
                                btnMenu.isClicked = false;
                                btnContinue.isClicked = false;
                            }
                            if (btnBallista.isClicked == true)
                            {
                                GameProcess = new GameProcess();
                                GameProcess.Ballista();
                                btnRetry.isClicked = false;
                                btnMenu.isClicked = false;
                                btnContinue.isClicked = false;
                            }
                            if (btnDethmatch.isClicked == true)
                            {
                                GameProcess = new GameProcess();
                                GameProcess.Dethmatch();
                                btnRetry.isClicked = false;
                                btnMenu.isClicked = false;
                                btnContinue.isClicked = false;
                            }
                            if (btnUpgrate.isClicked == true)
                            {
                                GameProcess.IsUpgrate = true;
                            }
                            if (btnExit.isClicked == true)
                            {
                                Exit();
                            }
                            // CurrentGameState = GameState.Game;
                            btnClassic.Update(touchCollection, gameTime);
                            btnBallista.Update(touchCollection, gameTime);
                            btnHard.Update(touchCollection, gameTime);
                            btnDethmatch.Update(touchCollection, gameTime);
                            btnShop.Update(touchCollection, gameTime);
                            btnUpgrate.Update(touchCollection, gameTime);
                            btnExit.Update(touchCollection, gameTime);
                        }
                    }
                }
            }
            else
            {
                if (GameProcess.IsPause)                                    //Пауза
                {
                    btnPause.isClicked = false;
                    if (btnMenu.isClicked)
                    {
                        btnClassic.isClicked = false;
                        btnHard.isClicked = false;
                        btnBallista.isClicked = false;
                        btnDethmatch.isClicked = false;
                        GameProcess.IsGame = false;
                        GameProcess.IsPause = false;
                        GameProcess.IsLose = false;
                        GameProcess.IsClassic = false;
                        GameProcess.IsHard = false;
                        GameProcess.IsBallista = false;
                        GameProcess.IsDethmatch = false;
                        NewGame();
                    }
                    if (btnResume.isClicked)
                    {
                        btnMenu.setPosition(new Vector2(150 * Dx, ScreenHeigth - ScreenHeigth / 4));
                        GameProcess.IsPause = false;
                        btnResume.isClicked = false;
                    }
                    btnMenu.Update(touchCollection, gameTime);
                    btnResume.Update(touchCollection, gameTime);
                }
                else
                {
                    btnContinue.isClicked = false;
                    btnRetry.isClicked = false;
                    btnPause.Update(touchCollection, gameTime);
                    if (btnPause.isClicked)
                        GameProcess.IsPause = true;
                    else
                    {
                        _timer_arrow += (float)gameTime.ElapsedGameTime.TotalSeconds;
                        foreach (TouchLocation tl in touchCollection)
                        {
                            if ((GameProcess.IsDethmatch)||(GameProcess.IsClassic))
                            {
                                sight.X = tl.Position.X - TowerPosition.X;
                                sight.Y = tl.Position.Y - TowerPosition.Y;
                                TowerRotation = (float)Math.Atan2(sight.Y, sight.X);
                                if (_timer_arrow > GameProcess.arrow_spawn)
                                {
                                    _timer_arrow = 0;
                                    Shooting();
                                }
                            }
                            else
                            {
                                if ((tl.State == TouchLocationState.Pressed))
                                {
                                    sight.X = tl.Position.X - TowerPosition.X;
                                    sight.Y = tl.Position.Y - TowerPosition.Y;
                                    TowerRotation = (float)Math.Atan2(sight.Y, sight.X);
                                    Shooting();
                                }
                            }
                        }
                        UpdateArrow();
                        TowerRectangle = new Rectangle((int)TowerPosition.X, (int)TowerPosition.Y, TowerTexture.Width, TowerTexture.Height);
                        TowerOriginal = new Vector2(TowerRectangle.Width / 2, TowerRectangle.Height / 2);

                        //Mob.Update(gameTime);
                        _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                        foreach (Mob onemob in mobs)
                        {
                            onemob.Update(gameTime);
                            if (GameProcess.IsHard)
                            {
                                if (onemob.sPosition.Y < 10)
                                {
                                    onemob.speed_y = 0;
                                    if (random.Next(3)==2)
                                    {
                                        onemob.speed_y = 35;
                                    }
                                }
                                if (onemob.sPosition.Y > 390)
                                {
                                    onemob.speed_y = 0;
                                    if (random.Next(3) == 2)
                                    {
                                        onemob.speed_y = -35;
                                    }
                                }
                            }
                            if ((onemob.Rectangle.X < 0) || (GameProcess.countArrow == 0))
                            {
                                GameProcess.LoseGame();
                                NewGame();
                                btnDethmatch.isClicked = false;
                                btnClassic.isClicked = false;
                                btnHard.isClicked = false;
                                btnBallista.isClicked = false;
                            }
                        }
                        if ((_timer > GameProcess.timespawn) && (mob_vishlo < GameProcess.mobs))
                        {
                            _timer = 0;
                            Create = false;
                            Create_1 = true;
                            while (Create == false)
                                {
                                    Create_1 = true;
                                    random_spawn = random.Next(AbsoluteScaleY(400));
                                    Rectangle_spawn = new Rectangle(AbsoluteScaleX(800), random_spawn, AbsoluteScaleX(10), AbsoluteScaleY(10));
                                    foreach (Mob onemob in mobs)
                                    {
                                        if (onemob.Rectangle.Intersects(Rectangle_spawn))
                                        {
                                            Create_1 = false;
                                        }
                                    }
                                    if (Create_1 == true)
                                    {
                                        Create = true;
                                    }
                                }
                            Mob = new Mob(new Vector2(800*Dx, random_spawn));
                            Mob.scale = Dx;
                            Mob.speed = GameProcess.speedMobs;
                            Mob.HP = GameProcess.HP_mobs;
                            Mob.LoadContent(Content);
                            mobs.Add(Mob);
                            mob_vishlo++;
                            if (GameProcess.IsDethmatch)
                            {
                                if (GameProcess.timespawn > 0.3f)
                                    GameProcess.timespawn = GameProcess.timespawn - 0.1f;
                                if (mob_vishlo % 10 == 0)
                                {
                                    GameProcess.speedMobs -= 2;
                                }
                            }
                            if (GameProcess.IsHard)
                            {
                                foreach (Mob onemob in mobs)
                                {
                                    if (random.Next(5) == 1)
                                    {
                                        onemob.speed_y = 35;
                                    }
                                    else
                                    {
                                        if (random.Next(5) == 5)
                                        {
                                            onemob.speed_y = -35;
                                        }
                                    }
                                }
                            }
                        }
                        if ((mobs.Count == 0) && (mob_vishlo == GameProcess.mobs))
                        {
                            GameProcess.WinGame();
                            btnClassic.isClicked = false;
                            btnHard.isClicked = false;
                            btnBallista.isClicked = false;
                            //NewGame();
                        }
                    }
                }
            }
            //foreach (Arrow onearrow in arrows)
            //{
            //    foreach (Mob onemob in mobs)
            //    {
            //        if (onearrow.Rectangle.Intersects(Mob.Rectangle))
            //        {
            //            onemob.isDead = true;
            //        }
            //    }
            //}

            //Console.WriteLine(bullet.Rectangle.Center + "стрела");
            //Console.WriteLine(Mob.Rectangle.X + " моб ");


            base.Update(gameTime);
        }
        public void UpgrateButton()
        {
            btn1_agility.isClicked = false;
            btn3_agility.isClicked = false;
            btn5_agility.isClicked = false;
            btn1_strength.isClicked = false;
            btn3_strength.isClicked = false;
            btn5_strength.isClicked = false;
        }
        public void UpdateArrow()
        {
            foreach (Arrow OneArrow in arrows)
            {
                OneArrow.ArrowPosition += OneArrow.ArrowVelocity;
                if (Vector2.Distance(OneArrow.ArrowPosition, TowerPosition) > AbsoluteScaleX(800))
                {                   
                    OneArrow.isVisible = false;
                    if(GameProcess.IsHard)
                        GameProcess.countArrow--;
                }
            }
            for (int i = 0; i < arrows.Count; i++)
            {
                if (!arrows[i].isVisible)
                {
                    arrows.RemoveAt(i);
                    i--;
                }
            }
        }
        public void Shooting()
        {
            Arrow newArrow = new Arrow(Content.Load<Texture2D>("Arrow"));



            newArrow.ArrowVelocity = new Vector2((float)Math.Cos(TowerRotation), (float)Math.Sin(TowerRotation)) * GameProcess.speedArrow*Dx;
            newArrow.ArrowPosition = TowerPosition + newArrow.ArrowVelocity;
            newArrow.sight = Math.Atan2(sight.Y, sight.X);
            newArrow.ArrowRotation = (float)Math.Atan2(sight.Y, sight.X);
            newArrow.scale = Dx;
            newArrow.isVisible = true;
            if (arrows.Count < GameProcess.countArrow)
                arrows.Add(newArrow);
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            if (!GameProcess.IsGame)
            {
                if (GameProcess.IsWin)
                {
                    //spriteBatch.Draw(WinTexture, new Vector2(AbsoluteX(0), AbsoluteY(0)), new Rectangle(0, 0, WinTexture.Width, WinTexture.Height), Color.White,0,new Vector2(0,0),1*Dx,SpriteEffects.None,0);
                    spriteBatch.Draw(Game_background, new Vector2(AbsoluteX(0), AbsoluteY(0)), new Rectangle(0, 0, WinTexture.Width, WinTexture.Height), Color.White, 0, new Vector2(0, 0), 1 * Dx, SpriteEffects.None, 0);
                    foreach (Arrow bullet in arrows)
                    {
                        bullet.Draw(spriteBatch);
                    }
                    foreach (Mob onemob in mobs)
                    {
                        onemob.Draw(spriteBatch);
                    }
                    spriteBatch.Draw(Win, new Vector2(AbsoluteX(0), AbsoluteY(0)), new Rectangle(0, 0, Win.Width, Win.Height), Color.White, 0, new Vector2(0,0), 1 * Dx, SpriteEffects.None, 0);
                    spriteBatch.DrawString(font, "Win", new Vector2(370, ScreenHeigth / 3), Color.Black);
                    spriteBatch.DrawString(font, "Points: " + GameProcess.Score, new Vector2(350, ScreenHeigth / 3+20), Color.Black);
                    spriteBatch.DrawString(font, "Total points: " + (GameProcess.MaxScore+GameProcess.Score), new Vector2(350, ScreenHeigth / 3+40), Color.Black);
                    btnContinue.Draw(spriteBatch);
                    btnMenu.Draw(spriteBatch);
                    btnShop_win_lose.Draw(spriteBatch);
                }
                else
                {
                    if (GameProcess.IsLose)
                    {
                        spriteBatch.Draw(LoseTexture, new Vector2(AbsoluteX(0), AbsoluteY(0)), new Rectangle(0, 0, LoseTexture.Width, LoseTexture.Height), Color.White, 0, new Vector2(0, 0), 1 * Dx, SpriteEffects.None, 0);                     
                        spriteBatch.DrawString(font, "Lose", new Vector2(365, ScreenHeigth / 3), Color.Black);
                        spriteBatch.DrawString(font, "Points: " + GameProcess.Score, new Vector2(350, ScreenHeigth / 3 + 20), Color.Black);
                        spriteBatch.DrawString(font, "Total points: " + (GameProcess.MaxScore + GameProcess.Score), new Vector2(350, ScreenHeigth / 3 + 40), Color.Black);
                        btnRetry.Draw(spriteBatch);
                        btnMenu.Draw(spriteBatch);
                        btnShop_win_lose.Draw(spriteBatch);
                    }
                    else
                    {
                        if (GameProcess.IsUpgrate)
                        {
                            spriteBatch.Draw(TowerTexture, new Vector2(100, 100), null, Color.White, 0f, TowerOriginal, 1f, SpriteEffects.None, 0);
                            spriteBatch.DrawString(font, "Upgrate", new Vector2(365, 50), Color.Black);
                            spriteBatch.DrawString(font, "Agility", new Vector2(300, 120), Color.Black);
                            spriteBatch.DrawString(font, "Strength", new Vector2(300, 220), Color.Black);
                            spriteBatch.DrawString(font, "Total points: " + (GameProcess.MaxScore), new Vector2(10, 20), Color.Black);
                            spriteBatch.DrawString(font, "Strength: " + GameProcess.Arrow_strength, new Vector2(150, 20), Color.Black);
                            spriteBatch.DrawString(font, "Agility: " + GameProcess.speedArrow, new Vector2(290, 20), Color.Black);
                            btnMenu.Draw(spriteBatch);

                            spriteBatch.DrawString(font, "500 gold", new Vector2(ScreenWidth / 2 + 25, 180), Color.Black);         //agility
                            spriteBatch.DrawString(font, "900 gold", new Vector2(ScreenWidth / 2 + 125, 180), Color.Black);
                            spriteBatch.DrawString(font, "1300 gold", new Vector2(ScreenWidth / 2 + 225, 180), Color.Black);
                            btn1_agility.Draw(spriteBatch);
                            btn3_agility.Draw(spriteBatch);
                            btn5_agility.Draw(spriteBatch);

                            spriteBatch.DrawString(font, "500 gold", new Vector2(ScreenWidth / 2 + 25, 280), Color.Black);       //strength
                            spriteBatch.DrawString(font, "900 gold", new Vector2(ScreenWidth / 2 + 125, 280), Color.Black);
                            spriteBatch.DrawString(font, "1300 gold", new Vector2(ScreenWidth / 2 + 225, 280), Color.Black);
                            btn1_strength.Draw(spriteBatch);
                            btn3_strength.Draw(spriteBatch);
                            btn5_strength.Draw(spriteBatch);
                        }
                        else
                        {
                            btnClassic.Draw(spriteBatch);
                            btnBallista.Draw(spriteBatch);
                            btnHard.Draw(spriteBatch);
                            btnDethmatch.Draw(spriteBatch);
                            btnShop.Draw(spriteBatch);
                            btnUpgrate.Draw(spriteBatch);
                            btnExit.Draw(spriteBatch);
                            spriteBatch.DrawString(font, "Total points: " + GameProcess.MaxScore, new Vector2(10, 20), Color.Black);
                        }                
                    }
                }
            }
            else
            {
                if (GameProcess.IsPause)
                {
                    spriteBatch.Draw(PauseTexture, new Vector2(0, 0), new Rectangle(0, 0, PauseTexture.Width, PauseTexture.Height), Color.White);
                    spriteBatch.DrawString(font, "Pause" , new Vector2(400, 100), Color.White);
                    btnMenu.setPosition(new Vector2(300, 150));
                    btnResume.setPosition(new Vector2(450,150));
                    btnResume.Draw(spriteBatch);
                    btnMenu.Draw(spriteBatch);
                }
                else
                {
                    spriteBatch.Draw(Game_background, new Vector2(AbsoluteX(0), AbsoluteY(0)), new Rectangle(0, 0, WinTexture.Width, WinTexture.Height), Color.White, 0, new Vector2(0, 0), 1 * Dx, SpriteEffects.None, 0);
                    // spriteBatch.Draw(TowerTexture, new Vector2(AbsoluteX(0), AbsoluteY(0)), new Rectangle(0, 0, TowerTexture.Width, TowerTexture.Height), Color.White, 0, new Vector2(0, 0), 1 * Dx, SpriteEffects.None, 0);
                    spriteBatch.DrawString(font, "Points: " + GameProcess.Score, new Vector2(10, 20), Color.Black);
                    btnPause.Draw(spriteBatch);
                    if (GameProcess.IsHard)
                        spriteBatch.DrawString(font, "Arrows: " + GameProcess.countArrow, new Vector2(ScreenWidth / 2 - 30, 20), Color.Black);
                    if (GameProcess.IsClassic)
                        spriteBatch.DrawString(font, "Level: " + GameProcess.lvl_game, new Vector2(ScreenWidth / 2 - 30, 20), Color.Black);
                    if (GameProcess.IsBallista)
                        spriteBatch.DrawString(font, "Level: " + GameProcess.lvl_game, new Vector2(ScreenWidth / 2 - 30, 20), Color.Black);
                    foreach (Arrow bullet in arrows)
                    {
                        bullet.Draw(spriteBatch);
                    }
                    foreach (Mob onemob in mobs)
                    {
                        onemob.Draw(spriteBatch);
                    }
                }
            }     
                //foreach (Arrow bullet in arrows)
                //{
                //    if (bullet.Rectangle.Intersects(onemob.Rectangle))
                //    {
                //        onemob.isDead = true;
                //        Console.WriteLine("EZZZZZZZZZz");
                //    }
                //}
            
            //    if (bullet.Rectangle.Intersects(Mob.Rectangle))
            //    {
            //        onemob.isDead = true;
            //    }                 
            //}
        
    
                    //Console.WriteLine(bullet.Rectangle.Right + "стрела");                   
                           
           // Console.WriteLine(Mob.Rectangle.X);

            //  Console.WriteLine(bullet.Rectangle.X);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
