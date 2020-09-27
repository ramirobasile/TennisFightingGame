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
		private readonly UI.Bar[] enduranceBars = new UI.Bar[2];
		private UI.Label pointsLabel;
		private UI.Label gamesLabel;
		private UI.Label setsLabel;
		private UI.Label[] servingLabels = new UI.Label[2];

		private string[] points = { "0", "15", "30", "40", "60" };

        public UIManager(Match match)
        {
            this.match = match;

			// HACK Make this not hardcoded
			pointsLabel = new UI.Label(
				"",
				new Point(0, 37),
				Assets.TitleFont,
				center: true,
				shadow: true,
				blinkSpeed: 60);

			gamesLabel = new UI.Label(
				"",
				new Point(0, 67),
				Assets.RegularFont,
				center: true,
				shadow: true,
				blinkSpeed: 60);

			setsLabel = new UI.Label(
				"",
				new Point(0, 87),
				Assets.RegularFont,
				center: true,
				shadow: true,
				blinkSpeed: 60);

			staminaBars[0] = new UI.Bar(
				Assets.PlaceholderTexture,
				Assets.PlaceholderTexture,
				new Rectangle(10, 40, 350, 20),
				Color.Yellow,
				-1);

            staminaBars[1] = new UI.Bar(
            	Assets.PlaceholderTexture,
            	Assets.PlaceholderTexture,
            	new Rectangle(TennisFightingGame.Viewport.Width - 350 - 10, 40, 350, 20),
            	Color.Yellow);

			enduranceBars[0] = new UI.Bar(
				Assets.PlaceholderTexture,
				Assets.PlaceholderTexture,
				new Rectangle(10, 65, 350, 5),
				Color.Lavender,
				-1);

            enduranceBars[1] = new UI.Bar(
            	Assets.PlaceholderTexture,
            	Assets.PlaceholderTexture,
            	new Rectangle(TennisFightingGame.Viewport.Width - 350 - 10, 65, 350, 5),
            	Color.Lavender);

			nameLabels[0] = new UI.Label(
				match.players[0].name,
				new Point(staminaBars[0].rectangle.X, 10),
				Assets.EmphasisFont,
				shadow: true);

			int nameLength = (int)Assets.EmphasisFont.MeasureString(match.players[1].name).X;
			nameLabels[1] = new UI.Label(
				match.players[1].name,
				new Point(staminaBars[1].rectangle.Right - nameLength, 10),
				Assets.EmphasisFont,
				shadow: true);

			servingLabels[0] = new UI.Label(
				"P1's service",
				new Point(staminaBars[0].rectangle.X, 80),
				Assets.EmphasisFont,
				blinkSpeed: 8,
				shadow: true);

			int servingLength = (int)Assets.EmphasisFont.MeasureString("P2's service").X;
			servingLabels[1] = new UI.Label(
				"P2's service",
				new Point(staminaBars[1].rectangle.Right - servingLength, 80),
				Assets.EmphasisFont,
				blinkSpeed: 8,
				shadow: true);

			// Blink on score
			match.manager.PointEnded += (_, __) => { pointsLabel.blink = 1; };
        }

		public void Update()
		{
			pointsLabel.Update();
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

			// TODO Put this whole thing in a method that is subscribed to MatchManager point score
			// events

			if (match.players[0].points <= 4 && match.players[1].points <= 4)
			{
				pointsLabel.text = string.Format("{0} - {1}", points[match.players[0].points], 
					points[match.players[1].points]);
			}

			foreach (Player player in match.players)
			{
				if (player.points == match.Opponent(player).points && player.points >= 3)
				{
					pointsLabel.text = "Deuce";
					break;
				} 
				if (player.points > match.Opponent(player).points && player.points > 3)
				{
					pointsLabel.text = string.Format("Adv. {0}", player.name);
					break;
				}
			}

			pointsLabel.Draw(spriteBatch);
			
			gamesLabel.text = string.Format("{0} - {1}", match.players[0].games, match.players[1].games);
			
			gamesLabel.Draw(spriteBatch);

			setsLabel.text = string.Format("{0} - {1}", match.players[0].sets,  match.players[1].sets);
			
			setsLabel.Draw(spriteBatch);
			
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

			for (int i = 0; i < enduranceBars.Length; i++)
            {
                enduranceBars[i].Draw(spriteBatch, match.players[i].endurance / Player.MaxEndurance);
            }

            spriteBatch.End();
        }
    }
}
