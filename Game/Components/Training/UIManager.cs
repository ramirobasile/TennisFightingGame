using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TennisFightingGame.Training
{
    public class UIManager
    {
        private readonly Match match;

        private readonly UI.Bar staminaBar;
		private readonly UI.Label nameLabel;
		private readonly UI.Label bouncesLabel;
		private readonly UI.Label comboLabel;
		//private readonly UI.Label[] inputsLabels;

        public UIManager(Match match)
        {
            this.match = match;

			staminaBar = new UI.Bar(Assets.PlaceholderTexture, Assets.PlaceholderTexture, 
				new Rectangle(10, 40, 350, 20), Color.Yellow);

			nameLabel = new UI.Label(match.players[0].name, new Point(staminaBar.rectangle.X, 10), 
				Assets.EmphasisFont, shadow: true);

            bouncesLabel = new UI.Label("Bounces: 0", 
                new Point(TennisFightingGame.Viewport.Width - 10, 10), 
				Assets.RegularFont, shadow: true, textAlign: UI.TextAlign.Right); 
                
            comboLabel = new UI.Label("Combo: 0", 
                new Point(TennisFightingGame.Viewport.Width - 10, 40), 
				Assets.RegularFont, shadow: true, textAlign: UI.TextAlign.Right);          
		}

        public void Draw(SpriteBatch spriteBatch)
        {
            // This Begin ensures UI is always on its own layer
            spriteBatch.Begin();

			nameLabel.Draw(spriteBatch);

			staminaBar.Draw(spriteBatch, match.players[0].stamina / Player.MaxStamina);

            bouncesLabel.text = string.Format("Bounces : {0}", match.bounces);
            bouncesLabel.Draw(spriteBatch);
            
            comboLabel.text = string.Format("Combo: {0}", match.consecutiveHits);
            comboLabel.Draw(spriteBatch);

            if (match.showDebugInfo)
            {
                // TODO
            }

            spriteBatch.End();
        }
    }
}
