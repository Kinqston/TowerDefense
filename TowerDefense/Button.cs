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
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace TowerDefense
{
    class Button
    {
        public Texture2D texture;
        Vector2 position;
        Rectangle rectangle;
        private float _timer;
        Color colour = new Color(255, 255, 255, 255);
        public Vector2 size;
        public Button(Texture2D newTexture, GraphicsDevice graphics)
        {
            texture = newTexture;
            size = new Vector2(graphics.Viewport.Width / 10, graphics.Viewport.Height/10);
        }
        bool down;
        public bool isClicked;

        public void Update(TouchCollection tc, GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            rectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            foreach (TouchLocation tl in tc)
            {
                Rectangle mouseRectangle = new Rectangle((int)tl.Position.X , (int)tl.Position.Y, 1, 1);
                if (mouseRectangle.Intersects(rectangle))
                {
                    if ((!down)||(_timer>0.5f))
                    {
                        _timer = 0;
                        down = true;
                        isClicked = true;
                    }
                    //if (colour.A == 255)
                    //    down = false;
                    //if (colour.A == 0)
                    //    down = true;                
                }
                else
                {
                    down = false;
                }
            }
        }
        public void setPosition(Vector2 newPosition)
        {
            position = newPosition;
        }
        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(texture,rectangle,colour);
        }
        }
    }
