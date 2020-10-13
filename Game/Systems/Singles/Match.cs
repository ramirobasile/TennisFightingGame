using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TennisFightingGame.Singles
{
	public class Match : global::TennisFightingGame.Match
	{
		public readonly MatchManager manager;
		public Transition transition;
		
		private readonly Pause pause;
		public readonly UIManager uiManager;

        private bool paused;
        public float time;

        public Match(Character[] characters, Court court)
        {
			this.court = court;

            ball = new Ball(new Rectangle(0, 0, 30, 30), Assets.PlaceholderTexture, this);

            players = new Player[]
            {
                new Player(characters[0], this, PlayerIndex.One, -1, court.spawnPoints[0]),
                new Player(characters[1], this, PlayerIndex.Two, 1, court.spawnPoints[1])
            };

            pause = new Pause();
            transition = new Transition(1);
            manager = new MatchManager(this);
            camera = new Camera(this);
            uiManager = new UIManager(this);

			players[0].input.Pressed += action => Pause(action, PlayerIndex.One);
			players[1].input.Pressed += action => Pause(action, PlayerIndex.Two);
			pause.Resumed += () => paused = false;
            pause.Quitted += Quit;
			pause.ResettedPlayerPositions += () =>
			{
				foreach (Player player in players)
				{
					player.Position = player.spawnPosition;
				}
			};
			pause.ResettedBallPosition += () =>
			{
				ball.Position = court.spawnPoints[((manager.ballSide == 1) ? 1 : 0)];
				ball.velocity = Vector2.Zero;
				ball.gravity = Ball.DefaultGravity;
			};
		}

        public override void Update()
        {
            if (paused)
            {
                pause.Update();
                return;
            }

            if (transition.transitioning)
            {
	            transition.Update();
            }

            manager.Update();
            ball.Update();
            foreach (Player player in players)
            {
                player.Update();
            }
            camera.Update();
			uiManager.Update();

            if (players.Count(p => p.state.serving) == 0)
            {
                time += TennisFightingGame.DeltaTime;
            }
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

			if (TennisFightingGame.ConfigFile.Boolean("Debug", "Collisionboxes"))
			{
				foreach (Wall wall in court.Geometry)
				{
					spriteBatch.Draw(Assets.PlaceholderTexture, wall.rectangle, Color.Blue * 0.5f);
				}

				spriteBatch.Draw(Assets.PlaceholderTexture, court.middle, Color.Red * 0.25f);
			}

			spriteBatch.End();

			// UI layer
			uiManager.Draw(spriteBatch);
			
			if (transition.transitioning)
			{
				transition.Draw(spriteBatch);
			}

		}

		public Player GetPlayerBySide(int side)
        {
            return players.First(p => p.courtSide == side);
        }
		
		public Player Opponent(Player player)
        {
            return players.First(p => p != player);
        }

        private void Pause(Actions action, PlayerIndex index)
        {
            if (action == Actions.Start)
            {
                pause.master = index;
                paused = true;
            }
		}
    }
}