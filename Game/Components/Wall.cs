using Microsoft.Xna.Framework;

namespace TennisFightingGame
{
	/// <summary>
	/// TODO
	/// </summary>
	public struct Wall
	{
		public Rectangle rectangle;
		public Vector2 friction;

		public Wall(Rectangle rectangle, Vector2 friction)
		{
			this.rectangle = rectangle;
			this.friction = friction;
		}
	}
}
