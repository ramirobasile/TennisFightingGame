using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace TennisFightingGame
{
	/// <summary>
	/// Handles throwing, updating and cancelling attacks and all of their related processes, such as
	/// playing their associated sounds, changing the velocity of the ball when an attack connects, etc.
	/// </summary>
	public class Moveset
	{
		private const float MaxStaminaCost = 30;
		private const float VelDiffScalar = 0.017f; // scalar of the difference in velocity to calculate hit stamina
		private const float NormalHitThreshold = 500;
		private const float StrongHitThreshold = 2500;
		private const float PanningScalar = 0.3f;
		private const float VelocityAcumulationScalar = 0.15f;

		public readonly Attack[] attacks;

		public Player player;
		public Attacks currentAttack = Attacks.None;

		public Moveset(Attack[] attacks, Player player)
		{
			this.player = player;

			// Attacks need to be copied by value to avoid conflicting attack references on mirror matches
			this.attacks = new Attack[attacks.Length];
			for (int i = 0; i < this.attacks.Length; i++)
			{
				this.attacks[i] = new Attack(attacks[i]); 
				this.attacks[i].Hit += Hit;
				this.attacks[i].AddedHitbox += AddedHitbox;
				this.attacks[i].Finished += () => { currentAttack = Attacks.None; };
			}

			player.input.Pressed += Press;
			player.state.Landed += LandCancel;
		}

		public delegate void ThrewAttackEventHandler(Attacks attack);
		public delegate void ServedEventHandler(Player player);

		public event ThrewAttackEventHandler ThrewAttack;
		public event ServedEventHandler Served;

		private Attack CurrentAttack { get { return attacks[(int)currentAttack]; } }

		public void Update()
		{
			if (currentAttack != Attacks.None && !CurrentAttack.isNull)
			{
				CurrentAttack.Update(player);
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			if (Game1.ConfigFile.Boolean("Debug", "Hitboxes"))
			{
				foreach (Attack attack in attacks)
				{
					attack.Draw(spriteBatch, player);
				}
			}
		}

		/// <summary>
		/// Set new attack and do its sounds.
		/// </summary>
		public void Throw(Attacks attack)
		{
			Attack newAttack = attacks[(int)attack];

			if (!newAttack.isNull && !newAttack.disabledWhenExhausted)
			{
				currentAttack = attack;
				player.AddStamina(-newAttack.staminaCost);

				Helpers.PlayRandomSFX(newAttack.onStartupSounds);

				if (ThrewAttack != null)
				{
					ThrewAttack.Invoke(attack);
				}

				if (attack == Attacks.Serve1 || attack == Attacks.Serve2 || attack == Attacks.Serve3)
				{
					if (Served != null)
					{
						Served.Invoke(player);
					}
				}
			}
		}

		/// <summary>
		/// Called when an attack's hit event is invoked, which happens when once of its hitboxes
		/// intersects with the ball.
		/// The velocity of the ball is set to the attacks force and, if the ball' was going the same
		/// way the attack was going to push it to, then their velocities are added, with the original
		/// velocity scaled down by a constant.
		/// Additionally, a sound with semi-random pitch and volume, dependant on the ball's new velocity
		/// will be played and the player will be taxed the corrsponding stamina for taking on the ball
		/// with a hitbox (the bigger the diference between the hitbox's force and the balls original
		/// velocity, the grater the stamina cost).
		/// </summary>
		/// <param name="hitbox">Hitbox.</param>
		private void Hit(Hitbox hitbox)
		{
			Vector2 originalVelocity = player.match.ball.velocity;

			if (!player.state.exhausted)
			{
				player.match.ball.Hit(new Vector2(hitbox.force.X * -player.direction, hitbox.force.Y), hitbox.gravityScalar);
			}
			else
			{
				player.match.ball.Hit(new Vector2(hitbox.exhaustedForce.X * -player.direction, 
					hitbox.exhaustedForce.Y), hitbox.gravityScalar);
			}

			// Stamina cost on hit is based on the difference in velocity of the ball before and after hit
			player.AddStamina(-MathHelper.Clamp(VelDiffScalar *
				(Math.Abs(originalVelocity.X) - 
				Math.Abs(player.match.ball.velocity.X)), 0, MaxStaminaCost));

			if (hitbox.cumulative && Math.Sign(player.match.ball.velocity.X) == Math.Sign(originalVelocity.X))
			{
				player.match.ball.AddVelocity(new Vector2(originalVelocity.X * VelocityAcumulationScalar, 0));
			}

			// Hitstun, hitlag and camera shake
			player.match.camera.shake = hitbox.hitStun;
			player.match.camera.shakeMagnitude = hitbox.shakeMagnitude;
			player.hitLag = hitbox.hitLag;
			player.match.ball.hitStun = hitbox.hitStun;

			// Hit sound
			float pan = player.courtSide * PanningScalar; // Pan the sound slightly towards the hitter's side
			Helpers.PlayRandomSFX(hitbox.onHitSounds, pan: pan);
		}

		private void AddedHitbox(Hitbox hitbox)
		{
			Helpers.PlayRandomSFX(hitbox.onAddedSounds);
		}

		/// <summary>
		/// Used by other classes to forcibly cancel attacks, like when a round ends.
		/// </summary>
		public void CancelCurrentAttack()
		{
			if (currentAttack != Attacks.None)
			{
				CurrentAttack.Cancel();
			}
		}

		/// <summary>
		/// Cancels attack on land. If the attack is "soft" land-cancellable, the time is set to the
		/// begining of the endlag (unless the attack is already in endlag). A "hard" land cancel will
		/// instatly end the attack.
		/// </summary>
		private void LandCancel()
		{
			if (currentAttack == Attacks.None)
			{
				return;
			}

			// Soft land cancel
			if (CurrentAttack.softLandCancel && CurrentAttack.time < CurrentAttack.TotalDuration - CurrentAttack.endlag)
			{
				CurrentAttack.activeHitboxes.Clear();
				CurrentAttack.time = CurrentAttack.TotalDuration - CurrentAttack.endlag;
			}

			// Hard land cancel
			if (CurrentAttack.hardLandCancel)
			{
				CurrentAttack.Cancel();
			}
		}

		/// <summary>
		/// Handle throwing attacks when buttons corresponding to attacks are pressed.
		/// </summary>
		/// <param name="action">Action pressed. See InputManager.</param>
		private void Press(Action action)
		{
			if (currentAttack != Attacks.None)
			{
				return;
			}

			if (player.state.serving)
			{
				switch (action)
				{
					case Action.Attack1:
						{
							Throw(Attacks.Serve1);
							break;
						}
					case Action.Attack2:
						{
							Throw(Attacks.Serve2);
							break;
						}
					case Action.Attack3:
						{
							Throw(Attacks.Serve3);
							break;
						}
				}

				return;
			}
			if (player.match.inPlay)
			{
				switch (action)
				{
					case Action.Attack1:
						{
							if (player.state.aerialState == AerialState.Standing)
							{
								Throw(Attacks.Attack1);
							}

							if (player.state.aerialState == AerialState.Airborne)
							{
								Throw(Attacks.Attack1Air);
							}

							break;
						}
					case Action.Attack2:
						{
							if (player.state.aerialState == AerialState.Standing)
							{
								Throw(Attacks.Attack2);
							}

							if (player.state.aerialState == AerialState.Airborne)
							{
								Throw(Attacks.Attack2Air);
							}

							break;
						}
					case Action.Attack3:
						{
							if (player.state.aerialState == AerialState.Standing)
							{
								Throw(Attacks.Attack3);
							}

							if (player.state.aerialState == AerialState.Airborne)
							{
								Throw(Attacks.Attack3Air);
							}

							break;
						}
				}
			}
		}
	}

	public enum Attacks
	{
		Attack1,
		Attack2,
		Attack3,
		Attack1Air,
		Attack2Air,
		Attack3Air,
		Serve1,
		Serve2,
		Serve3,
		None
	}
}
