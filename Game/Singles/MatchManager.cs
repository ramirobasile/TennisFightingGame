using System;
using Microsoft.Xna.Framework;

namespace TennisFightingGame.Singles
{
    /* Game rules, scoring and serving. */

    public class MatchManager
    {
        private const float StaminaOnChangeoverScalar = 3;
        private const float StaminaOnGameScalar = 1.5f;

        private readonly Match match;
        private readonly int bestOf;
        private readonly int minGames;
        private readonly int gamesDifference;

        public int bounces;
        public int consecutiveHits; // combo
        public int ballSide = -1; // the side of the court the ball currently is in

        public Player server;
        private readonly Player firstServer;

        public MatchManager(Match match, int bestOf = 1, int minGames = 6, 
            int gamesDifference = 2)
        {
            this.match = match;
            this.bestOf = bestOf;
            this.minGames = minGames;
            this.gamesDifference = gamesDifference;

            PassedNet += PassNet;
            Scored += Score;
            PointEnded += EndPoint;
            GameEnded += EndGame;
            SetEnded += EndSet;
            MatchEnded += EndMatch;
            match.ball.Bounced += Bounce;
            match.ball.Hitted += Hit;
            foreach (Player player in match.players)
            {
                player.moveset.Served += Serve;
            }
            
            server = match.players[TennisFightingGame.Random.Next(0, match.players.Length)];
            firstServer = server;
            server.state.serving = true;
        }

        private bool IsChangeover { get { return Game % 2 == 0; } }

        public int Game
        {
            get
            {
                int game = 0;
                foreach (Player player in match.players)
                {
                    game += player.games;
                }
                return game;
            }
        }

        public int Set
        {
            get
            {
                int set = 0;
                foreach (Player player in match.players)
                {
                    set += player.sets;
                }
                return set;
            }
        }

		public delegate void CrossingEventHandler(int side);
		public delegate void PassedNetEventHandler(int side);
		public delegate void ScoredEventHandler(Player scorer, Player scored);
		public delegate void PointEndEventHandler(Player scorer, Player scored);
		public delegate void GameEndEventHandler(Player scorer, Player scored);
		public delegate void SetEndEventHandler(Player scorer, Player scored);
		public delegate void MatchEndEventHandler(Player scorer, Player scored);

		public event ScoredEventHandler Scored;
		public event PointEndEventHandler PointEnded;
		public event GameEndEventHandler GameEnded;
		public event SetEndEventHandler SetEnded;
        public event MatchEndEventHandler MatchEnded;
        public event PassedNetEventHandler PassedNet;
        public event CrossingEventHandler Crossing;

        public void Update()
        {
            // Ball passed to the right side
            if (match.ball.lastRectangle.X < match.court.middle.Center.X &&
                match.ball.rectangle.X >= match.court.middle.Center.X)
            {
                if (PassedNet != null)
                {
                    PassedNet.Invoke(1);
                }
            }

            // Ball passed to the left side
            if (match.ball.lastRectangle.X > match.court.middle.Center.X &&
                match.ball.rectangle.X <= match.court.middle.Center.X)
            {
                if (PassedNet != null)
                {
                    PassedNet.Invoke(-1);
                }
            }
        }

        private void Bounce()
        {
            if (!match.inPlay)
            {
                return;
            }

            bounces++;

            if (bounces > 1)
            {
                if (Scored != null)
                {
                    Scored.Invoke(match.GetPlayerBySide(-ballSide), match.GetPlayerBySide(ballSide));
                }
            }
        }

        private void Hit()
		{
            // Set defender to the other player is the ball is projected to pass the net
            float middleDistance = MathHelper.Distance( match.GetPlayerBySide(ballSide).Position.X, 
                                                        match.court.middle.Center.X);
            float landingDistance = MathHelper.Distance(match.GetPlayerBySide(ballSide).Position.X,
                                                        match.ball.LandingPoint(match.court.net.rectangle.Top).X);

            if (landingDistance > middleDistance)
            {
                if (Crossing != null)
                {
                    Crossing.Invoke(ballSide);
                }
            }
            
            // Don't register hits and bounces if not inPlay but allow camera to move correctly
            if (!match.inPlay)
            {
                return;
            }

			bounces = 1;

			consecutiveHits++;

			if (consecutiveHits > 3)
			{
				if (Scored != null)
				{
					Scored.Invoke(match.GetPlayerBySide(-ballSide), match.GetPlayerBySide(ballSide));
				}
			}
		}

