using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TennisFightingGame.Training
{
    public class UIManager
    {
        private readonly SpriteFont font;
        private readonly Match match;

        private readonly UI.Bar staminaBar;
		private readonly UI.Label nameLabel;

        public UIManager(Match match)
        {
            this.match = match;

			staminaBar = new UI.Bar(Assets.PlaceholderTexture, Assets.PlaceholderTexture, 
				new Rectangle(10, 40, 400, 20));
			nameLabel = new UI.Label(match.players[0].name, new Point(staminaBar.rectangle.X, 10), 
				Assets.EmphasisFont, shadow: true);
		}

        public void Draw(SpriteBatch spriteBatch)
        {
            // UI is always on its own layer
            spriteBatch.Begin();

			nameLabel.Draw(spriteBatch);
			staminaBar.Draw(spriteBatch, match.players[0].stamina / Player.MaxStamina);

            spriteBatch.End();
        }
    }
}