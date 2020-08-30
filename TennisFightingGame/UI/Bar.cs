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

        public Bar(Texture2D backgroundTexture, Texture2D fillTexture, Rectangle rectangle, int direction = 1)
        {
            this.backgroundTexture = backgroundTexture;
            this.fillTexture = fillTexture;
            this.rectangle = rectangle;
            this.direction = direction;
        }

        public void Draw(SpriteBatch spriteBatch, float fill)
        {
            Rectangle fillRect;
            if (direction == 1)
            {
                fillRect = new Rectangle(rectangle.X, rectangle.Y, (int) (rectangle.Width * fill), rectangle.Height);
            }
            else
            {
                fillRect = new Rectangle(rectangle.X + rectangle.Width - (int) (rectangle.Width * fill), rectangle.Y,
                    (int) (rectangle.Width * fill), rectangle.Height);
            }

            spriteBatch.Draw(backgroundTexture, rectangle, Color.Gray); // colors are WIP
            spriteBatch.Draw(fillTexture, fillRect, Color.Yellow);
        }
    }
}