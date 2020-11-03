using System;
using Microsoft.Xna.Framework;

namespace TennisFightingGame
{
	/// <summary>
	/// Handles changing player state according to input. Te player then reacts to the current state.
	/// Tip: Do a Toggle All Folds on this file, it can get overwhelming.
	/// </summary>
	public class StateManager
	{
		private readonly Player player;

		public MovementStates movementState;
		public AerialStates aerialState;

		public bool serving;
		public bool exhausted;
		public bool fastFell;

		float jumpSquatTime;
		float turningTime;

		public StateManager(Player player)
		{
			this.player = player;

			player.input.Pressed += Press;
			player.input.Held += Hold;
			player.input.Released += Release;
			player.input.DoublePressed += DoublePress;
		}

		public bool Turning { get { return turningTime > 0; } }
		public bool Jumping { get { return jumpSquatTime > 0; } }
		public bool Attacking { get { return player.moveset.currentAttack != null; } }
		public bool Walking
		{
			get
			{
				return movementState == MovementStates.WalkingBackwards ||
					   movementState == MovementStates.WalkingForwards;
			}
		}
		public bool Sprinting
		{
			get
			{
				return movementState == MovementStates.SprintingBackwards ||
					   movementState == MovementStates.SprintingForwards;
			}
		}

		public delegate void JumpedEventHandler();
		public delegate void TurnedEventHandler();
		public delegate void LandedEventHandler();
		public delegate void FastFellEventHandler();

		public event JumpedEventHandler Jumped;
		public event TurnedEventHandler Turned;
		public event LandedEventHandler Landed;
		public event FastFellEventHandler FastFell;

		public void Update()
		{
			jumpSquatTime -= TennisFightingGame.DeltaTime;
			turningTime -= TennisFightingGame.DeltaTime;

			// Usually this isn't necessary but under some circumstances the player can
			// end up with a movement state other than idle or falling while attacking
			if (Attacking)
			{
				if (aerialState == AerialStates.Airborne)
				{
					movementState = MovementStates.Falling;
				}
				else
				{
					movementState = MovementStates.Idle;
				}
			}

			// If doing jump squat and jump squat time is done, jump
			if (aerialState == AerialStates.JumpSquat && jumpSquatTime <= 0)
			{
				if (Jumped != null)
				{
					Jumped.Invoke();
				}
			}

			// Check for exhaustion
			if (player.stamina < player.stats.exhaustedThreshold)
			{
				exhausted = true;
			}

			// Check for recovery from exhaustion
			if (player.stamina > player.stats.recoverThreshold && exhausted)
			{
				exhausted = false;
			}

			// Check if there's a bottom collision and decide aerialState based on that
			Wall[] walls = player.match.court.PlayerGeometry;
			bool bottom = false;

			for (int i = 0; i < walls.Length && !bottom; i++)
			{
				Rectangle check = new Rectangle(player.rectangle.X, player.rectangle.Y + Player.CheckDistance,
					player.rectangle.Width, player.rectangle.Height);

				bottom = walls[i].rectangle.Collision(check, player.lastRectangle).Bottom;
			}

			// Bottom collision and airborne means player just landed
			if (bottom && aerialState == AerialStates.Airborne)
			{
				if (Landed != null)
				{
					Landed.Invoke();
				}

				fastFell = false;

				movementState = MovementStates.Idle; // There's a reason for this
				aerialState = AerialStates.Standing;
			}

			// No bottom collision and jump squat is over means now airborne
			if (!bottom && jumpSquatTime <= 0)
			{
				aerialState = AerialStates.Airborne;
			}
		}

