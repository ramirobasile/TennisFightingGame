using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TennisFightingGame
{
	/// <summary>
	/// An Animation object has a draw method that draws a particular frame (currentFrame) of an 
	/// animation, which is cycled over by its update method at a certain rate determined by the fps 
	/// field.
	/// A sprite sheet has rows of frames that constitute an animation. Each row is separated by the
	/// full height of a single frame plus a small margin. Likewise, each frame is separated by the full
	/// width of a single frame plus the same small margin.
	/// </summary>
	public class Animation
	{
		public readonly int row;
		private readonly float fps;
		private readonly int frames;
		private readonly bool loops;
		public readonly bool resetOnStop;

		public float time;
		public int currentFrame;

		public Animation(int row, int frames, float fps, bool loops = false, bool resetOnStop = true)
		{
			this.row = row;
			this.frames = frames;
			this.fps = fps;
			this.loops = loops;
			this.resetOnStop = resetOnStop;
		}

		// Deep copy
		public Animation(Animation reference)
		{
			row = reference.row;
			frames = reference.frames;
			fps = reference.fps;
			loops = reference.loops;
			resetOnStop = reference.resetOnStop;
		}

		// For serialization
		private Animation()
		{
		}

		public void Update()
		{
			time += Game1.DeltaTime;

			if (time > fps)
			{
				time = 0;

				if (loops)
				{
					currentFrame = Helpers.Wrap(currentFrame + 1, 0, frames - 1);
				}
				else
				{
					currentFrame = MathHelper.Clamp(currentFrame + 1, 0, frames - 1);
				}
			}
		}

		public void Reset()
		{
			time = 0;
			currentFrame = 0;
		}

		public void Draw(SpriteBatch spriteBatch, Texture2D spriteSheet, int margin, 
			Point size, Point position, float direction)
		{
			Rectangle cutout = new Rectangle(currentFrame * size.X + margin * (currentFrame + 1),
				row * size.Y + margin * (row + 1), size.X, size.Y);

			// TODO Make this flipped when courtSide and direction arent equal
			if (direction == 1)
			{
				spriteBatch.Draw(spriteSheet, position.ToVector2(), cutout,  Color.White, 
					0, Vector2.Zero, Vector2.One, SpriteEffects.FlipHorizontally, 0);
			}
			else
			{
				spriteBatch.Draw(spriteSheet, position.ToVector2(), cutout, Color.White);
			}
		}
	}
}