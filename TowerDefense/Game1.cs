using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;

namespace Arrow
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

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
            TowerTexture = Content.Load<Texture2D>("Tower");
            TowerPosition = new Vector2(ScreenWidth / 4 - TowerTexture.Width/4, ScreenHeigth / 2- TowerTexture.Height/4);

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
            TouchCollection touchCollection = TouchPanel.GetState();
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
            
            base.Update(gameTime);
        }
        public void UpdateArrow()
        {
            foreach (Arrow OneArrow in arrows)
            {
                OneArrow.ArrowPosition += OneArrow.ArrowVelocity;
                if (Vector2.Distance(OneArrow.ArrowPosition, TowerPosition) > 700)
                    OneArrow.isVisible = false;
            }
            for(int i = 0; i < arrows.Count; i++)
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
            newArrow.ArrowVelocity = new Vector2((float)Math.Cos(TowerRotation),(float)Math.Sin(TowerRotation))*5f;
            newArrow.ArrowPosition = TowerPosition + newArrow.ArrowVelocity * 5f;
            newArrow.ArrowRotation = (float)Math.Atan2(sight.Y, sight.X);
            newArrow.isVisible = true;
            if(arrows.Count<3)
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

            spriteBatch.Draw(TowerTexture,TowerPosition,null,Color.White,0f,TowerOriginal,1f,SpriteEffects.None,0);
            foreach (Arrow bullet in arrows)
            {
                bullet.Draw(spriteBatch);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
