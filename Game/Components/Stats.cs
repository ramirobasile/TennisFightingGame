﻿namespace TennisFightingGame
{
	/// <summary>
	/// Stats for a Player.Character.
	/// </summary>
	public struct Stats
	{
		public const float NormalGravity = 4500;

		public readonly float gravity;
		public readonly float friction;
		public readonly float staminaRegen;
		public readonly float runSpeed;
		public readonly float runStaminaCost;
		public readonly float runningJumpSpeed;
		public readonly float walkSpeed;
		public readonly float walkStaminaCost;
		public readonly float jumpSpeed;
		public readonly float jumpStaminaCost;
		public readonly float driftSpeed;
		public readonly float exhaustedSpeed;
		public readonly float exhaustedJumpSpeed;
		public readonly float exhaustedJumpStaminaCost;
		public readonly float exhaustedThreshold;
		public readonly float recoverThreshold;
		public readonly float fastFallSpeed;

		public Stats(float gravity, float friction, float staminaRegen, float runSpeed, float runStaminaCost, 
			float runningJumpSpeed, float walkSpeed, float walkStaminaCost, float jumpSpeed, 
			float jumpStaminaCost, float driftSpeed, float exhaustedSpeed, float exhaustedJumpSpeed, 
			float exhaustedJumpStaminaCost, float exhaustedThreshold, float recoverThreshold,
			float fastFallSpeed)
		{
			this.gravity = gravity;
			this.friction = friction;
			this.staminaRegen = staminaRegen;
			this.runSpeed = runSpeed;
			this.runStaminaCost = runStaminaCost;
			this.runningJumpSpeed = runningJumpSpeed;
			this.walkSpeed = walkSpeed;
			this.walkStaminaCost = walkStaminaCost;
			this.jumpSpeed = jumpSpeed;
			this.jumpStaminaCost = jumpStaminaCost;
			this.driftSpeed = driftSpeed;
			this.exhaustedSpeed = exhaustedSpeed;
			this.exhaustedJumpSpeed = exhaustedJumpSpeed;
			this.exhaustedJumpStaminaCost = exhaustedJumpStaminaCost;
			this.exhaustedThreshold = exhaustedThreshold;
			this.recoverThreshold = recoverThreshold;
			this.fastFallSpeed = fastFallSpeed;
		}
	}
}