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
		public const int FrictionRound = 10; // Velocity under this will be rounded off
        public const int CheckDistance = 1; // Distance below hurtbox to check for standing
        public const int MaxStamina = 100;
        public const int MaxEndurance = 100;

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
		public int points;
		public int games;
		public int sets;
        public int direction;
		public float hitLag;
		public float stamina = MaxStamina;
		public float endurance = MaxEndurance;
		public Vector2 velocity = Vector2.Zero;
		private ParticleGenerator runningParticles;

		public Player(Character character, Match match, PlayerIndex index, int courtSide, 
			Point spawnPosition)
		{
			this.match = match;
			this.index = index;
			this.courtSide = courtSide;
			this.spawnPosition = spawnPosition; // default point to put the player at when the round begins

			input = new InputManager(index,forwardDirection: -courtSide);
			state = new StateManager(this);

			name = character.name;
			direction = -courtSide;
			rectangle = new Rectangle(spawnPosition, character.rectangle.Size);
			moveset = new Moveset(character.attacks, this);
			sprite = new Sprite(character.spriteSheet, character.rectangle.Size, character.animations, this);
			stats = character.stats;
			stepSound = character.stepSound;
			jumpSound = character.jumpSound;
			turnSound = character.turnSound;

			runningParticles = new ParticleGenerator(
				Position,
				match.court.courtTexture,
				minColor: Color.Gray, maxColor: Color.LightGray,
				minSpeed: 400, maxSpeed: 800,
				minLife: 0.2f, maxLife: 0.3f,
				minDirection: Vector2.Zero, maxDirection: Vector2.Zero,
				minAcceleration: new Vector2(0, 60), maxAcceleration: new Vector2(0, 90),
				duration: 1,
				fireRate: 0.033f,
				loops: true,
				cutout: new Rectangle(0, 0, 10, 10),
				useCutout: true);

            state.Jumped += Jump;
			state.Turned += Turn;
			state.Landed += Land;
			state.FastFell += FastFall;
        }

		public Point Position { get { return rectangle.Location; } set { rectangle.Location = value; } }

		public void Update()
        {
            // Don't update during hitlag
            if (hitLag > 0)
            {
                hitLag -= TennisFightingGame.DeltaTime;
                return;
            }
			
			// Apply friction and gravity when appropiate
			switch (state.aerialState)
            {
				// Gravity
                case AerialStates.Airborne:
	                {
	                    velocity.Y += stats.gravity * TennisFightingGame.DeltaTime;
	                    break;
	                }

				/* When running, friction is applied so the player has deceleration when he stops running.
				 * When walking, velocity is set to 0. Since this is done before setting velocity beased on
				 * the movement state, this makes velocity constant when walking.
				 * This must be done before updating state because, when walk is released, movementState 
				 * is set to idle and thus velocity will not be 0 on release, which results in 
				 * undesired deceleration for walking. */
				case AerialStates.Standing:
					{
	                    if (state.Walking || state.serving)
	                    {
	                        velocity.X = 0; // constant x speed while not running
	                    }
						
						if (Math.Abs(velocity.X) > FrictionRound)
                    	{
							velocity.X -= stats.friction * TennisFightingGame.DeltaTime * Math.Sign(velocity.X);
                    	}

	                    break;
                	}
            }

			// Update components
			input.Update();
            state.Update();
			moveset.Update();
			sprite.Update();

        	runningParticles.position = new Point(rectangle.Center.X, rectangle.Bottom);
        	runningParticles.cutout = new Rectangle(
        		rectangle.Center.X + 1125,
        		rectangle.Bottom + 380,
        		runningParticles.cutout.Width,
        		runningParticles.cutout.Height);
        	runningParticles.Update();
        	runningParticles.enabled = false;

			if (match.inPlay)
			{
				AddStamina(stats.staminaRegen * TennisFightingGame.DeltaTime);
				
				endurance = MathHelper.Clamp(
					endurance - stats.enduranceDegen * TennisFightingGame.DeltaTime, 
					0, 
					MaxEndurance);
			}

			// Set velocity and stamina costs based on movement state
			switch (state.movementState)
			{
				case MovementStates.WalkingBackwards:
					{
						velocity.X = -stats.walkSpeed;
						AddStamina(-stats.walkStaminaCost * TennisFightingGame.DeltaTime);
						break;
					}
				case MovementStates.WalkingForwards:
					{
						velocity.X = stats.walkSpeed;
						AddStamina(-stats.walkStaminaCost * TennisFightingGame.DeltaTime);
						break;
					}
				case MovementStates.SprintingBackwards:
					{
						velocity.X = -stats.runSpeed;
						AddStamina(-stats.runStaminaCost * TennisFightingGame.DeltaTime);

						runningParticles.SetDirection(new Vector2(0.5f, -1));
						runningParticles.enabled = true;

						break;
					}
				case MovementStates.SprintingForwards:
					{
						velocity.X = stats.runSpeed;
						AddStamina(-stats.runStaminaCost * TennisFightingGame.DeltaTime);

						runningParticles.SetDirection(new Vector2(-0.5f, -1));
						runningParticles.enabled = true;

						break;
					}
				case MovementStates.DriftingBackwards:
					{
						if (Math.Sign(velocity.X) > 0)
						{
							velocity.X += -stats.driftSpeed;
						}

						break;
					}
				case MovementStates.DriftingForwards:
					{
						if (Math.Sign(velocity.X) < 0)
						{
							velocity.X += stats.driftSpeed;
						}

						break;
					}
				case MovementStates.CrawlingBackwards:
					{
						velocity.X = -stats.exhaustedSpeed;
						break;
					}
				case MovementStates.CrawlingForwards:
					{
						velocity.X = stats.exhaustedSpeed;
						break;
					}
			}

			Position += (velocity * TennisFightingGame.DeltaTime).ToPoint(); // update position

            // Resolve collision between player and walls by moving player out of the wall
            foreach (Wall wall in match.court.PlayerGeometry)
            {
                Collision collision = wall.rectangle.Collision(rectangle, lastRectangle);

                if (collision.Intersects)
                {
                    Position += wall.rectangle.Correction(rectangle, lastRectangle);
                }
            }

            lastRectangle = rectangle; // previous update rectangle
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			sprite.Draw(spriteBatch);
			moveset.Draw(spriteBatch);

			runningParticles.Draw(spriteBatch);

			if (TennisFightingGame.ConfigFile.Boolean("Debug", "Collisionboxes"))
			{
				spriteBatch.Draw(Assets.PlaceholderTexture, rectangle, Color.Yellow * .5f);
			}

		}

		public void AddStamina(float value)
		{
			stamina = MathHelper.Clamp(stamina + value, 0, MaxStamina);
		}

		public void AddEndurance(float value)
		{
			endurance = MathHelper.Clamp(endurance + value, 0, MaxEndurance);
		}

        private void Jump()
        {
			if (!state.serving)
			{
				switch (state.movementState)
				{
					case MovementStates.Idle:
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
					case MovementStates.WalkingBackwards:
					case MovementStates.WalkingForwards:
						{
							velocity.Y = -stats.jumpSpeed;
							AddStamina(-stats.jumpStaminaCost);
							break;
						}
					case MovementStates.SprintingBackwards:
					case MovementStates.SprintingForwards:
						{
							velocity.Y = -stats.runningJumpSpeed;
							AddStamina(-stats.exhaustedJumpStaminaCost);
							break;
						}
					case MovementStates.CrawlingBackwards:
					case MovementStates.CrawlingForwards:
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
			input.forwardDirection = direction;
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
