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

        Button btnPlay;

        public int score;

        bool dead;

        public int common = 10;
        public int mob_vishlo;
        public static Random random;

        private List<Mob> mobs = new List<Mob>();
        private float _timer;

        Texture2D TowerTexture;
        Rectangle TowerRectangle;
        Vector2 TowerOriginal;
        Vector2 TowerPosition;
        float TowerRotation;
        private const float tangVelocity = 4f;
        private Vector2 sight;
        List<Arrow> arrows = new List<Arrow>();

        private int ScreenWidth;
        private int ScreenHeigth;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            var metric = new Android.Util.DisplayMetrics();
            Activity.WindowManager.DefaultDisplay.GetMetrics(metric);
            // установка параметров экрана          
            graphics.PreferredBackBufferWidth = metric.WidthPixels;
            graphics.PreferredBackBufferHeight = metric.HeightPixels;
            graphics.IsFullScreen = true;
            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
            random = new Random();
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

            btnPlay = new Button(Content.Load<Texture2D>("Button"), graphics.GraphicsDevice);
            btnPlay.setPosition(new Vector2(400, 200));

            TowerTexture = Content.Load<Texture2D>("Tower");
            TowerPosition = new Vector2(ScreenWidth / 4 - TowerTexture.Width / 4, ScreenHeigth / 2 - TowerTexture.Height / 4);

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
        protected override void Update(GameTime gameTime)
        {
            foreach (Arrow bullet in arrows)
            {
                foreach (Mob onemob in mobs)
                {
                    if (bullet.Rectangle.Intersects(onemob.Rectangle)&&(bullet.isVisible==true))
                    {
                        onemob.isDead = true;
                        GameProcess.MaxScore++;
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
            if ((mobs.Count == 0) && (mob_vishlo == 10))
            {
                GameProcess.WinGame();
                btnPlay.isClicked = false;
                mob_vishlo = 0;
            }
            TouchCollection touchCollection = TouchPanel.GetState();
            if (!GameProcess.IsGame)
            {
                if (btnPlay.isClicked == true)
                {
                    GameProcess = new GameProcess();
                    GameProcess.IsGame = true;
                }
                // CurrentGameState = GameState.Game;
                btnPlay.Update(touchCollection);
            }
            else
            {
                foreach (TouchLocation tl in touchCollection)
                {
                    if ((tl.State == TouchLocationState.Pressed))
                    {
                        sight.X = tl.Position.X - TowerPosition.X;
                        sight.Y = tl.Position.Y - TowerPosition.Y;
                        TowerRotation = (float)Math.Atan2(sight.Y, sight.X);
                        Shooting();
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
                }
                if ((_timer > 0.5f) && (mob_vishlo < common))
                {
                    //  random = Convert.ToInt32(new Random(800));
                    _timer = 0;
                    //  Mob.LoadContent(Content);
                    Mob = new Mob(new Vector2(800, random.Next(400)));
                    Mob.LoadContent(Content);
                    mobs.Add(Mob);
                    mob_vishlo++;
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
        public void UpdateArrow()
        {
            foreach (Arrow OneArrow in arrows)
            {
                OneArrow.ArrowPosition += OneArrow.ArrowVelocity;
                if (Vector2.Distance(OneArrow.ArrowPosition, TowerPosition) > 600)
                    OneArrow.isVisible = false;
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
            newArrow.ArrowVelocity = new Vector2((float)Math.Cos(TowerRotation), (float)Math.Sin(TowerRotation)) * 5f;
            newArrow.ArrowPosition = TowerPosition + newArrow.ArrowVelocity * 5f;
            newArrow.ArrowRotation = (float)Math.Atan2(sight.Y, sight.X);
            newArrow.isVisible = true;
            if (arrows.Count < 5)
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
                btnPlay.Draw(spriteBatch);
                Console.WriteLine(GameProcess.MaxScore);
                spriteBatch.DrawString(font, "Score: " + GameProcess.MaxScore, new Vector2(10, 20), Color.Black);
            }
            else
            {             
                spriteBatch.Draw(TowerTexture, TowerPosition, null, Color.White, 0f, TowerOriginal, 1f, SpriteEffects.None, 0);
                foreach (Arrow bullet in arrows)
                {
                    bullet.Draw(spriteBatch);
                }
                foreach (Mob onemob in mobs)
                {
                    onemob.Draw(spriteBatch);
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
