using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TennisFightingGame
{
	public class CourtSelect : Menu
	{
		private readonly UI.Label titleLabel;
		private readonly UI.Button returnButton;

		public CourtSelect()
		{
			titleLabel = new UI.Label("Court Select", new Point(0, 20), Assets.TitleFont, center: true);

			returnButton = new UI.Button("Return to main menu", new Point(0, 80), Assets.RegularFont, center: true);
			returnButton.Clicked += index =>
			{
				if (Returned != null)
				{
					Returned.Invoke();
				}
			};

			// First button is return, following buttons for each court
			UI.Button[] buttons = new UI.Button[Assets.Courts.Length + 1];
			buttons[0] = returnButton;
			for (int i = 1; i < Assets.Courts.Length + 1; i++)
			{
				buttons[i] = new UI.Button(Assets.Courts[i - 1].name,
					new Point(0, 110 + Assets.RegularFont.Height() * i),
					Assets.RegularFont, center: true);

				buttons[i].Clicked += SelectCourt;
			}

			buttonSets = new UI.Button[][] { buttons };
		}

		public delegate void CourtSelectedEventHandler(Court court);
		public delegate void ReturnedEventHandler();

		public event CourtSelectedEventHandler CourtSelected;
		public event ReturnedEventHandler Returned;

		public override void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Begin();

			titleLabel.Draw(spriteBatch);

			spriteBatch.End();

			base.Draw(spriteBatch);
		}

		private void SelectCourt(PlayerIndex _)
		{
			// Selected button is offsetted by 1 because return button goes before courts
			if (CourtSelected != null && selectedButton != 0)
			{
				CourtSelected.Invoke(Assets.Courts[selectedButton - 1]);
			}
		}
	}
}