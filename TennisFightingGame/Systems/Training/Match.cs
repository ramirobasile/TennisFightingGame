using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TennisFightingGame.Training
{
    /* Training doesn't need Game Manager... */

    public class Match : TennisFightingGame.Match
	{
		private const int StageWidth = 3750;
		private const int StageHeight = 1000;
		private const int WallWidth = 100;
		private const int NetWidth = 40;
		private const int NetHeight = 70;

        private int bounces;
        private bool paused;
		private readonly Pause pause;
		private readonly UIManager uiManager;

        public Match(Character character, Court court)
        {
			this.court = court;

			ball = new Ball(new Rectangle(0, 1000, 30, 30), Assets.PlaceholderTexture, this);

            players = new[]
            {
                new Player(character, this, PlayerIndex.One, 1, court.spawnPoints[0])
            };

            pause = new Pause();
            camera = new Camera(.5f);
            uiManager = new UIManager(this);

            ball.Bounced += () => bounces++;
            pause.Resumed += () => paused = false;
            pause.SelectedServe += Serve;
            pause.SelectedStamina += () => players[0].AddStamina(Player.MaxStamina);
            pause.Quitted += MatchQuit;
            players[0].input.Pressed += action => Pause(action, PlayerIndex.One);
            players[0].moveset.Served += ServeDone;

            inPlay = false;
            players[0].state.serving = true;
        }

        public override void Update()
        {
            if (paused)
            {
                pause.Update();
                return;
            }

            ball.Update();

            foreach (Player player in players)
            {
                player.Update();
            }

            camera.centre = players[0].Position.ToVector2();
            camera.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (paused)
            {
                pause.Draw(spriteBatch);
                return;
            }

			// Background layer
			spriteBatch.Begin();

			spriteBatch.Draw(court.backgroundTexture, new Rectangle(0, 0, 960, 405), Color.White);

			spriteBatch.End();

			// Camera layer
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null,
				camera.transform);

			spriteBatch.Draw(court.courtTexture, new Vector2(-1125, -380), Color.White);

			foreach (Player player in players)
			{
				player.Draw(spriteBatch);
			}

			ball.Draw(spriteBatch);

			if (Game1.ConfigFile.Boolean("Debug", "Collisionboxes"))
			{
				foreach (Wall wall in court.Geometry)
				{
					spriteBatch.Draw(Assets.PlaceholderTexture, wall.rectangle, Color.Blue * .5f);
				}
			}

			spriteBatch.End();

			// UI layer
			uiManager.Draw(spriteBatch);
		}

        private void Serve()
        {
            players[0].state.serving = true;
            inPlay = false;
        }

        private void ServeDone(Player player)
        {
			ball.velocity = Vector2.Zero;
			ball.gravity = Ball.DefaultGravity;
            ball.Position = player.Position;
            player.state.serving = false;
            ball.Position = player.rectangle.Center;
			inPlay = true;
        }

        private void Pause(Action action, PlayerIndex index)
        {
            if (action == Action.Start)
            {
                paused = true;
            }
        }
    }
}