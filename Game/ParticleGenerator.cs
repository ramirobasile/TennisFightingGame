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
		public Color minColor, maxColor;
		float minSpeed, maxSpeed;
		float minLife, maxLife;
		Vector2 minDirection, maxDirection;
		Vector2 minAcceleration, maxAcceleration;
		private float duration;
		private float fireRate;
		private bool loops;
		public Rectangle cutout;
		private readonly bool useCutout;

		List<Particle> particles = new List<Particle>();
		float fireTimer;
		public bool enabled = true;
		
		public ParticleGenerator(Point position, Texture2D texture,
			Color minColor, Color maxColor,
			float minSpeed, float maxSpeed, float minLife, float maxLife,
			Vector2 minDirection, Vector2 maxDirection, Vector2 minAcceleration,
			Vector2 maxAcceleration, float duration, float fireRate,
			bool loops = false, Rectangle cutout = default(Rectangle),
			bool useCutout = false)
		{
			this.position = position;
			this.texture = texture;
			this.minColor = minColor;
			this.maxColor = maxColor;
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
			this.cutout = cutout;
			this.useCutout = useCutout;
		}

		///<summary>
		/// Fixed values for speed, life, direction and acceleration
		///</summary>
		public ParticleGenerator(Point position, Texture2D texture, Color color,
			float speed, float life, Vector2 direction, Vector2 acceleration,
			float duration, float fireRate, bool loops = false,
			Rectangle cutout = default(Rectangle), bool useCutout = false)
		{
			this.position = position;
			this.texture = texture;
			this.minColor = color;
			this.maxColor = color;
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
			this.cutout = cutout;
			this.useCutout = useCutout;
		}
		
		public virtual void Update()
		{
			fireTimer += TennisFightingGame.DeltaTime;

			if (fireTimer >= fireRate && enabled)
			{
				Particle newParticle = new Particle(
					new Rectangle(position, texture.Bounds.Size),
					TennisFightingGame.Random.NextFloat(minLife, maxLife),
					Helpers.RandomColor(minColor, maxColor),
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
				if (useCutout)
				{
					particle.Draw(spriteBatch, cutout);
				}
				else
				{
					particle.Draw(spriteBatch);
				}
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
