using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TennisFightingGame
{
	/// <summary>
	/// Base camera that each gamemode will use for their own camera.
	/// The base camera can follow a point, zoom in and out and shake for a specified duration.
	/// </summary>
    public abstract class Camera
    {
		const float ShakeOffsetThreshold = 1;
		const float ShakeSpeed = 100;

        public Vector2 centre;

        public Matrix transform;
        public float zoom;
		public float shake;
		public int shakeMagnitude = 10;

		private Random random;
		private Vector2 offset;
		private Vector2 goal;

		public Camera(float zoom = 1)
        {
            this.zoom = zoom;

			random = new Random();
        }

        public virtual void Update()
		{
			shake -= TennisFightingGame.DeltaTime;

			if (shake > 0)
			{
				/* If offset is close enough to goal, either make goal zero if it wasn't before or
				 * give goal a random value. This basically makes offset keep bouncing around zero and
				 * random values. */			
				if(Vector2.Distance(offset, goal) < ShakeOffsetThreshold)
				{
					if (goal == Vector2.Zero)
					{
						goal = new Vector2(random.Next(-shakeMagnitude, shakeMagnitude), 
							random.Next(-shakeMagnitude, shakeMagnitude));
					}
					else
					{
						goal = Vector2.Zero;
					}
				}

				offset.X = MathHelper.Lerp(offset.X, goal.X, ShakeSpeed * TennisFightingGame.DeltaTime);
				offset.Y = MathHelper.Lerp(offset.Y, goal.Y, ShakeSpeed * TennisFightingGame.DeltaTime);
			}
			else
			{
				offset = Vector2.Zero;
			}

			transform =
                Matrix.CreateScale(new Vector3(zoom, zoom, 0)) *
                Matrix.CreateTranslation(new Vector3(
                    (-centre.X + offset.X) * zoom + TennisFightingGame.Viewport.Width / 2,
                    (-centre.Y + offset.Y) * zoom + TennisFightingGame.Viewport.Height / 2, 0)
                );
        }
    }
}