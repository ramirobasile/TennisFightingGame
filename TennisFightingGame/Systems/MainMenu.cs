using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TennisFightingGame
{
	/// <summary>
	/// TODO A solid documentation of what this essential class does.
	/// </summary>
    public class MainMenu : Menu
    {
		private readonly UI.Label titleLabel;

		private readonly UI.Button doublesButton;
        private readonly UI.Button quitButton;
        private readonly UI.Button settingsButton;
        private readonly UI.Button singlesButton;
        private readonly UI.Button trainingButton;

        public MainMenu()
        {
			titleLabel = new UI.Label("TennisFightingGame", new Point(0, 20), Assets.TitleFont, center: true);

			singlesButton = new UI.Button("Singles", new Point(0, 80), Assets.RegularFont, center: true);
            singlesButton.Clicked += StartSinglesCharacterSelect;

            doublesButton = new UI.Button("Doubles", new Point(0, 110), Assets.RegularFont, center: true, 
            	selectable: false);
            doublesButton.Clicked += StartDoublesCharacterSelect;

            trainingButton = new UI.Button("Training", new Point(0, 140), Assets.RegularFont, center: true);
            trainingButton.Clicked += StartTrainingCharacterSelect;

            settingsButton = new UI.Button("Settings", new Point(0, 170), Assets.RegularFont, center: true);
            settingsButton.Clicked += OpenSettings;

			quitButton = new UI.Button("Quit", new Point(0, 200), Assets.RegularFont, center: true);
            quitButton.Clicked += Quit;

            buttonSets = new UI.Button[][]
            {
                new UI.Button[] { singlesButton, doublesButton, trainingButton, settingsButton, quitButton }
            };
        }

		public delegate void QuittedEventHandler();

		public event QuittedEventHandler Quitted;

        private void StartSinglesCharacterSelect(PlayerIndex _)
        {
            CharacterSelect singlesCharacterSelect = new CharacterSelect(2);
            singlesCharacterSelect.AllCharactersSelected += StartSinglesCourtSelect;
            singlesCharacterSelect.Returned += ReturnToMainMenu;
            Game1.Level = singlesCharacterSelect;
        }

		private void StartSinglesCourtSelect(Player.Character[] selectedCharacters)
		{
			CourtSelect singlesCourtSelect = new CourtSelect();
			singlesCourtSelect.CourtSelected += (selectedCourt) => { StartSinglesMatch(selectedCharacters, selectedCourt); };
			singlesCourtSelect.Returned += ReturnToMainMenu;
			Game1.Level = singlesCourtSelect;
		}

        private void StartSinglesMatch(Player.Character[] selectedCharacters, Court selectedCourt)
        {
            Singles.Match singlesMatch = new Singles.Match(selectedCharacters, selectedCourt, 30);
            singlesMatch.MatchEnded += ReturnToMainMenu;
            singlesMatch.MatchQuitted += ReturnToMainMenu;
            Game1.Level = singlesMatch;
        }

		private void StartDoublesCharacterSelect(PlayerIndex _)
        {
        }

        private void StartTrainingCharacterSelect(PlayerIndex index)
        {
            CharacterSelect trainingCharacterSelect = new CharacterSelect(1);
            trainingCharacterSelect.AllCharactersSelected += StartTrainingCourtSelect;
            trainingCharacterSelect.Returned += ReturnToMainMenu;
            Game1.Level = trainingCharacterSelect;
        }

		private void StartTrainingCourtSelect(Player.Character[] selectedCharacters)
		{
			CourtSelect trainingCourtSelect = new CourtSelect();
			trainingCourtSelect.CourtSelected += (selectedCourt) => { StartTrainingMatch(selectedCharacters, selectedCourt); };
			trainingCourtSelect.Returned += ReturnToMainMenu;
			Game1.Level = trainingCourtSelect;
		}

		private void StartTrainingMatch(Player.Character[] selectedCharacters, Court selectedCourt)
        {
            Training.Match trainingMatch = new Training.Match(selectedCharacters[0], selectedCourt);
            trainingMatch.MatchEnded += ReturnToMainMenu;
            trainingMatch.MatchQuitted += ReturnToMainMenu;
            Game1.Level = trainingMatch;
        }

        private void ReturnToMainMenu()
        {
            Game1.Level = this;
        }

		private void OpenSettings(PlayerIndex _)
		{
			switch (Environment.OSVersion.Platform)
			{
				case PlatformID.Unix:
					{
						Process.Start("xdg-open", "TennisFightingGame.ini");
						break;
					}
				case PlatformID.WinCE:
				case PlatformID.Win32NT:
				case PlatformID.Win32Windows:
					{
						Process.Start("TennisFightingGame.ini");
						break;
					}
				case PlatformID.MacOSX:
					{
						break;
					}
			}
		}

        private void Quit(PlayerIndex _)
        {
            if (Quitted != null)
            {
                Quitted.Invoke();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

			titleLabel.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(spriteBatch);
        }
    }
}