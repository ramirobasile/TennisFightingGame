using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace TennisFightingGame
{
	/// <summary>
	/// Just the ball. Throws net passing events and such too.
	/// </summary>
	public class Ball
    {
		const float ShadowOpacity = 0.2f;
        private const float Gravity = 2400;
		private const int NormalBounce = 750;
		private const int NonBounce = 300;

		private readonly Texture2D texture;
		private readonly Wall[] geometry;

        public Rectangle rectangle;
        public Vector2 velocity = new Vector2(0, 0);
		public Rectangle lastRectangle;
		public float hitStun;
		public Polynomial gravityScalar = Polynomial.Identity;
		private float gravityScalarTime;

		public Ball(Rectangle rectangle, Texture2D texture, Wall[] geometry)
        {
            this.rectangle = rectangle;
            this.texture = texture;
            this.geometry = geometry;
        }

        public Point Position
        {
            get { return new Point(rectangle.X, rectangle.Y); }
            set
            {
                rectangle.X = value.X;
                rectangle.Y = value.Y;
            }
        }

		public delegate void BouncedEventHandler();
		public delegate void HittedEventHandler();

		public event BouncedEventHandler Bounced;
        public event HittedEventHandler Hitted; // yeah, I know, I know...

        public void Update()
        {
            if (hitStun > 0)
            {
                hitStun -= Game1.DeltaTime;
                return;
            }

            lastRectangle = rectangle; // previous update rectangle
			gravityScalarTime += Game1.DeltaTime;

            velocity.Y += Gravity * gravityScalar.Of(gravityScalarTime) * Game1.DeltaTime;
            Position += (velocity * Game1.DeltaTime).ToPoint(); // update position

            // Collision correction and bouncing (and bounce event call)
            foreach (Wall wall in geometry)
            {
                Collision collision = wall.Collision(rectangle, lastRectangle);

                if (rectangle.Intersects(wall.rectangle))
                {
					gravityScalar = Polynomial.Identity;
                    velocity.X *= wall.friction.X;
					velocity.Y *= wall.friction.Y;
					Position += wall.Correction(rectangle, lastRectangle);

					if (velocity.Length() > NonBounce)
					{
						float volume = MathHelper.Clamp(velocity.Length() / NormalBounce, 0, 1);
						Helpers.PlaySFX(Assets.BounceSound, volume);
					}
				}

				if (velocity.Length() > NonBounce)
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
						Position += wall.Correction(rectangle, lastRectangle);
					}
				}
            }
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			// TODO Match court floor
			// If !inPlay, return
			
			Rectangle shadowRectangle = new Rectangle(Position.X,
				0 - rectangle.Height / 2,
				rectangle.Width,
				rectangle.Height);

			spriteBatch.Draw(Assets.ShadowTexture, shadowRectangle, Color.White * ShadowOpacity);

			// TODO Sprite or texture
			spriteBatch.Draw(Assets.PlaceholderTexture, rectangle, Color.ForestGreen);
		}

		public void Hit(Vector2 newVelocity)
		{
			velocity = newVelocity;

			if (Hitted != null)
			{
				Hitted.Invoke();
			}
		}

		// Overloading Hit since Polynomial can't really be made optional
		public void Hit(Vector2 newVelocity, Polynomial polynomial)
		{
			velocity = newVelocity;
			gravityScalarTime = 0;
			gravityScalar = polynomial;

			if (Hitted != null)
			{
				Hitted.Invoke();
			}
		}

		public void AddVelocity(Vector2 velocity)
		{
			this.velocity += velocity;
		}

		public Point LandingPoint(int floorLevel)
        {
            double a = Gravity / 2;
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