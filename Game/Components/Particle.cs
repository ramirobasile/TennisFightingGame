using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TennisFightingGame
{
	public class Particle
	{
		private Rectangle rectangle;
		public float life;
		private Color color;
		private readonly Texture2D texture;
		private readonly float speed;
		private readonly Vector2 direction;
		private readonly Vector2 acceleration;

		private Vector2 velocity;

		public Particle(Rectangle rectangle, float life, Color color, 
			Texture2D texture, float speed, Vector2 direction, 
			Vector2 acceleration)
		{
			this.rectangle = rectangle;
			this.life = life;
			this.color = color;
			this.speed = speed;
			this.texture = texture;
			this.direction = direction;
			this.acceleration = acceleration;

			velocity = direction * speed;
		}

		public Point Position { get { return rectangle.Location; } set { rectangle.Location = value; } }

		public void Update()
		{
			life -= TennisFightingGame.DeltaTime;

			velocity += acceleration;

			Position += (velocity * TennisFightingGame.DeltaTime).ToPoint();
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(texture, rectangle, color);
		}
	}
}
