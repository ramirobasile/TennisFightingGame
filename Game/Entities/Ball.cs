using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TennisFightingGame
{
	/// <summary>
	/// Just the ball. Throws net passing events and such too.
	/// </summary>
	public class Ball
    {
        public const float DefaultGravity = 2400;
		private const float ShadowOpacity = 0.2f;
		private const int NormalBounceThreshold = 750;
		private const int NonBounceThreshold = 300;

        public Rectangle rectangle;
		private readonly Texture2D texture;
		private readonly Match match;

        public Vector2 velocity = new Vector2(0, 0);
		public float hitStun;
		public float gravity;
		public Rectangle lastRectangle;

		public Ball(Rectangle rectangle, Texture2D texture, Match match)
        {
            this.rectangle = rectangle;
            this.texture = texture;
			this.match = match;
        }

		public Point Position { get { return rectangle.Location; } set { rectangle.Location = value; } }

		public delegate void BouncedEventHandler();
		public delegate void HittedEventHandler();

		public event BouncedEventHandler Bounced;
        public event HittedEventHandler Hitted; // yeah, I know, I know...

        public void Update()
        {
            if (hitStun > 0)
            {
                hitStun -= TennisFightingGame.DeltaTime;
                return;
            }

            lastRectangle = rectangle; // previous update rectangle

            velocity.Y += gravity * TennisFightingGame.DeltaTime;
            Position += (velocity * TennisFightingGame.DeltaTime).ToPoint(); // update position

            // Collision correction and bouncing (and bounce event call)
            foreach (Wall wall in match.court.Geometry)
            {
                Collision collision = wall.rectangle.Collision(rectangle, lastRectangle);

                if (rectangle.Intersects(wall.rectangle))
                {
					gravity = DefaultGravity;
                    velocity.X *= wall.friction.X;
					velocity.Y *= wall.friction.Y;
					Position += wall.rectangle.Correction(rectangle, lastRectangle);

					if (velocity.Length() > NonBounceThreshold)
					{
						float volume = MathHelper.Clamp(velocity.Length() / NormalBounceThreshold, 0, 1);
						Helpers.PlaySFX(Assets.BounceSound, volume);
					}
				}

				if (velocity.Length() > NonBounceThreshold)
				{
					if (collision.Top || collision.Bottom)
					{
						velocity.Y *= -1;

						if (Bounced != null)
						{
							Bounced.Invoke();
						}
					}

					if (collision.Left || collision.Right)
					{
						velocity.X *= -1;
					}

					if (rectangle.Intersects(wall.rectangle))
					{
						Position += wall.rectangle.Correction(rectangle, lastRectangle);
					}
				}
            }
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			Rectangle shadowRectangle = new Rectangle(Position.X,
				match.court.floor.rectangle.Top - rectangle.Height / 2,
				rectangle.Width,
				rectangle.Height);

			if (Position.Y <= shadowRectangle.Y)
			{
				spriteBatch.Draw(Assets.ShadowTexture, shadowRectangle, Color.White * ShadowOpacity);
			}

			// TODO Sprite or texture
			spriteBatch.Draw(Assets.PlaceholderTexture, rectangle, Color.ForestGreen);
		}

		public void Hit(Vector2 newVelocity, float newGravity = DefaultGravity)
		{
			velocity = newVelocity;
			gravity = newGravity;

			if (Hitted != null)
			{
				Hitted.Invoke();
			}
		}

		public Point LandingPoint(int floorLevel)
        {
            double a = gravity / 2;
            double b = velocity.Y;
            double c = Position.Y - floorLevel;

            double sqrtpart = Math.Sqrt(b * b - 4 * a * c);
            double t;

            if ((-b + sqrtpart) / (2 * a) > 0)
            {
                t = (-b + sqrtpart) / (2 * a);
            }
            else
            {
                t = (-b - sqrtpart) / (2 * a);
            }

            double x = Position.X + velocity.X * t;

            return new Point((int) x, floorLevel);
        }
    }
}