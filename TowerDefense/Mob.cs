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
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefense
{
    class Mob : AnimatedSprite
    {
        public bool isDead;
        public int speed;
        public int speed_y;
        public int HP;
        public Mob(Vector2 position) : base(position)
        {
            FramesPerSeconds = 10;
            isDead = false; 
        }
        public void LoadContent(ContentManager content)
        {
            sTexture = content.Load<Texture2D>("Mob");
            AddAnimation(12);
        }
        public override void Update(GameTime gametime)
        {
            float deltaTime = (float)gametime.ElapsedGameTime.TotalSeconds;
            
                sDirection = new Vector2(speed, speed_y);
                sPosition += sDirection * deltaTime;
            
            base.Update(gametime);
                
        }     
    }
}