        private void PassNet(int newSide)
        {
            ballSide = newSide;

            if (!match.inPlay)
            {
                return;
            }
            
			if (consecutiveHits == 0)
			{
				if (Scored != null)
				{
					Scored.Invoke(match.GetPlayerBySide(newSide), match.GetPlayerBySide(ballSide));
				}
			}

            bounces = 0;
            consecutiveHits = 0;
        }

        private void Score(Player scorer, Player scored)
        {
            match.inPlay = false;

            // Instantiate transition so following events can change stuff
            // about it before it being started
            match.transition = match.uiManager.GetTransition(scorer, scored);
            
            /* The order here is very important, because we're checking if 
             * we're on game/set/match point and we want to do those things 
             * before adding sets/games/points respectively */
            if (IsOnMatchPoint(scorer))
            {
                if (MatchEnded != null)
                {
                    MatchEnded.Invoke(scorer, scored);
                }
            } 
            else if (IsOnSetPoint(scorer))
            {
                if (SetEnded != null)
                {
                    SetEnded.Invoke(scorer, scored);
                }
            }
            else if (IsOnGamePoint(scorer))
            {
                if (GameEnded != null)
                {
                    GameEnded.Invoke(scorer, scored);
                }
            }
            else if (PointEnded != null)
            {
                PointEnded.Invoke(scorer, scored);
            }

            match.transition.HalfFinished += PointSetup;
            // Service should be corrected, when needed, by events invoked before
            match.transition.Finished += () => server.state.serving = true;
            match.transition.Start();
        }

        private void EndPoint(Player scorer, Player scored)
        {
            scorer.points++;

            // Recover a stamina based on recovery and endurance
            foreach (Player player in match.players)
            {
                    match.transition.HalfFinished += () => 
                    {
                        player.AddStamina(player.stats.staminaRecovery * (player.endurance / Player.MaxEndurance));
                    };
            }
        }

        private void EndGame(Player scorer, Player scored)
        {
            scorer.games++;
            
            foreach (Player player in match.players)
            {
                // More stamina than usual is recovered between games. 
                // Even more in changeovers (odd games)
                if (IsChangeover)
                {
                    match.transition.HalfFinished += () => 
                    {
                        player.AddStamina(player.stats.staminaRecovery * 
                            StaminaOnChangeoverScalar * (player.endurance / Player.MaxEndurance));
                    };
                }
                else
                {
                    match.transition.HalfFinished += () => 
                    {
                        player.AddStamina(player.stats.staminaRecovery * StaminaOnGameScalar * 
                            (player.endurance / Player.MaxEndurance));
                    };
                }

                player.points = 0;
            }

            // Service exchange every game
            server = match.Opponent(server);
        }

        private void EndSet(Player scorer, Player scored)
        {
            scorer.sets++;
            
            foreach (Player player in match.players)
            {
                match.transition.HalfFinished += () => { player.AddStamina(Player.MaxStamina); };
                match.transition.HalfFinished += () =>  { player.AddEndurance(Player.MaxEndurance); };
                player.points = 0;
                player.games = 0;
            }

            // Service exchange every set
            server = match.Opponent(server);
        }

        private void EndMatch(Player scorer, Player scored)
        {
            match.transition.HalfFinished += () => match.End(scorer);
        }

        private void PointSetup()
        {
            match.ball.Position = new Point(match.ball.Position.X, 3000);
            match.ball.velocity = Vector2.Zero;
			match.ball.gravity = Ball.DefaultGravity;
            
            // Reset positions
            foreach (Player player in match.players)
            {
                player.Position = player.spawnPosition;
                player.velocity = Vector2.Zero;
                player.moveset.CancelCurrentAttack();
                player.direction = -player.courtSide;
            }
        }

        private void Serve(Player player)
        {
			match.ball.velocity = Vector2.Zero;
			match.ball.Position = player.rectangle.Center;
			bounces = 1;
			consecutiveHits = 0;
            player.state.serving = false;
            match.inPlay = true;
        }

        // A player is on game point when they have over 3 points and at least 
        // one point on the opponent
        public bool IsOnGamePoint(Player player)
        {
            if (player.points >= 3 && player.points > match.Opponent(player).points)
            {
                return true;
            }

            return false;
        }
        
        // A player is on game point when they have over 5 games and at least 
        // one game on the opponent
        public bool IsOnSetPoint(Player player)
        {
            if (player.games >= 5 && player.games - match.Opponent(player).games >= 1 &&
                IsOnGamePoint(player))
            {
                return true;
            }

            return false;
        }
        
        public bool IsOnMatchPoint(Player player)
        {
            if (player.sets == bestOf / 2 && IsOnSetPoint(player))
            {
                return true;
            }

            return false;
        }
    }
}
