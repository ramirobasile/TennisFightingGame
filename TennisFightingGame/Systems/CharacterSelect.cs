using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TennisFightingGame
{
    public class CharacterSelect : Menu
    {
		private readonly UI.Label titleLabel;
		private UI.Label readyLabel;
		private readonly UI.Button returnButton;
        private readonly int[] selectedButtons;

		private bool[] readiedPlayers;
		private readonly Player.Character[] selectedCharacters;

        public CharacterSelect(int players)
        {
            selectedCharacters = new Player.Character[players];
            selectedButtons = new int[players];
            readiedPlayers = new bool[players];

			titleLabel = new UI.Label("Character Select", new Point(0, 20), Assets.TitleFont, center: true);

			returnButton = new UI.Button("Return to main menu", new Point(0, 80), Assets.RegularFont, center: true);
            returnButton.Clicked += index =>
            {
                if (Returned != null)
                {
                    Returned.Invoke();
                }
            };

			// First button is return, following buttons for each character
			UI.Button[] buttons = new UI.Button[Assets.Characters.Length + 1];
			buttons[0] = returnButton;
            for (int i = 1; i < Assets.Characters.Length + 1; i++)
            {
                buttons[i] = new UI.Button(Assets.Characters[i - 1].name, 
                	new Point(0, 110 + Assets.RegularFont.Height() * (i - 1)), 
					Assets.RegularFont, center: true);

                buttons[i].Clicked += SelectCharacter;
            }

			readyLabel = new UI.Label("", new Point(0, buttons[buttons.Length - 1].position.Y + 30), Assets.RegularFont, center: true);

			buttonSets = new UI.Button[][] { buttons };
        }

		public delegate void AllCharactersSelectedEventHandler(Player.Character[] selectedCharacters);
		public delegate void ReturnedEventHandler();

		public event AllCharactersSelectedEventHandler AllCharactersSelected;
        public event ReturnedEventHandler Returned;

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

			titleLabel.Draw(spriteBatch);

            if (readiedPlayers.All(p => p))
            {
				readyLabel.text = "All players ready!";
            }
            else
			{
				readyLabel.text = "Not all players ready yet...";
			}

			readyLabel.Draw(spriteBatch);

            // Button drawing overriden so multiple buttons can be drawn as "selected" at once
            for (int i = 0; i < buttonSets[selectedSet].Length; i++)
            {
                buttonSets[selectedSet][i].selected = selectedButtons.Any(b => b == i);
                buttonSets[selectedSet][i].Draw(spriteBatch);
            }

            spriteBatch.End();
        }

        // Input handling overriden because multiple buttons can be selected at once on the CSS
        protected override void Press(Action action, PlayerIndex index)
        {
            // We don't want inputs from players that are not selecting characters or players that are already ready
            if ((int) index >= selectedButtons.Length || readiedPlayers[(int) index])
            {
                return;
            }

            if (action == Action.Up)
            {
                selectedButtons[(int) index] = MathHelper.Clamp(selectedButtons[(int) index] - 1, 0,
                    buttonSets[selectedSet].Length - 1);
            }

            if (action == Action.Down)
            {
                selectedButtons[(int) index] = MathHelper.Clamp(selectedButtons[(int) index] + 1, 0,
                    buttonSets[selectedSet].Length - 1);
            }
        }

        protected override void Release(Action action, PlayerIndex index)
        {
            if ((int) index >= selectedButtons.Length)
            {
                return;
            }

            if (action == Action.Attack1)
            {
                buttonSets[selectedSet][selectedButtons[(int) index]].Click(index);
            }

            if (action == Action.Start && readiedPlayers.All(p => p))
            {
                if (AllCharactersSelected != null)
                {
                    AllCharactersSelected.Invoke(selectedCharacters);
                }
            }
        }

        private void SelectCharacter(PlayerIndex index)
        {
			if (!readiedPlayers[(int)index])
			{
				selectedCharacters[(int)index] = Assets.Characters[selectedButtons[(int)index] - 1];
				readiedPlayers[(int)index] = true;
			}
			else
			{
				readiedPlayers[(int)index] = false;
			}
        }
    }
}