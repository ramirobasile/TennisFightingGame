using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TennisFightingGame.UI
{
    public struct Bar
    {
        private const float FillSpeed = 3;

        private readonly Texture2D backgroundTexture;
        private readonly Texture2D fillTexture;
        public readonly Rectangle rectangle;
        private readonly int direction;
        private readonly Color color;
        
        private Rectangle lagFillRectangle;
        private Rectangle realFillRectangle;

        public Bar(Texture2D backgroundTexture, Texture2D fillTexture,
        	Rectangle rectangle, Color color, int direction = 1)
        {
            this.backgroundTexture = backgroundTexture;
            this.fillTexture = fillTexture;
            this.rectangle = rectangle;
            this.direction = direction;
            this.color = color;

            lagFillRectangle = new Rectangle();
            realFillRectangle = new Rectangle();
        }

        public void Draw(SpriteBatch spriteBatch, float newFill)
        {
            int realFill = (int)(rectangle.Width * newFill);
                
            int lagFill = (int)MathHelper.Lerp(
                lagFillRectangle.Width, 
                rectangle.Width * newFill, 
                TennisFightingGame.DeltaTime * FillSpeed);

            if (direction == 1)
            {
                realFillRectangle = new Rectangle(rectangle.X, rectangle.Y, realFill, rectangle.Height);
                lagFillRectangle = new Rectangle(rectangle.X, rectangle.Y, lagFill, rectangle.Height);
            }
            else
            {
                realFillRectangle = new Rectangle(rectangle.X + rectangle.Width - realFill, rectangle.Y,
                    realFill, rectangle.Height);
                lagFillRectangle = new Rectangle(rectangle.X + rectangle.Width - lagFill, rectangle.Y,
                    lagFill, rectangle.Height);
            }

            spriteBatch.Draw(backgroundTexture, rectangle, Color.Gray); // colors are WIP
            spriteBatch.Draw(fillTexture, lagFillRectangle, Color.Red);
            spriteBatch.Draw(fillTexture, realFillRectangle, color);
        }
    }
}
