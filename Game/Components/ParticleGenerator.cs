using System;
﻿using Microsoft.Xna.Framework;
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
		float minSpeed, maxSpeed;
		float minLife, maxLife;
		Vector2 minDirection, maxDirection;
		Vector2 minAcceleration, maxAcceleration;

		private float duration;
		private float fireRate;
		private bool loops;
		public bool enabled = true;

		List<Particle> particles = new List<Particle>();
		float fireTimer;
		
		public ParticleGenerator(Point position, Texture2D texture, Color color,
			float minSpeed, float maxSpeed, float minLife, float maxLife,
			Vector2 minDirection, Vector2 maxDirection, Vector2 minAcceleration,
			Vector2 maxAcceleration, float duration, float fireRate, bool loops = false)
		{
			this.position = position;
			this.texture = texture;
			this.color = color;
			this.minLife = minLife;
			this.maxLife = maxLife;
			this.minSpeed = minSpeed;
			this.maxSpeed = maxSpeed;
			this.minDirection = minDirection;
			this.maxDirection = maxDirection;
			this.minAcceleration = minAcceleration;
			this.maxAcceleration = maxAcceleration;
			this.duration = duration;
			this.fireRate = fireRate;
			this.loops = loops;
		}

		///<summary>
		/// Fixed values for speed, life, direction and acceleration
		///</summary>
		public ParticleGenerator(Point position, Texture2D texture, Color color,
			float speed, float life, Vector2 direction, Vector2 acceleration,
			float duration, float fireRate, bool loops = false)
		{
			this.position = position;
			this.texture = texture;
			this.color = color;
			minLife = life;
			maxLife = life;
			minSpeed = speed;
			maxSpeed = speed;
			minDirection = direction;
			maxDirection = direction;
			minAcceleration = acceleration;
			maxAcceleration = acceleration;
			this.duration = duration;
			this.fireRate = fireRate;
			this.loops = loops;
		}
		
		public virtual void Update()
		{
			fireTimer += TennisFightingGame.DeltaTime;

			if (fireTimer >= fireRate && enabled)
			{
				Particle newParticle = new Particle(
					new Rectangle(position, texture.Bounds.Size),
					TennisFightingGame.Random.NextFloat(minLife, maxLife),
					color,
					texture,
					TennisFightingGame.Random.NextFloat(minSpeed, maxSpeed),
					new Vector2(
						TennisFightingGame.Random.NextFloat(minDirection.X, maxDirection.X),
						TennisFightingGame.Random.NextFloat(minDirection.Y, maxDirection.Y)),
					new Vector2(
						TennisFightingGame.Random.NextFloat(minAcceleration.X, maxAcceleration.X),
						TennisFightingGame.Random.NextFloat(minAcceleration.Y, maxAcceleration.Y)));

				particles.Add(newParticle);
				
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

		public void Draw(SpriteBatch spriteBatch)
		{
			foreach (Particle particle in particles)
			{
				particle.Draw(spriteBatch);
			}
		}

		// TODO Remove
		public void SetDirection(Vector2 newDirection)
		{
			minDirection = newDirection;
			maxDirection = newDirection;
		}
	}
}
