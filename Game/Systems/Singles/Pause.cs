using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TennisFightingGame.Singles
{
    public class Pause : Menu
	{
		private UI.Label titleLabel;
		private readonly UI.Button resumeButton;
		private readonly UI.Button quitButton;
		private readonly UI.Button resetPlayerPositionsButton;
		private readonly UI.Button resetBallPositionButton;

		public Pause()
        {
			titleLabel = new UI.Label("", new Point(0, 20), Assets.TitleFont, center: true);

            resumeButton = new UI.Button("Resume match", new Point(0, 80), Assets.RegularFont, center: true);
            resumeButton.Clicked += index =>
            {
                if (Resumed != null)
                {
                    Resumed.Invoke();
                }
            };
			quitButton = new UI.Button("Quit to main menu", new Point(0, 110), Assets.RegularFont, center: true);
            quitButton.Clicked += index =>
            {
                if (Quitted != null)
                {
                    Quitted.Invoke();
                }
            };

			resetPlayerPositionsButton = new UI.Button("Reset player positions (Debug)", new Point(0, 140), Assets.EmphasisFont, center: true);
			resetPlayerPositionsButton.Clicked += index =>
			{
				if (ResettedPlayerPositions != null)
				{
					ResettedPlayerPositions.Invoke();
				}
				if (Resumed != null)
				{
					Resumed.Invoke();
				}
			};

			resetBallPositionButton = new UI.Button("Reset ball position (Debug)", new Point(0, 170), Assets.EmphasisFont, center: true);
			resetBallPositionButton.Clicked += index =>
			{
				if (ResettedBallPosition != null)
				{
					ResettedBallPosition.Invoke();
				}
				if (Resumed != null)
				{
					Resumed.Invoke();
				}
			};

			buttonSets = new UI.Button[][]
			{
				new UI.Button[] { resumeButton, quitButton }
			};

			if (TennisFightingGame.ConfigFile.Boolean("Debug", "ExtraOptions"))
			{
				buttonSets = new[]
				{
					new[] { resumeButton, quitButton, resetPlayerPositionsButton, resetBallPositionButton }
				};
			}
        }

		public delegate void ResumeEventHandler();
		public delegate void QuittedEventHandler();
		public delegate void ResetPlayerPositionsEventHandler();
		public delegate void ResetBallPositionEventHandler();

		public event ResumeEventHandler Resumed;
        public event QuittedEventHandler Quitted;
		public event ResetPlayerPositionsEventHandler ResettedPlayerPositions;
		public event ResetBallPositionEventHandler ResettedBallPosition;

		public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

			titleLabel.text = string.Format("Pause (P{0})", (int)master + 1);
			titleLabel.Draw(spriteBatch);

			spriteBatch.End();

            base.Draw(spriteBatch);
        }

        // Overrides to check if the inputs are done by the master of the pause screen, they are ignored otherwise
        protected override void Press(Actions action, PlayerIndex index)
        {
            if (index == master)
            {
                base.Press(action, index);
            }
        }

        protected override void Release(Actions action, PlayerIndex index)
        {
            if (index == master)
            {
                base.Release(action, index);
            }
        }
    }
}