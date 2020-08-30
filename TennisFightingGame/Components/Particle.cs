using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TennisFightingGame
{
	public struct Particle
	{
		float life;
		Color color;
		Texture2D texture;
		Vector2 velocity;
		Vector2 gravity;

		public void Update()
		{
			life -= Game1.DeltaTime;
		}
	}
}
