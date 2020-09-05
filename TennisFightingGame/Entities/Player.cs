using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace TennisFightingGame
{
	/// <summary>
	/// Represents a player and handles physics, such as movement and collision.
	/// The player moves by reacting to the state the state manager sets, which does so based on inputs.
	/// </summary>
	public class Player
    {
		private const int FrictionRound = 10; // Velocity under 10 will be rounded off
        public const int CheckDistance = 1; // Distance below hurtbox to check for standing
        public const int MaxStamina = 100;

		public readonly Match match;
        public readonly StateManager state;
		public Sprite sprite;
		public Moveset moveset;
        public readonly InputManager input;
        public readonly PlayerIndex index;
		public readonly Stats stats;
		public readonly string name;
        public readonly Point spawnPosition;
        private readonly SoundEffect stepSound;
        private readonly SoundEffect jumpSound;
        private readonly SoundEffect turnSound;

		public Rectangle rectangle;
        public Rectangle lastRectangle;
        public int courtSide;
        public int direction;
		public float hitLag;
		public float stamina = MaxStamina;
		public Vector2 velocity = Vector2.Zero;

		public Player(Character character, Match match, PlayerIndex index, int courtSide, 
			Point spawnPosition)
		{
			this.match = match;
			this.index = index;
			this.courtSide = courtSide;
			this.spawnPosition = spawnPosition; // default point to put the player at when the round begins

			input = new InputManager(index);
			state = new StateManager(this);

			name = character.name;
			direction = courtSide;
			rectangle = new Rectangle(spawnPosition, character.rectangle.Size);
			moveset = new Moveset(character.attacks, this);
			sprite = new Sprite(character.spriteSheet, character.rectangle.Size, character.animations, this);
			stats = character.stats;
			stepSound = character.stepSound;
			jumpSound = character.jumpSound;
			turnSound = character.turnSound;

            state.Jumped += Jump;
			state.Turned += Turn;
			state.Landed += Land;
			state.FastFell += FastFall;
        }

		// Returns and sets the XY of the player's rectangle
		public Point Position
		{
			get { return new Point(rectangle.X, rectangle.Y); }
			set
			{
				rectangle.X = value.X;
				rectangle.Y = value.Y;
			}
		}

		public void Update()
        {
            // Don't update during hitlag
            if (hitLag > 0)
            {
                hitLag -= Game1.DeltaTime;
                return;
            }

			// Update subcomponents
			input.Update();
            state.Update();
			moveset.Update();
			sprite.Update();

			if (match.inPlay)
			{
				AddStamina(stats.staminaRegen * Game1.DeltaTime);
			}

			// Apply friction and gravity when appropiate
			switch (state.aerialState)
            {
				// Gravity
                case AerialState.Airborne:
	                {
	                    velocity.Y += stats.gravity * Game1.DeltaTime;
	                    break;
	                }

				/* When running, friction is applied so the player has deceleration when he stops running.
				 * When walking, velocity is set to 0. Since this is done before setting velocity beased on
				 * the movement state, this makes velocity constant when walking.*/
				case AerialState.Standing:
					{
						if (Math.Abs(velocity.X) > FrictionRound)
                    	{
							velocity.X -= stats.friction * Game1.DeltaTime * Math.Sign(velocity.X);
                    	}

	                    if (state.Walking || state.serving)
	                    {
	                        velocity.X = 0; // constant x speed while not running
	                    }

	                    break;
                	}
            }

			// Set velocity and stamina costs based on movement state
			switch (state.movementState)
			{
				case MovementState.WalkingBackwards:
					{
						velocity.X = -stats.walkSpeed;
						AddStamina(-stats.walkStaminaCost * Game1.DeltaTime);
						break;
					}
				case MovementState.WalkingForwards:
					{
						velocity.X = stats.walkSpeed;
						AddStamina(-stats.walkStaminaCost * Game1.DeltaTime);
						break;
					}
				case MovementState.SprintingBackwards:
					{
						velocity.X = -stats.runSpeed;
						AddStamina(-stats.runStaminaCost * Game1.DeltaTime);
						break;
					}
				case MovementState.SprintingForwards:
					{
						velocity.X = stats.runSpeed;
						AddStamina(-stats.runStaminaCost * Game1.DeltaTime);
						break;
					}
				case MovementState.DriftingBackwards:
					{
						if (Math.Sign(velocity.X) > 0)
						{
							velocity.X += -stats.driftSpeed;
						}

						break;
					}
				case MovementState.DriftingForwards:
					{
						if (Math.Sign(velocity.X) < 0)
						{
							velocity.X += stats.driftSpeed;
						}

						break;
					}
				case MovementState.CrawlingBackwards:
					{
						velocity.X = -stats.exhaustedSpeed;
						break;
					}
				case MovementState.CrawlingForwards:
					{
						velocity.X = stats.exhaustedSpeed;
						break;
					}
			}

			Position += (velocity * Game1.DeltaTime).ToPoint(); // update position

            // Resolve collision between player and walls by moving player out of the wall
            foreach (Wall wall in match.court.PlayerGeometry)
            {
                Collision collision = wall.Collision(rectangle, lastRectangle);

                if (collision.Intersects)
                {
                    Position += wall.Correction(rectangle, lastRectangle);
                }
            }

            lastRectangle = rectangle; // previous update rectangle
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			sprite.Draw(spriteBatch);
			moveset.Draw(spriteBatch);

			if (Game1.ConfigFile.Boolean("Debug", "Collisionboxes"))
			{
				spriteBatch.Draw(Assets.PlaceholderTexture, rectangle, Color.Yellow * .5f);
			}

		}

		public void AddStamina(float value)
		{
			stamina = MathHelper.Clamp(stamina + value, 0, MaxStamina);
		}

        private void Jump()
        {
			if (!state.serving)
			{
				switch (state.movementState)
				{
					case MovementState.Idle:
						{
							if (state.exhausted)
							{
								velocity.Y = -stats.exhaustedJumpSpeed;
								AddStamina(-stats.exhaustedJumpStaminaCost);
							}
							else
							{
								velocity.Y = -stats.jumpSpeed;
								AddStamina(-stats.jumpStaminaCost);
							}

							break;
						}
					case MovementState.WalkingBackwards:
					case MovementState.WalkingForwards:
						{
							velocity.Y = -stats.jumpSpeed;
							AddStamina(-stats.jumpStaminaCost);
							break;
						}
					case MovementState.SprintingBackwards:
					case MovementState.SprintingForwards:
						{
							velocity.Y = -stats.runningJumpSpeed;
							AddStamina(-stats.exhaustedJumpStaminaCost);
							break;
						}
					case MovementState.CrawlingBackwards:
					case MovementState.CrawlingForwards:
						{
							velocity.Y = -stats.exhaustedJumpSpeed;
							AddStamina(-stats.exhaustedJumpStaminaCost);
							break;
						}
				}

				Helpers.PlaySFX(jumpSound);
			}
        }

		private void Turn()
		{
			direction *= -1;
			Helpers.PlaySFX(turnSound);
		}

		private void Land()
		{
			velocity.Y = 0;
		}
		
		private void FastFall()
		{
			velocity.Y += stats.fastFallSpeed;
			Helpers.PlaySFX(Assets.FastFallSound);
		}
	}
}