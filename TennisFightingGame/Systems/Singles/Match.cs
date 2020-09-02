using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TennisFightingGame.Singles
{
	public class Match : TennisFightingGame.Match
	{
		public readonly MatchManager matchManager;
		public Transition transition;
		
		private readonly Pause pause;
		private readonly UIManager uiManager;

        private bool paused;
        public bool transitioning;
        public int[] points = { 0, 0 };
        public float time;

        public Match(Character[] characters, Court court, int firstTo)
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
            matchManager = new MatchManager(this, firstTo);
            camera = new Camera(this);
            uiManager = new UIManager(this);

            matchManager.MatchEnded += MatchEnd;
			players[0].input.Pressed += action => Pause(action, PlayerIndex.One);
			players[1].input.Pressed += action => Pause(action, PlayerIndex.Two);
			pause.Resumed += () => paused = false;
            pause.Quitted += MatchQuit;
			pause.ResettedPlayerPositions += () =>
			{
				foreach (Player player in players)
				{
					player.Position = player.spawnPosition;
				}
			};
			pause.ResettedBallPosition += () =>
			{
				ball.Position = new Point((matchManager.side > 0) ? 2000 : 1300, -200);
				ball.Hit(Vector2.Zero);
			};
			
			transition.Finished += () => transitioning = false;
		}

        public override void Update()
        {
            if (paused)
            {
                pause.Update();
                return;
            }

            if (transitioning)
            {
	            transition.Update();
            }

            matchManager.Update();
            ball.Update();
            foreach (Player player in players)
            {
                player.Update();
            }
            camera.Update();
			uiManager.Update();

            if (players.Count(p => p.state.serving) == 0)
            {
                time += Game1.DeltaTime;
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

			if (Game1.ConfigFile.Boolean("Debug", "Collisionboxes"))
			{
				foreach (Wall wall in court.Geometry)
				{
					spriteBatch.Draw(Assets.PlaceholderTexture, wall.rectangle, Color.Blue * .5f);
				}

				spriteBatch.Draw(Assets.PlaceholderTexture, court.middle.rectangle, Color.Red * .25f);
			}

			spriteBatch.End();

			// UI layer
			uiManager.Draw(spriteBatch);
			
			if (transitioning)
			{
				transition.Draw(spriteBatch);
			}

		}

		public Player GetPlayerBySide(int side)
        {
            return players.First(p => p.courtSide == side);
        }

        private void Pause(Action action, PlayerIndex index)
        {
            if (action == Action.Start)
            {
                pause.master = index;
                paused = true;
            }
        }
    }
}