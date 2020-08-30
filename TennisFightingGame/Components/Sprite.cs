using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TennisFightingGame
{
	/// <summary>
	/// Handles the player's animations.
	/// Sets the current animation based on the player's state. It updates that animation and draws 
	/// it by feeding it a sprite sheet to get frames from and a frame size.
	/// </summary>
	public struct Sprite
	{
		private readonly Texture2D spriteSheet;
		public readonly Point frameSize;
		private readonly int margins;
		private readonly Animation[] animations;

		public Player player;
		public Animations currentAnimation;
		private Animations lastAnimation;

		public Sprite(Texture2D spriteSheet, Point frameSize, int margins, Animation[] animations)
		{
			this.spriteSheet = spriteSheet;
			this.frameSize = frameSize;
			this.margins = margins;
			this.animations = animations;

			player = null;
			currentAnimation = Animations.Idle;
			lastAnimation = Animations.Idle;
			ChangedAnimation = null; // ffs c#
		}

		public delegate void ChangedAnimationEventHandler(Animations newAnimation);

		public event ChangedAnimationEventHandler ChangedAnimation;

		private Animation CurrentAnimation { get { return animations[(int)currentAnimation]; } }

		public void Update()
		{
			switch (player.state.aerialState)
			{
				case AerialState.Airborne:
					{
						SetAnimation(Animations.Falling);
						break;
					}
				case AerialState.Standing:
					{
						SetAnimation(Animations.Idle);
						break;
					}
			}

			if (!player.state.serving)
			{
				switch (player.state.movementState)
				{
					case MovementState.WalkingBackwards:
						{
							SetAnimation(Animations.WalkingBackwards);
							break;
						}
					case MovementState.WalkingForwards:
						{
							SetAnimation(Animations.WalkingForwards);
							break;
						}
					case MovementState.SprintingBackwards:
						{
							SetAnimation(Animations.SprintingBackwards);
							break;
						}
					case MovementState.SprintingForwards:
						{
							SetAnimation(Animations.SprintingForwards);
							break;
						}
					case MovementState.DriftingBackwards:
						{
							SetAnimation(Animations.DriftingBackwards);
							break;
						}
					case MovementState.DriftingForwards:
						{
							SetAnimation(Animations.DriftingForwards);
							break;
						}
					case MovementState.CrawlingBackwards:
						{
							SetAnimation(Animations.CrawlingBackwards);
							break;
						}
					case MovementState.CrawlingForwards:
						{
							SetAnimation(Animations.CrawlingForwards);
							break;
						}
				}
			}

			switch (player.state.CurrentAttack)
			{
				case Attacks.Attack1:
					{
						SetAnimation(Animations.Attack1);
						break;
					}
				case Attacks.Attack1Air:
					{
						SetAnimation(Animations.Attack1Air);
						break;
					}
				case Attacks.Attack2:
					{
						SetAnimation(Animations.Attack2);
						break;
					}
				case Attacks.Attack2Air:
					{
						SetAnimation(Animations.Attack2Air);
						break;
					}
				case Attacks.Attack3:
					{
						SetAnimation(Animations.Attack3);
						break;
					}
				case Attacks.Attack3Air:
					{
						SetAnimation(Animations.Attack3Air);
						break;
					}
				case Attacks.Serve1:
				case Attacks.Serve2:
				case Attacks.Serve3:
					{
						SetAnimation(Animations.Serve);
						break;
					}
			}

			if (lastAnimation != currentAnimation && CurrentAnimation.resetOnStop)
			{
				CurrentAnimation.Reset();
			}

			CurrentAnimation.Update();

			lastAnimation = currentAnimation;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			// Draw shadow
			Rectangle shadowRectangle = new Rectangle(player.Position.X + frameSize.X / 4,
				player.match.court.floor.rectangle.Top - frameSize.Y / 8,
				frameSize.X / 2,
				frameSize.Y / 4);
			spriteBatch.Draw(Assets.ShadowTexture, shadowRectangle, Color.White * 0.5f);

			// Draw animation
			CurrentAnimation.Draw(spriteBatch, spriteSheet, margins, frameSize, 
				player.Position);
		}

		private void SetAnimation(Animations animation)
		{
			currentAnimation = animation;

			if (ChangedAnimation != null)
			{
				ChangedAnimation.Invoke(animation);
			}
		}
	}

	public enum Animations
	{
		Idle,
		WalkingForwards,
		WalkingBackwards,
		SprintingForwards,
		SprintingBackwards,
		CrawlingForwards,
		CrawlingBackwards,
		DriftingForwards,
		DriftingBackwards,
		Jumping,
		Falling,
		Landing,
		Attack1,
		Attack1Air,
		Attack2,
		Attack2Air,
		Attack3,
		Attack3Air,
		Serve
	}
}