		private void Press(Actions action)
		{
			if (Attacking || Turning || !player.match.inPlay)
			{
				return;
			}

			// HACK This might need to be a switch...
			if (action == Actions.Jump && aerialState == AerialStates.Standing)
			{
				aerialState = AerialStates.JumpSquat;
				
				if (exhausted)
				{
					jumpSquatTime = player.stats.exhaustedJumpSquat;
				}
				else
				{
					jumpSquatTime = player.stats.jumpSquat;
				}

			}

			if (action == Actions.Turn && aerialState == AerialStates.Standing)
			{
				if (player.direction == 1)
				{
					movementState = MovementStates.TurningBackwards;
				}
				else
				{
					movementState = MovementStates.TurningForwards;
				}

				turningTime = player.stats.turnDelay;

				if (Turned != null)
				{
					Turned.Invoke();
				}
			}

			if (action == Actions.Down && aerialState == AerialStates.Airborne && 
				!fastFell && !Attacking && player.velocity.Y > 0)
			{
				fastFell = true;

				if (FastFell != null)
				{
					FastFell.Invoke();
				}
			}
		}

		private void Hold(Actions action)
		{
			// Walking
			if (Attacking || Turning || !player.match.inPlay)
			{
				return;
			}

			switch (action)
			{
				default:
					{
						if (aerialState == AerialStates.Airborne)
						{
							movementState = MovementStates.Falling;
						} 
						else
						{
							movementState = MovementStates.Idle;
						}
						break;
					}

				case Actions.Left:
					{
						if ((aerialState == AerialStates.Standing || aerialState == AerialStates.JumpSquat) &&
							(player.velocity.X < 0 || Math.Abs(player.velocity.X) < Player.FrictionRound))
						{
							if (exhausted)
							{
								movementState = MovementStates.CrawlingBackwards;
							}
							else
							{
								movementState = MovementStates.WalkingBackwards;
							}
						}

						if (aerialState == AerialStates.Airborne)
						{
							movementState = MovementStates.DriftingBackwards;
						}

						break;
					}

				case Actions.Right:
					{
						if ((aerialState == AerialStates.Standing || aerialState == AerialStates.JumpSquat) &&
							(player.velocity.X > 0 || Math.Abs(player.velocity.X) < Player.FrictionRound))
						{
							if (exhausted)
							{
								movementState = MovementStates.CrawlingForwards;
							}
							else
							{
								movementState = MovementStates.WalkingForwards;
							}
						}

						if (aerialState == AerialStates.Airborne)
						{
							movementState = MovementStates.DriftingForwards;
						}

						break;
					}
			}
		}

		private void Release(Actions action)
		{
			switch (action)
			{
				default:
					{
						if (aerialState == AerialStates.Airborne)
						{
							movementState = MovementStates.Falling;
						} 
						else
						{
							movementState = MovementStates.Idle;
						}
						break;
					}
			}
		}

		private void DoublePress(Actions action)
		{
			// Sprinting
			if (Attacking || Turning || !player.match.inPlay)
			{
				return;
			}
			
			switch (action)
			{
				default:
					{
						if (aerialState == AerialStates.Airborne)
						{
							movementState = MovementStates.Falling;
						} 
						else
						{
							movementState = MovementStates.Idle;
						}
						break;
					}

				case Actions.Left:
					{
						if (movementState != MovementStates.WalkingBackwards)
						{
							break;
						}

						movementState = MovementStates.SprintingBackwards;
						break;
					}

				case Actions.Right:
					{
						if (movementState != MovementStates.WalkingForwards)
						{
							break;
						}
						movementState = MovementStates.SprintingForwards;
						break;
					}
			}
		}
	}

	public enum MovementStates
	{
		Idle,
		Falling, // This one seems counter-intuitive but it's there for Sprite
		WalkingForwards,
		WalkingBackwards,
		SprintingForwards,
		SprintingBackwards,
		CrawlingForwards,
		CrawlingBackwards,
		DriftingForwards,
		DriftingBackwards,
		TurningForwards,
		TurningBackwards
	}

	public enum AerialStates
	{
		Airborne,
		JumpSquat,
		Standing,
		Grounded
	}
}
