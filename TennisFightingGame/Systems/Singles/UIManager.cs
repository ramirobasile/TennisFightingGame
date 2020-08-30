using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TennisFightingGame.Singles
{
    /* Handles UI for all players in a Match. */

    public class UIManager
    {
        private readonly Match match;

		private readonly UI.Label[] nameLabels = new UI.Label[2];
		private readonly UI.Bar[] staminaBars = new UI.Bar[2];
		private UI.Label scoreLabels;
		private UI.Label[] servingLabels = new UI.Label[2];

        public UIManager(Match match)
        {
            this.match = match;

			scoreLabels = new UI.Label("", new Point(0, 37), Assets.TitleFont, center: true, shadow: true, blinkSpeed: 60);

			// HACK Make this not hardcoded
			staminaBars[0] = new UI.Bar(Assets.PlaceholderTexture, Assets.PlaceholderTexture, 
				new Rectangle(10, 40, 400, 20), -1);
            staminaBars[1] = new UI.Bar(Assets.PlaceholderTexture, Assets.PlaceholderTexture, 
            	new Rectangle(Game1.Viewport.Width - 400 - 10, 40, 400, 20));

			nameLabels[0] = new UI.Label(match.players[0].name, 
				new Point(staminaBars[0].rect.X, 10), Assets.EmphasisFont, shadow: true);
			nameLabels[1] = new UI.Label(match.players[1].name,
				new Point(staminaBars[1].rect.Right - (int)Assets.EmphasisFont.MeasureString(match.players[1].name).X, 10), 
				Assets.EmphasisFont, shadow: true);

			servingLabels[0] = new UI.Label("P1's service",
				new Point(staminaBars[0].rect.X, 70), Assets.EmphasisFont, blinkSpeed: 8, shadow: true);
			servingLabels[1] = new UI.Label("P2's service",
				new Point(staminaBars[1].rect.Right - (int)Assets.EmphasisFont.MeasureString("P2's service").X, 70),
				Assets.EmphasisFont, blinkSpeed: 8, shadow: true);

			// Blink on score
			match.matchManager.PointScored += (_, __) => { scoreLabels.blink = 1; };
        }

		public void Update()
		{
			scoreLabels.Update();
			foreach (UI.Label label in servingLabels)
			{
				label.Update();
			}
			foreach (UI.Label label in nameLabels)
			{
				label.Update();
			}
		}

        public void Draw(SpriteBatch spriteBatch)
        {
            // UI is always on its own layer
            spriteBatch.Begin();

			foreach (UI.Label label in nameLabels)
			{
				label.Draw(spriteBatch);
			}

			scoreLabels.text = string.Format("{0} - {1}", match.points[0], match.points[1]);
			scoreLabels.Draw(spriteBatch);

			// Blinking serving label
			for (int i = 0; i < match.players.Length; i++)
			{
				if (match.players[i].state.serving)
				{
					servingLabels[i].Draw(spriteBatch);
					if (servingLabels[i].blink < 0)
					{
						servingLabels[i].blink = 100;
					}
				}
			}

			for (int i = 0; i < staminaBars.Length; i++)
            {
                staminaBars[i].Draw(spriteBatch, match.players[i].stamina / Player.MaxStamina);
            }

            spriteBatch.End();
        }
    }
}