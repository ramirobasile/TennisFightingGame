using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TennisFightingGame.UI
{
    public class Button
    {
		public Label label;
		public bool selectable;

		public bool selected;

        public Button(Label label, bool selectable = true)
        {
            this.label = label;
			this.selectable = selectable;
		}

        // Legacy
		public Button(string text, Point position, SpriteFont font, bool center = false, bool selectable = true)
		{
            label = new Label(text, position, font, center: center);
            this.selectable = selectable;
		}

        public delegate void ClickedEventHandler(PlayerIndex index);

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
            label.color = Color.White;

            if (selectable && selected)
            {
                label.color = Color.Yellow;
            }

			label.Draw(spriteBatch);
        }
    }
}