using System;
using Microsoft.Xna.Framework;

namespace TennisFightingGame
{
	/// <summary>
	/// Colliders with a friction value that a ball's velocity will scale off of when bounced from.
	/// </summary>
	public class Wall : Collider
	{
		public Vector2 friction;

		public Wall(Rectangle rect, Vector2 friction) 
			: base (rect)
		{
			this.friction = friction;
		}
	}
}
