using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TennisFightingGame
{
	///<summary>
	/// Base
	///</summary>
	public class ParticleGenerator
	{
		public Point position;
		public Texture2D texture; 
		public Color color;
		private readonly float[] speedRange;
		private readonly float[] lifeRange;
		private readonly Vector2[] directionRange;
		private readonly Vector2[] accelerationRange;

		private readonly float duration;
		private readonly float fireRate;
		private readonly bool loops;

		List<Particle> particles = new List<Particle>();
		float fireTimer;
		
		///<summary>
		/// Fixed values rather than ranged random values
		///</summary>
		public ParticleGenerator(Point position, Texture2D texture, Color color,
			float speed, float life, Vector2 direction, Vector2 acceleration,
			float duration, float fireRate, bool loops = false)
		{
			this.position = position;
			this.texture = texture;
			this.color = color;
			speedRange = new float[] { speed, speed };
			lifeRange = new float[] { life, life };
			directionRange = new Vector2[] { direction, direction };
			accelerationRange = new Vector2[] { acceleration, acceleration };
			this.duration = duration;
			this.fireRate = fireRate;
			this.loops = loops;
		}
		
		// TODO
		public ParticleGenerator()
		{
		}
		
		public virtual void Update()
		{
			fireTimer += Game1.DeltaTime;

			if (fireTimer >= fireRate)
			{
				particles.Add(Fire(position, texture, color, speedRange, 
					lifeRange, directionRange, accelerationRange));
				
				if (loops)
				{
					fireTimer = 0;
				}
			}

			for (int i = 0; i < particles.Count; i++)
			{
				particles[i].Update();

				if (particles[i].life <= 0)
				{
					particles.RemoveAt(i);
				}
			}
		}

		private Particle Fire(Point position, Texture2D texture, Color color,
			float[] speedRange, float[] lifeRange, Vector2[] directionRange, 
			Vector2[] accelerationRange)
		{
			Rectangle rectangle = new Rectangle(position, texture.Bounds.Size);
			float speed = Game1.Random.Next((int)(speedRange[0] * 100), 
											(int)(speedRange[1] * 100)) / 100;
			float life = Game1.Random.Next(	(int)(lifeRange[0] * 100), 
											(int)(lifeRange[1] * 100)) / 100;
			Vector2 direction = new Vector2(
				Game1.Random.Next(
					(int)(directionRange[0].X * 100), 
					(int)(directionRange[1].X * 100)
					) / 100,
				Game1.Random.Next(
					(int)(directionRange[0].Y * 100), 
					(int)(directionRange[1].Y * 100)
					) / 100
			);
			Vector2 acceleration = new Vector2(
				Game1.Random.Next(
					(int)(accelerationRange[0].X * 100), 
					(int)(accelerationRange[1].X * 100)
					) / 100,
				Game1.Random.Next(
					(int)(accelerationRange[0].Y * 100), 
					(int)(accelerationRange[1].Y * 100)
					) / 100
			);

			return new Particle(rectangle, life, color, texture, speed, direction, 
				acceleration);
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			foreach (Particle particle in particles)
			{
				particle.Draw(spriteBatch);
			}
		}
	}
}
