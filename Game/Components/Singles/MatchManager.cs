using System;
using Microsoft.Xna.Framework;

namespace TennisFightingGame.Singles
{
    /* Game rules, scoring and serving. */

    public class MatchManager
    {
        private readonly Match match;
        private readonly int bestOf;
        private readonly int minGames;
        private readonly int gamesDifference;

        public int bounces;
        public int consecutiveHits; // combo
        public int ballSide = -1; // the side of the court the ball currently is in

        public Player service;
        private readonly Player firstService;

        public MatchManager(Match match, int bestOf = 1, int minGames = 6, 
            int gamesDifference = 2)
        {
            this.match = match;
            this.bestOf = bestOf;
            this.minGames = minGames;
            this.gamesDifference = gamesDifference;

            PassedNet += PassNet;
            PointEnded += EndPoint;
            GameEnded += EndGame;
            SetEnded += EndSet;
            match.ball.Bounced += Bounce;
            match.ball.Hitted += Hit;
            foreach (Player player in match.players)
            {
                player.moveset.Served += Serve;
            }
            
            service = match.players[TennisFightingGame.Random.Next(0, match.players.Length)];
            firstService = service;
            service.state.serving = true;
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
		public delegate void PointEndEventHandler(Player scorer, Player scored);
		public delegate void GameEndEventHandler(Player scorer, Player scored);
		public delegate void SetEndEventHandler(Player scorer, Player scored);
		public delegate void MatchEndEventHandler();

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
                if (PointEnded != null)
                {
                    PointEnded.Invoke(match.GetPlayerBySide(-ballSide), match.GetPlayerBySide(ballSide));
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
				if (PointEnded != null)
				{
					PointEnded.Invoke(match.GetPlayerBySide(-ballSide), match.GetPlayerBySide(ballSide));
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
				if (PointEnded != null)
				{
					PointEnded.Invoke(match.GetPlayerBySide(newSide), match.GetPlayerBySide(ballSide));
				}
			}

            bounces = 0;
            consecutiveHits = 0;
        }

        private void EndPoint(Player scorer, Player scored)
        {
            match.inPlay = false;

            if (IsOnGamePoint(scorer))
            {
                if (GameEnded != null)
                {
                    GameEnded.Invoke(scorer, scored);
                }
                
                return;
            }

            scorer.points++;

            foreach (Player player in match.players)
            {
                player.AddStamina(player.stats.staminaRecovery * (player.endurance / Player.MaxEndurance));
            }

            match.transitioning = true;
            match.transition = new Transition("", 0.2f, 0.33f, 0.2f);
            match.transition.HalfFinished += PointSetup;
            match.transition.Finished += () => service.state.serving = true;
        }

        private void EndGame(Player scorer, Player scored)
        {
            match.inPlay = false;

            if (IsOnSetPoint(scorer))
            {
                if (SetEnded != null)
                {
                    SetEnded.Invoke(scorer, scored);
                }

                return;
            }

            scorer.games++;
            
            foreach (Player player in match.players)
            {
                player.AddStamina(Player.MaxStamina * (player.endurance / Player.MaxEndurance));
                player.points = 0;
            }

            // TODO Add new game transition behaviour
            match.transitioning = true;
            match.transition = new Transition("Game", 0.33f, 1.5f, 0.33f);
            match.transition.HalfFinished += PointSetup;
            
            service = match.Opponent(service);
            match.transition.Finished += () => service.state.serving = true;
        }

        private void EndSet(Player scorer, Player scored)
        {
            match.inPlay = false;

            if (IsOnMatchPoint(scorer))
            {
                match.transitioning = true;
                match.transition = new Transition(10); // cleans up previous subscriptions in the process
                match.transition.HalfFinished += () => 
                {
                    if (MatchEnded != null)
                    {
                        MatchEnded.Invoke();
                    }
                };

                return;
            }

            scorer.sets++;
            
            foreach (Player player in match.players)
            {
                player.AddStamina(Player.MaxStamina);
                player.points = 0;
                player.games = 0;
            }

            // TODO Add new game transition behaviour
            match.transitioning = true;
            match.transition = new Transition("Set", 0.5f, 4, 0.5f);
            match.transition.HalfFinished += PointSetup;
            
            if (IsChangeover)
            {
                service = firstService;
            }
            else
            {
                service = match.Opponent(firstService);
            }
            service = match.players[0];
            match.transition.Finished += () => service.state.serving = true;
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

        private bool IsOnGamePoint(Player player)
        {
            if ((player.points >= 4 && player.points - match.Opponent(player).points >= 1) ||
                (player.points >= 3 && player.points - match.Opponent(player).points >= 2))
            {
                return true;
            }

            return false;
        }
        
        private bool IsOnSetPoint(Player player)
        {
            if ((player.games >= 4 && player.games - match.Opponent(player).games >= 1) ||
                (player.games >= 3 && player.games - match.Opponent(player).games >= 2))
            {
                return true;
            }

            return false;
        }
        
        private bool IsOnMatchPoint(Player player)
        {
            if (player.sets == bestOf / 2)
            {
                return true;
            }

            return false;
        }
    }
}
