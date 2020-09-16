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
		private const float MaxStaminaCost = 50;
		private const float VelDiffScalar = 0.0133f; // scalar of the difference in velocity to calculate hit stamina
		private const float NormalHitThreshold = 500;
		private const float StrongHitThreshold = 2500;
		private const float PanningScalar = 0.3f;
		private const float VelocityAcumulationScalar = 0.15f;

		public readonly Attack[] attacks;

		public Player player;
		public Attack currentAttack;

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
				this.attacks[i].Finished += () => { currentAttack = null; };
			}

			player.input.Pressed += Press;
			player.state.Landed += LandCancel;
		}

		public delegate void ThrewAttackEventHandler(Attack attack);
		public delegate void ServedEventHandler(Player player);

		public event ThrewAttackEventHandler ThrewAttack;
		public event ServedEventHandler Served;

		public void Update()
		{
			if (currentAttack != null)
			{
				currentAttack.Update(player);
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			if (TennisFightingGame.ConfigFile.Boolean("Debug", "Hitboxes"))
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
		public void Throw(Attack attack)
		{
			if (!attack.disabledWhenExhausted)
			{
				currentAttack = attack;
				player.AddStamina(-attack.staminaCost);

				Helpers.PlayRandomSFX(attack.onStartupSounds);

				if (ThrewAttack != null)
				{
					ThrewAttack.Invoke(attack);
				}

				if (attack.serve)
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
				player.match.ball.Hit(new Vector2(hitbox.force.X * player.direction, 
					hitbox.force.Y), 
					hitbox.gravity);
			}
			else
			{
				player.match.ball.Hit(new Vector2(hitbox.exhaustedForce.X * player.direction, 
					hitbox.exhaustedForce.Y), 
					hitbox.gravity);
			}

			// Stamina cost on hit is based on the difference in velocity of the ball before and after hit
			player.AddStamina(-MathHelper.Clamp(VelDiffScalar *
				(Math.Abs(originalVelocity.X) - 
				Math.Abs(player.match.ball.velocity.X)), 0, MaxStaminaCost));

			if (hitbox.cumulative && Math.Sign(player.match.ball.velocity.X) == Math.Sign(originalVelocity.X))
			{
				player.match.ball.velocity += new Vector2(originalVelocity.X * VelocityAcumulationScalar, 0);
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
			if (currentAttack != null)
			{
				currentAttack.Cancel();
			}
		}

		/// <summary>
		/// Cancels attack on land. If the attack is "soft" land-cancellable, the time is set to the
		/// begining of the endlag (unless the attack is already in endlag). A "hard" land cancel will
		/// instatly end the attack.
		/// </summary>
		private void LandCancel()
		{
			if (currentAttack == null)
			{
				return;
			}

			// Soft land cancel
			if (currentAttack.softLandCancel && 
				currentAttack.time < currentAttack.TotalDuration - currentAttack.recovery)
			{
				currentAttack.activeHitboxes.Clear();
				currentAttack.time = currentAttack.TotalDuration - currentAttack.recovery;
			}

			// Hard land cancel
			if (currentAttack.hardLandCancel)
			{
				currentAttack.Cancel();
			}
		}

		/// <summary>
		/// Handle throwing attacks when buttons corresponding to attacks are pressed.
		/// </summary>
		/// <param name="action">Action pressed. See InputManager.</param>
		private void Press(Actions action)
		{
			// Attacks can only be thrown if current is null and it's your turn
			// to serve in case the match isn't inPlay
			if (currentAttack != null || (!player.match.inPlay && !player.state.serving))
			{
				return;
			}

			foreach (Attack attack in attacks)
			{
				// TODO Explain and cleanup
				AerialStates simplifiedAerialState = player.state.aerialState;
				if (player.state.aerialState == AerialStates.JumpSquat)
				{
					simplifiedAerialState = AerialStates.Airborne;
				}
				
				if (attack.action == action &&
					attack.aerialState == simplifiedAerialState &&
					(attack.serve == player.state.serving) &&
					player.input.MotionInput(attack.motionInput))
				{
					Throw(attack);
					break;
				}
			}
		}
	}
}
