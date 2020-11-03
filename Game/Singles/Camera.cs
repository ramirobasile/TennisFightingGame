using System;
using Microsoft.Xna.Framework;

namespace TennisFightingGame.Singles
{
	/// <summary>
	/// Using Camera as a base, the single's camera is speciallized.
	/// By default, the camera will try to keep both players within view by doing two things:
	///  * Positioning itself between the average of their positions
	///  * Zooming out whenever a player is not fully on frame and in whenever possible
	/// All of this is done while keeping a slight margin between the players and the edge of the
	/// screen.
	/// 
	/// When the ball is hit by a player, and if the ball would land on the other side of that player's
	/// field, the camera will try give priority to keeping the defender in view.
	/// Additionaly, once the ball passes over, the camera will no longer try to keep the attacker on
	/// frame, rather it will swtich to center on the point between the ball and the defender, with a
	/// degree of margin towards the direction the ball is flying to.
	/// </summary>
    public class Camera : global::TennisFightingGame.Camera
    {
        private const float MinZoom = 0.6f;
        private const float MaxZoom = 0.75f;
        private const float MoveSpeed = 6;
		private const float MaxMoveSpeed = 10;
        private const float HitStunMoveThreshold = 0.1f;
		private const float HitStunMoveSpeed = 3;
		private const float ZoomSpeed = 5;
		private const float AverageBallVelocity = 1500;

		private readonly Match match;
		private readonly int margin;

        public Player attacker;
        public Player defender;
		private bool focusBall;

		public Camera(Match match)
        {
            this.match = match;

            margin = match.players[0].sprite.frameSize.X * 2;


			defender = match.manager.server;
            attacker = match.Opponent(match.manager.server);

			// HACK A bit too hard coded maybe...
			match.manager.server.moveset.Served += SetFocus;
			match.manager.PointEnded += (_, __) => SetFocus(match.manager.server);
			match.manager.GameEnded += (_, __) => SetFocus(match.manager.server);
			match.manager.SetEnded += (_, __) => SetFocus(match.manager.server);
            match.manager.Crossing += Crossing;
			match.manager.PassedNet += ChangeAttacker;
        }

        public override void Update()
		{
            base.Update();
			
			// Camera doesn't move when hitStun is above a certain const
			if (match.ball.hitStun > HitStunMoveThreshold)
			{
				return;
			}
			
			float x;
			float y;
			float speed;
			float focusDistance = Vector2.Distance(
				defender.rectangle.Center.ToVector2(),
				attacker.rectangle.Center.ToVector2());

            // Set zoom
            // Lerps zoom to keep both players on screen until it reaches min/max zoom
            zoom = MathHelper.Clamp(
                MathHelper.Lerp(
                    zoom,
                    TennisFightingGame.Viewport.Middle().X / zoom / focusDistance,
                    TennisFightingGame.DeltaTime * ZoomSpeed),
                MinZoom,
                MaxZoom
            );

			// Set x and y
			// Point between attacker's and defender's center
			Vector2 playersMidpoint = new Vector2((attacker.rectangle.Center.X + defender.rectangle.Center.X) / 2,
				(attacker.rectangle.Center.Y + defender.rectangle.Center.Y) / 2);

			// FIXME Weird shit happens when ball bounces off wall
			if (focusBall)
			{
				int ballDirection = Math.Sign(match.ball.velocity.X);
				float ballMargin = ballDirection * TennisFightingGame.Viewport.Width / 2;

				x = MathHelper.Clamp(
					(playersMidpoint.X + match.ball.rectangle.Center.X + ballMargin) / 2,
                    defender.rectangle.Center.X - TennisFightingGame.Viewport.Width / zoom / 2 + margin,
                    defender.rectangle.Center.X + TennisFightingGame.Viewport.Width / zoom / 2 - margin
                    );
			} 
			else
			{
				x = MathHelper.Clamp(
					playersMidpoint.X,
                    defender.rectangle.Center.X - TennisFightingGame.Viewport.Width / zoom / 2 + margin,
                    defender.rectangle.Center.X + TennisFightingGame.Viewport.Width / zoom / 2 - margin
                    );
			}

			y = -TennisFightingGame.Viewport.Height / 3 + (TennisFightingGame.Viewport.Height / 2 * zoom - TennisFightingGame.Viewport.Height / 2);

			// Set speed
			if (defender.match.ball.hitStun <= 0)
			{
				speed = MathHelper.Clamp(MoveSpeed * match.ball.velocity.X / AverageBallVelocity,
					MoveSpeed, MaxMoveSpeed);
			}
			else
			{
				speed = MathHelper.Clamp(HitStunMoveSpeed * match.ball.velocity.X / AverageBallVelocity,
					HitStunMoveSpeed, MaxMoveSpeed);
			}

            centre = Vector2.Lerp(centre, new Vector2(x, y), TennisFightingGame.DeltaTime * speed);
        }

        private void SetFocus(Player newDefender)
        {
            defender = newDefender;
            attacker = match.Opponent(defender);
			focusBall = false;
        }

		private void ChangeAttacker(int newSide)
		{
			focusBall = defender.courtSide == newSide;
		}

        private void Crossing(int side)
        {
			SetFocus(match.GetPlayerBySide(-side));
			focusBall = false;
        }
    }
}
