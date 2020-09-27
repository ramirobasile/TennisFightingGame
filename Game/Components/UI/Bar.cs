using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TennisFightingGame.UI
{
    public struct Bar
    {
        private readonly Texture2D backgroundTexture;
        private readonly Texture2D fillTexture;
        public readonly Rectangle rectangle;
        private readonly int direction;
        private readonly Color color;

        public Bar(Texture2D backgroundTexture, Texture2D fillTexture,
        	Rectangle rectangle, Color color, int direction = 1)
        {
            this.backgroundTexture = backgroundTexture;
            this.fillTexture = fillTexture;
            this.rectangle = rectangle;
            this.direction = direction;
            this.color = color;
        }

        public void Draw(SpriteBatch spriteBatch, float fill)
        {
            Rectangle fillRectangle;
            if (direction == 1)
            {
                fillRectangle = new Rectangle(rectangle.X, rectangle.Y, (int) (rectangle.Width * fill), rectangle.Height);
            }
            else
            {
                fillRectangle = new Rectangle(rectangle.X + rectangle.Width - (int) (rectangle.Width * fill), rectangle.Y,
                    (int) (rectangle.Width * fill), rectangle.Height);
            }

            spriteBatch.Draw(backgroundTexture, rectangle, Color.Gray); // colors are WIP
            spriteBatch.Draw(fillTexture, fillRectangle, color);
        }
    }
}
