using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TennisFightingGame
{
	/// <summary>
	/// Handles the player's animations.
	/// Sets the current animation based on the player's state. It updates that animation and draws 
	/// it by feeding it a sprite sheet to get frames from and a frame size.
	/// </summary>

	public class Sprite
	{
		// TODO Refactor uppercase and make constant
		const float ShadowOpacity = 0.3f;
		const int Margins = 3; // HACK Make this a parameter

		public readonly Texture2D spriteSheet;
		public readonly Point frameSize;
		private readonly Animation[] animations;

		public Player player;
		public Animation currentAnimation;
		private Animation lastAnimation;

		public Sprite(Texture2D spriteSheet, Point frameSize, Animation[] animations,
			Player player)
		{
			this.spriteSheet = spriteSheet;
			this.frameSize = frameSize;
			this.animations = animations;
			this.player = player;

			currentAnimation = animations[(int)MovementStates.Idle];
			lastAnimation = animations[(int)MovementStates.Idle];
		}

		// For serialization
		public Sprite()
		{
		}

		public Rectangle FrameCutout 
		{ 
			get 
			{ 
				int frame = currentAnimation.currentFrame;
				int row = currentAnimation.row;

				return new Rectangle(
					frame * frameSize.X + Margins * (frame + 1),
					row * frameSize.Y + Margins * (row + 1), 
					frameSize.X, 
					frameSize.Y);
			}
		}

		public void Update()
		{
			if (player.state.Attacking)
			{
				// TODO Give each attack an animation maybe? Seems like the only
				// not insane solution
				//currentAnimation = player.moveset.currentAttack.animation;
			}
			else
			{
				currentAnimation = animations[(int)player.state.movementState];
			}

			if (lastAnimation != currentAnimation && currentAnimation.resetOnStop)
			{
				currentAnimation.Reset();
			}

			currentAnimation.Update();

			lastAnimation = currentAnimation;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			// Draw shadow
			Rectangle shadowRectangle = new Rectangle(player.Position.X + frameSize.X / 4,
				player.match.court.floor.rectangle.Top - frameSize.Y / 8,
				frameSize.X / 2,
				frameSize.Y / 4);

			spriteBatch.Draw(Assets.ShadowTexture, shadowRectangle, Color.White * ShadowOpacity);

			// Draw animation
			currentAnimation.Draw(spriteBatch, spriteSheet, Margins, frameSize,  
				player.Position, player.direction);
		}
	}
}
