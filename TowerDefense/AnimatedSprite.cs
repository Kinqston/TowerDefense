using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefense
{
    abstract class AnimatedSprite
    {
        public Vector2 sPosition;
        protected Texture2D sTexture;
        private Rectangle[] sRectangle;
        private int FrameIndex;

        private double timeElapsed;
        private double timeToUpdate;

        public Vector2 sDirection;

        public int FramesPerSeconds {
            set { timeToUpdate = (1f / value);}
        }

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)sPosition.X, (int)sPosition.Y, sTexture.Width/12, sTexture.Height);
            }
        }

        public AnimatedSprite(Vector2 position)
        {
            sPosition = position;
        } 
        public void AddAnimation(int frames)
        {
            int Width = sTexture.Width / frames;
            sRectangle = new Rectangle[frames];
            for (int i = 0; i<frames;i++)
            {
                sRectangle[i] = new Rectangle(i * Width, 0, Width, sTexture.Height);
            }

        }

        public virtual void Update(GameTime gametime)
        {
            timeElapsed += gametime.ElapsedGameTime.TotalSeconds;
            if (timeElapsed>timeToUpdate)
            {
                timeElapsed -= timeToUpdate;
                if (FrameIndex<sRectangle.Length-1)
                {
                    FrameIndex++;
                }
                else
                {
                    FrameIndex = 0;
                }
            } 
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sTexture,sPosition,sRectangle[FrameIndex],Color.White);
        }
    }
}