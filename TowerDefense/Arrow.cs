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

namespace Arrow
{
    class Arrow
    {
        public Texture2D ArrowTexture;
        public Vector2 ArrowPosition;
        public Vector2 ArrowVelocity;
        public Vector2 ArrowOriginalPosition;
        public float ArrowRotation;
        public bool isVisible;

        public Arrow(Texture2D newTexture2D)
        {
            ArrowTexture = newTexture2D;
            isVisible = false;
        }
        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(ArrowTexture, ArrowPosition, null, Color.White, ArrowRotation, Vector2.Zero, 1f, SpriteEffects.None, 0);
        }  
    }
}