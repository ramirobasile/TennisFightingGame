using System;
using Microsoft.Xna.Framework;

namespace TennisFightingGame.Singles
{
    /* Game rules, scoring and serving. */

    public class MatchManager
    {
        private readonly int firstTo; //first to reach wins
        private readonly Match match;

        private readonly int staminaPerRound = 15;
        public int bounces;
        public int consecutiveHits; // combo
        public int side = -1; // the side of the court the ball currently is in

        public Player starting;

        public MatchManager(Match match, int firstTo)
        {
            this.match = match;
            this.firstTo = firstTo;

            PointScored += PointScore;
            PassedNet += PassNet;
            match.ball.Bounced += Bounce;
            match.ball.Hitted += Hit;
            foreach (Player player in match.players)
            {
                player.moveset.Served += Serve;
            }
            
            // Random first serve
            Random random = new Random();
            starting = match.players[random.Next(0, 2)];
            starting.state.serving = true;
            match.inPlay = true;
        }

		public delegate void CrossingEventHandler(int side);
		public delegate void MatchEndEventHandler();
		public delegate void PassedNetEventHandler(int side);
		public delegate void PointScoredEventHandler(Player scorer, Player scored);

		public event PointScoredEventHandler PointScored;
        public event MatchEndEventHandler MatchEnded;
        public event PassedNetEventHandler PassedNet;
        public event CrossingEventHandler Crossing;

        public void Update()
        {
            // Ball passed to the right side
            if (match.ball.lastRectangle.X < match.court.middle.rectangle.Center.X &&
                match.ball.rectangle.X >= match.court.middle.rectangle.Center.X &&
                match.inPlay)
            {
                if (PassedNet != null)
                {
                    PassedNet.Invoke(1);
                }
            }

            // Ball passed to the left side
            if (match.ball.lastRectangle.X > match.court.middle.rectangle.Center.X &&
                match.ball.rectangle.X <= match.court.middle.rectangle.Center.X &&
                match.inPlay)
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
                if (PointScored != null)
                {
                    PointScored.Invoke(match.GetPlayerBySide(-side), match.GetPlayerBySide(side));
                }
            }
        }

        private void Hit()
		{
            if (!match.inPlay)
            {
                return;
            }
            
			bounces = 1;

            // Set defender to the other player is the ball is projected to pass the net
            float middleDistance = MathHelper.Distance(match.GetPlayerBySide(side).Position.X, match.court.middle.rectangle.Center.X);
            float landingDistance = MathHelper.Distance(match.GetPlayerBySide(side).Position.X,
                match.ball.LandingPoint(match.court.net.rectangle.Top).X);

            if (landingDistance > middleDistance)
            {
                if (Crossing != null)
                {
                    Crossing.Invoke(side);
                }
            }

			consecutiveHits++;
			if (consecutiveHits > 3)
			{
				if (PointScored != null)
				{
					PointScored.Invoke(match.GetPlayerBySide(-side), match.GetPlayerBySide(side));
				}
			}
		}

        private void PassNet(int newSide)
        {
            if (!match.inPlay)
            {
                return;
            }
            
			if (consecutiveHits == 0)
			{
				if (PointScored != null)
				{
					PointScored.Invoke(match.GetPlayerBySide(newSide), match.GetPlayerBySide(side));
				}
			}

            bounces = 0;
            consecutiveHits = 0;
            side = newSide;
        }

        private void PointScore(Player scorer, Player scored)
        {
            match.inPlay = false;
            
            // Add points
            match.points[((int) scorer.index + 2) % 2]++;

            // Check if a player has won
            for (int i = 0; i < match.points.Length; i++)
            {
                if (match.points[i] >= firstTo)
                {
                    if (MatchEnded != null)
                    {
                        MatchEnded.Invoke();
                    }
                }
            }
            
            match.transitioning = true;
            match.transition = new Transition(1);
            match.transition.HalfFinished += RoundEnd;
            match.transition.Finished += () => RoundStart(scorer, scored);
        }

        private void RoundEnd()
        {
            match.ball.Position = new Point(0, 3000);
            
            // Reset positions
            foreach (Player player in match.players)
            {
                player.AddStamina(staminaPerRound);
                player.Position = player.spawnPosition;
                player.velocity = Vector2.Zero;
                player.moveset.CancelCurrentAttack();
            }
        }
        
        private void RoundStart(Player scorer, Player scored)
        {
            scored.state.serving = true;
        }

        private void Serve(Player player)
        {
            match.inPlay = true;
            match.ball.velocity = Vector2.Zero;
			match.ball.Position = player.rectangle.Center;
			bounces = 1;
			consecutiveHits = 0;
            player.state.serving = false;
            //match.ball.Hit(-Vector2.UnitY * 1400);
        }
    }
}