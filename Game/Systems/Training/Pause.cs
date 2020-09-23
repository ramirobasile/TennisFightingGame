using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TennisFightingGame.Training
{
    public class Pause : Menu
    {
        private readonly UI.Button resumeButton;
        private readonly UI.Button serveButton;
        private readonly UI.Button staminaButton;
        private readonly UI.Button showDebugInformationButton;
        private readonly UI.Button quitButton;

		private UI.Label titleLabel;

		public Pause()
		{
			titleLabel = new UI.Label("Pause", new Point(0, 20), Assets.TitleFont, center: true);

			resumeButton = new UI.Button("Resume match", new Point(0, 80), Assets.RegularFont, center: true);
            resumeButton.Clicked += index =>
            {
                if (Resumed != null)
                {
                    Resumed.Invoke();
                }
            };

            serveButton = new UI.Button("Serve", new Point(0, 110), Assets.RegularFont, center: true);
            serveButton.Clicked += index =>
            {
                if (SelectedServe != null)
                {
                    SelectedServe.Invoke();
                }

                if (Resumed != null)
                {
                    Resumed.Invoke();
                }
            };

            staminaButton = new UI.Button("Replenish stamina", new Point(0, 140), Assets.RegularFont, center: true);
            staminaButton.Clicked += index =>
            {
                if (SelectedStamina != null)
                {
                    SelectedStamina.Invoke();
                }

                if (Resumed != null)
                {
                    Resumed.Invoke();
                }
            };
            
            showDebugInformationButton = new UI.Button("Show debug information", new Point(0, 170), Assets.RegularFont, center: true);
            showDebugInformationButton.Clicked += index =>
            {
                if (SelectedDebugInfo != null)
                {
                    SelectedDebugInfo.Invoke();
                }

                if (Resumed != null)
                {
                    Resumed.Invoke();
                }
            };

            quitButton = new UI.Button("Quit to main menu", new Point(0, 200), Assets.RegularFont, center: true);
            quitButton.Clicked += index =>
            {
                if (Quitted != null)
                {
                    Quitted.Invoke();
                }
            };

            buttonSets = new UI.Button[][]
            {
                new UI.Button[] {resumeButton, serveButton, staminaButton, showDebugInformationButton, quitButton}
            };
        }

		public delegate void QuittedEventHandler();
		public delegate void ResumedEventHandler();
		public delegate void SelectedServeEventHandler();
		public delegate void SelectedStaminaEventHandler();
		public delegate void SelectedDebugInfoEventHandler();

		public event ResumedEventHandler Resumed;
        public event SelectedServeEventHandler SelectedServe;
        public event SelectedStaminaEventHandler SelectedStamina;
        public event SelectedDebugInfoEventHandler SelectedDebugInfo;

        public event QuittedEventHandler Quitted;

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

			titleLabel.Draw(spriteBatch);

			spriteBatch.End();

            base.Draw(spriteBatch);
        }
    }
}
