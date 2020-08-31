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
		public AerialState aerialState;

		public bool serving;
		public bool exhausted;

		public StateManager(Player player)
		{
			this.player = player;

			player.input.Pressed += Press;
			player.input.Held += Hold;
			player.input.Released += Release;
			player.input.DoublePressed += DoublePress;
		}

		public Attacks CurrentAttack { get { return player.moveset.currentAttack; } }
		public bool Attacking { get { return CurrentAttack != Attacks.None; } }
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
		public delegate void LandedEventHandler();

		public event JumpedEventHandler Jumped;
		public event LandedEventHandler Landed;

		public void Update()
		{
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

				if (aerialState != AerialState.Jumping && wall.Collision(check, player.lastRectangle).Bottom)
				{
					if (aerialState == AerialState.Airborne)
					{
						if (Landed != null)
						{
							Landed.Invoke();
						}
					}

					aerialState = AerialState.Standing;
					return;
				}

				if (!wall.Collision(check, player.lastRectangle).Bottom)
				{
					aerialState = AerialState.Airborne;
					return;
				}
			}
		}

		private void Press(Action action)
		{
			if (Attacking)
			{
				return;
			}

			if (action == Action.Jump)
			{
				if (aerialState == AerialState.Standing)
				{
					aerialState = AerialState.Jumping;
					if (Jumped != null)
					{
						Jumped.Invoke();
					}
				}
			}

		}

		private void Hold(Action action)
		{
			switch (action)
			{
				default:
					{
						movementState = MovementState.Idle;
						break;
					}
				case Action.Left:
					{
						if ((aerialState == AerialState.Standing || aerialState == AerialState.Jumping) && !Attacking)
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

						if (aerialState == AerialState.Airborne)
						{
							movementState = MovementState.DriftingBackwards;
						}

						break;
					}

				case Action.Right:
					{
						if ((aerialState == AerialState.Standing || aerialState == AerialState.Jumping) && !Attacking)
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

						if (aerialState == AerialState.Airborne)
						{
							movementState = MovementState.DriftingForwards;
						}

						break;
					}
			}
		}

		private void Release(Action action)
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

		private void DoublePress(Action action)
		{
			switch (action)
			{
				default:
					{
						movementState = MovementState.Idle;
						break;
					}
				case Action.Left:
					{
						if (movementState != MovementState.WalkingBackwards)
						{
							break;
						}

						movementState = MovementState.SprintingBackwards;
						break;
					}

				case Action.Right:
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
		DriftingBackwards
	}

	public enum AerialState
	{
		Airborne,
		Jumping,
		Standing,
		Grounded
	}
}