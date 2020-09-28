using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace TennisFightingGame
{
	/// <summary>
	/// Attack.
	/// </summary>
    public class Attack
	{
		public Actions action;
		public AerialStates aerialState;
		public Actions[] motionInput;
		public float chargeTime;
		public bool serve;
		public bool exhaused;

		public Hitbox[] hitboxes;
		[NonSerialized] public SoundEffect[][] onStartupSounds;
        public float staminaCost;
		public bool hardLandCancel;
		public bool softLandCancel;
		public bool hardHitCancel;
		public bool softHitCancel;
		public bool multiHit;
		public float startup;
		public float recovery;

		public List<Hitbox> activeHitboxes = new List<Hitbox>();
		public List<Hitbox> hits = new List<Hitbox>();
        public float time;

		public Attack(Actions action = Actions.Light, AerialStates aerialState = AerialStates.Grounded, 
			Actions[] motionInput = null, float chargeTime = 0, bool serve = false, bool exhausted = false,
			Hitbox[] hitboxes = null,  SoundEffect[][] onStartupSounds = null,
			float startup = 0,  float recovery = 0, bool hardLandCancel = false,
			bool softLandCancel = false, bool hardHitCancel = false, bool softHitCancel = false,
			bool multiHit = false, float staminaCost = 0)
		{
			this.action = action;
			this.aerialState = aerialState;
			this.motionInput = motionInput;
			this.chargeTime = chargeTime;
			this.serve = serve;
			this.exhaused = exhausted;
			this.hitboxes = hitboxes;
			this.onStartupSounds = onStartupSounds;
			this.startup = startup;
			this.recovery = recovery;
			this.hardLandCancel = hardLandCancel;
			this.softLandCancel = softLandCancel;
			this.hardHitCancel = hardHitCancel;
			this.softHitCancel = softHitCancel;
			this.multiHit = multiHit;
			this.staminaCost = staminaCost;
        }

		// Deep copy
		public Attack(Attack copying)
		{
			action = copying.action;
			aerialState = copying.aerialState;
			motionInput = copying.motionInput;
			chargeTime = copying.chargeTime;
			serve = copying.serve;
			exhaused = copying.exhaused;
			hitboxes = copying.hitboxes;
			onStartupSounds = copying.onStartupSounds;
			startup = copying.startup;
			recovery = copying.recovery;
			hardLandCancel = copying.hardLandCancel;
			softLandCancel = copying.softLandCancel;
			hardHitCancel = copying.hardHitCancel;
			softHitCancel = copying.softHitCancel;
			multiHit = copying.multiHit;
			staminaCost = copying.staminaCost;
		}

        // For serialization
		private Attack()
		{
		}

		public float TotalDuration
        {
            get
            {
                Hitbox last = hitboxes.OrderBy(hitbox => hitbox.start + hitbox.duration).Last();
                return startup + last.start + last.duration + recovery; // lazy linq
            }
        }

		public delegate void HitEventHandler(Hitbox hitbox);
		public delegate void SwungEventHandler();
		public delegate void AddedHitboxEventHandler(Hitbox hitbox);
		public delegate void RemovedHitboxEventHandler(Hitbox hitbox);
		public delegate void FinishedEventHandler();

		public event HitEventHandler Hit;
		public event AddedHitboxEventHandler AddedHitbox;
		public event RemovedHitboxEventHandler RemovedHitbox;
		public event FinishedEventHandler Finished;
		
		
        public void Update(Player player)
        {
			time += TennisFightingGame.DeltaTime;

            // Activate hitboxes after delays and deactivate them after duration
            foreach (Hitbox hitbox in hitboxes)
            {
                if (time > startup + hitbox.start &&
					time < (startup + hitbox.start + hitbox.duration) && // this isn't redundant, it prevents infinite AddedHitbox invokes
					!activeHitboxes.Contains(hitbox))
                {
                    activeHitboxes.Add(hitbox); // hitbox activated
                    
                    if (AddedHitbox != null)
                    {
	                    AddedHitbox.Invoke(hitbox);
                    }
                }

                if (time > (startup + hitbox.start + hitbox.duration) && activeHitboxes.Contains(hitbox))
                {
                    activeHitboxes.Remove(hitbox); // hitbox deactivated
                    
                    if (RemovedHitbox != null)
                    {
	                    RemovedHitbox.Invoke(hitbox);
                    }
                }

                if (time > TotalDuration)
                {
					Cancel();
                    return;
                }
            }

            // Hit ball
			foreach (Hitbox hitbox in activeHitboxes)
			{
				if ((!hits.Any() || (multiHit && !hits.Contains(hitbox))) && 
				    RelativeRectangle(hitbox, player.rectangle, player.direction)
						.Intersects(player.match.ball.rectangle))
				{				
					hits.Add(hitbox);

					if (Hit != null)
					{
						Hit.Invoke(hitbox);
					}

					// Soft hit cancel
					if (softHitCancel && time < TotalDuration - recovery)
					{
						time = TotalDuration - recovery;
					}

					// Hard hit cancel
					if (hardHitCancel)
					{
						Cancel();
					}

					return;
				}
            }
		}

		public void Draw(SpriteBatch spriteBatch, Player player)
		{
			foreach (Hitbox hitbox in activeHitboxes)
			{
				hitbox.Draw(spriteBatch, Assets.PlaceholderTexture,
					RelativeRectangle(hitbox, player.rectangle, player.direction));
			}
		}

		public void Cancel()
		{
			time = 0;
			activeHitboxes.Clear();
			hits.Clear();

			if (Finished != null)
			{
				Finished.Invoke();
			}
		}

		// TODO Rectangle for hitbox
		private Rectangle RelativeRectangle(Hitbox hitbox, Rectangle rectangle, int direction)
        {
            return new Rectangle(
                rectangle.Center.X - hitbox.rectangle.Width / 2 + hitbox.rectangle.X * direction,
                rectangle.Center.Y - hitbox.rectangle.Height / 2 + hitbox.rectangle.Y,
                hitbox.rectangle.Width,
                hitbox.rectangle.Height
            );
        }
	}
}
