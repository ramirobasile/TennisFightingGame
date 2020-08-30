using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TennisFightingGame.UI
{
    public class Button
    {
        public delegate void ClickedEventHandler(PlayerIndex index);

        private readonly SpriteFont font;
        public readonly Point position;
		private readonly string text;
		private readonly bool center;
		public bool selectable;

		public bool selected;

        public Button(string text, Point position, SpriteFont font, bool center = false, 
        	bool selectable = true)
        {
            this.text = text;
            this.position = position;
            this.font = font;
			this.center = center;
			this.selectable = selectable;
		}

		public event ClickedEventHandler Clicked;

        public void Click(PlayerIndex index)
        {
            if (Clicked != null)
            {
                Clicked.Invoke(index);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Color color = Color.White;

            if (selected)
            {
                color = Color.Yellow;
            }

			if (center)
			{
				spriteBatch.DrawString(font, text, Helpers.CenterTextHorizontally(text, position.Y, font).ToVector2(), color);
			}
			else
			{
				spriteBatch.DrawString(font, text, position.ToVector2(), color);
			}
        }
    }
}