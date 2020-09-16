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

		public MovementState movementState;
		public AerialStates aerialState;

		public bool serving;
		public bool exhausted;
		public bool fastFell;

		float jumpSquatTime;

		public StateManager(Player player)
		{
			this.player = player;

			player.input.Pressed += Press;
			player.input.Held += Hold;
			player.input.Released += Release;
			player.input.DoublePressed += DoublePress;
		}

		public bool Attacking { get { return player.moveset.currentAttack != null; } }
		public bool Walking
		{
			get
			{
				return movementState == MovementState.WalkingBackwards ||
					   movementState == MovementState.WalkingForwards;
			}
		}
		public bool Running
		{
			get
			{
				return movementState == MovementState.SprintingBackwards ||
					   movementState == MovementState.SprintingForwards;
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

			if (aerialState == AerialStates.JumpSquat && jumpSquatTime <= 0)
			{
				if (Jumped != null)
				{
					Jumped.Invoke();
				}
			}

			if (player.stamina < player.stats.exhaustedThreshold)
			{
				exhausted = true;
			}

			if (player.stamina > player.stats.recoverThreshold && exhausted)
			{
				exhausted = false;
			}

			// FIXME If floor isn't the first item in PlayerGeometry, shit doesn't work
			// HACK Jumping on top of things doesn't work all that well with the current setup
			// Handle AerialState and Landed
			foreach (Wall wall in player.match.court.PlayerGeometry)
			{
				Rectangle check = new Rectangle(player.rectangle.X, player.rectangle.Y + Player.CheckDistance,
					player.rectangle.Width, player.rectangle.Height);

				if (aerialState != AerialStates.JumpSquat && wall.Collision(check, player.lastRectangle).Bottom)
				{
					if (aerialState == AerialStates.Airborne)
					{
						if (Landed != null)
						{
							Landed.Invoke();
						}

						fastFell = false;
					}

					aerialState = AerialStates.Standing;
					return;
				}

				if (!wall.Collision(check, player.lastRectangle).Bottom && jumpSquatTime <= 0)
				{
					aerialState = AerialStates.Airborne;
					return;
				}
			}
		}

		private void Press(Actions action)
		{
			if (Attacking || !player.match.inPlay)
			{
				return;
			}

			if (action == Actions.Jump && aerialState == AerialStates.Standing)
			{
				aerialState = AerialStates.JumpSquat;
				
				jumpSquatTime = player.stats.jumpSquat;

			}

			if (action == Actions.Turn && aerialState == AerialStates.Standing)
			{
				if (player.direction == 1)
				{
					movementState = MovementState.TurningBackwards;
				}
				else
				{
					movementState = MovementState.TurningForwards;
				}

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
			if (Attacking || !player.match.inPlay)
			{
				movementState = MovementState.Idle;
				return;
			}

			switch (action)
			{
				case Actions.Left:
					{
						if (aerialState == AerialStates.Standing || aerialState == AerialStates.JumpSquat)
						{
							if (exhausted)
							{
								movementState = MovementState.CrawlingBackwards;
							}
							else
							{
								movementState = MovementState.WalkingBackwards;
							}
						}

						if (aerialState == AerialStates.Airborne)
						{
							movementState = MovementState.DriftingBackwards;
						}

						break;
					}

				case Actions.Right:
					{
						if (aerialState == AerialStates.Standing || aerialState == AerialStates.JumpSquat)
						{
							if (exhausted)
							{
								movementState = MovementState.CrawlingForwards;
							}
							else
							{
								movementState = MovementState.WalkingForwards;
							}
						}

						if (aerialState == AerialStates.Airborne)
						{
							movementState = MovementState.DriftingForwards;
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
						movementState = MovementState.Idle;
						break;
					}
			}
		}

		private void DoublePress(Actions action)
		{
			// Sprinting
			if (Attacking || !player.match.inPlay)
			{
				return;
			}
			switch (action)
			{
				default:
					{
						movementState = MovementState.Idle;
						break;
					}
				case Actions.Left:
					{
						if (movementState != MovementState.WalkingBackwards)
						{
							break;
						}

						movementState = MovementState.SprintingBackwards;
						break;
					}

				case Actions.Right:
					{
						if (movementState != MovementState.WalkingForwards)
						{
							break;
						}
						movementState = MovementState.SprintingForwards;
						break;
					}
			}
		}
	}

	public enum MovementState
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