using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TennisFightingGame
{
    /* Base class for menues. */

    public abstract class Menu : Level
	{
		public PlayerIndex master;

		protected InputManager[] inputManagers;
		protected UI.Button[][] buttonSets;
		protected int selectedButton;
        protected int selectedSet;

        public Menu(PlayerIndex master = PlayerIndex.One)
        {
            this.master = master;

            inputManagers = new[]
	            {
	                new InputManager(PlayerIndex.One),
	                new InputManager(PlayerIndex.Two),
	                new InputManager(PlayerIndex.Three),
	                new InputManager(PlayerIndex.Four)
	            };

            /* Subscribe to all inputs
             * The lambda func allows me to tell which player did which input, each implementation of menu will decide
             * whether or not evey player is allowed to interact with the menu. */
            foreach (InputManager inputManager in inputManagers)
            {
                inputManager.Pressed += action => Press(action, inputManager.index);
                inputManager.Released += action => Release(action, inputManager.index);
            }
        }

        public override void Update()
        {
            foreach (InputManager inputManager in inputManagers)
            {
                inputManager.Update();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            for (int i = 0; i < buttonSets[selectedSet].Length; i++)
            {
                buttonSets[selectedSet][i].selected = i == selectedButton;
                buttonSets[selectedSet][i].Draw(spriteBatch);
            }

            spriteBatch.End();
        }

        protected virtual void Press(Action action, PlayerIndex index)
        {
			// TODO Selectable
            if (action == Action.Up)
            {
                selectedButton = Helpers.Wrap(selectedButton - 1, 0, buttonSets[selectedSet].Length - 1);
                Helpers.PlaySFX(Assets.MenuMoveSound); // HACK Some menu derivative might not want this
            }
            if (action == Action.Down)
            {
                selectedButton = Helpers.Wrap(selectedButton + 1, 0, buttonSets[selectedSet].Length - 1);
                Helpers.PlaySFX(Assets.MenuMoveSound); // HACK Some menu derivative might not want this
            }
        }

        protected virtual void Release(Action action, PlayerIndex index)
        {
            // Select button is OnRelease so the input is not picked up after a pause
            if (action == Action.Attack1)
            {
                buttonSets[selectedSet][selectedButton].Click(index);
                Helpers.PlaySFX(Assets.MenuEnterSound); // HACK Some menu derivative might not want this
            }
        }
    }
}