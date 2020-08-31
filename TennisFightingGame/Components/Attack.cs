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
		private readonly Hitbox[] hitboxes;
		public readonly SoundEffect[][] onStartupSounds;
		public readonly bool isNull;
        public readonly float staminaCost; // stamina cost
		public readonly bool hardLandCancel;
		public readonly bool softLandCancel;
		public readonly bool hardHitCancel;
		public readonly bool softHitCancel;
		public readonly bool multiHit;
		public readonly bool disabledWhenExhausted;
		private readonly float startup;
		public readonly float endlag;

		public List<Hitbox> activeHitboxes = new List<Hitbox>();
		public List<Hitbox> hits = new List<Hitbox>();
        public float time;
        private bool swing;

		// No-attack attack
		public Attack()
		{
			isNull = true;
		}

		public Attack(Hitbox[] hitboxes = null, SoundEffect[][] onStartupSounds = null,
			float startup = 0,  float endlag = 0, bool hardLandCancel = false, 
			bool disabledWhenExhausted = false, bool softLandCancel = false, bool hardHitCancel = false, 
			bool softHitCancel = false, bool multiHit = false, float staminaCost = 0)
		{
			this.hitboxes = hitboxes;
			this.onStartupSounds = onStartupSounds;
			this.startup = startup;
			this.endlag = endlag;
			this.hardLandCancel = hardLandCancel;
			this.softLandCancel = softLandCancel;
			this.hardHitCancel = hardHitCancel;
			this.softHitCancel = softHitCancel;
			this.multiHit = multiHit;
			this.staminaCost = staminaCost;
			this.disabledWhenExhausted = disabledWhenExhausted;
        }

		public float TotalDuration
        {
            get
            {
                Hitbox last = hitboxes.OrderBy(hitbox => hitbox.start + hitbox.duration).Last();
                return startup + last.start + last.duration + endlag; // lazy linq
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
		public event SwungEventHandler Swung;
		public event FinishedEventHandler Finished;
		
		public Attack(Attack copying)
		{
			isNull = copying.isNull;
			hitboxes = copying.hitboxes;
			onStartupSounds = copying.onStartupSounds;
			startup = copying.startup;
			endlag = copying.endlag;
			hardLandCancel = copying.hardLandCancel;
			softLandCancel = copying.softLandCancel;
			hardHitCancel = copying.hardHitCancel;
			softHitCancel = copying.softHitCancel;
			multiHit = copying.multiHit;
			staminaCost = copying.staminaCost;
			disabledWhenExhausted = copying.disabledWhenExhausted;
		}
		
        public void Update(Player player)
        {
			time += Game1.DeltaTime;

			if (time >= startup && !swing)
			{
				if (Swung != null)
				{
					Swung.Invoke();
				}

				swing = true;
			}

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
					if (softHitCancel && time < TotalDuration - endlag)
					{
						time = TotalDuration - endlag;
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
			if (isNull)
			{
				return;
			}

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
			swing = true;

			if (Finished != null)
			{
				Finished.Invoke();
			}
		}

		// TODO Rectangle for hitbox
		private Rectangle RelativeRectangle(Hitbox hitbox, Rectangle rectangle, int side)
        {
            return new Rectangle(
                rectangle.Center.X - hitbox.rectangle.Width / 2 + hitbox.rectangle.X * -side,
                rectangle.Center.Y - hitbox.rectangle.Height / 2 + hitbox.rectangle.Y,
                hitbox.rectangle.Width,
                hitbox.rectangle.Height
            );
        }
	}
}