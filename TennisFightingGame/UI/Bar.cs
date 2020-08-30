using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TennisFightingGame.UI
{
    public struct Bar
    {
        private readonly Texture2D backgroundTexture;
        private readonly Texture2D fillTexture;
        public readonly Rectangle rect;
        private readonly int direction;

        public Bar(Texture2D backgroundTexture, Texture2D fillTexture, Rectangle rect, int direction = 1)
        {
            this.backgroundTexture = backgroundTexture;
            this.fillTexture = fillTexture;
            this.rect = rect;
            this.direction = direction;
        }

        public void Draw(SpriteBatch spriteBatch, float fill)
        {
            Rectangle fillRect;
            if (direction == 1)
            {
                fillRect = new Rectangle(rect.X, rect.Y, (int) (rect.Width * fill), rect.Height);
            }
            else
            {
                fillRect = new Rectangle(rect.X + rect.Width - (int) (rect.Width * fill), rect.Y,
                    (int) (rect.Width * fill), rect.Height);
            }

            spriteBatch.Draw(backgroundTexture, rect, Color.Gray); // colors are WIP
            spriteBatch.Draw(fillTexture, fillRect, Color.Yellow);
        }
    }
}