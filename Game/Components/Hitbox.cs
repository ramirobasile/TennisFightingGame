using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace TennisFightingGame
{
    public class Hitbox
    {
		// TODO Better way of documenting this
        public Rectangle rectangle; // Hitbox bounds
        public float start; //from A in the total duration of the attack
        public float duration; //to B, relatively
        public Vector2 force; // Force with which the ball is hit
		public Vector2 exhaustedForce; // Alternative force when exhausted
		public float hitStun; // Time the ball is frozen for
		public float hitLag; // Time tha player is frozen for
		public int shakeMagnitude; // Screen shake magnitude during hitStun
		public bool cumulative; // Hits on the same direction accumulate (scaled down by a const)
		public float gravity; // Custom gravity
		// A random sound effect of each SoundEffect array will be played
		[NonSerialized] public SoundEffect[][] onAddedSounds;
		[NonSerialized] public SoundEffect[] onHitSounds;
		
		public Hitbox(Rectangle rectangle, float start, float duration, Vector2 force, 
			Vector2 exhaustedForce = default(Vector2), float gravity = Ball.DefaultGravity,
			float hitStun = 0,  float hitLag = 0,  int shakeMagnitude = 0, 
			bool cumulative = true, SoundEffect[][] onAddedSounds = null, 
			SoundEffect[] onHitSounds = null)
        {
            this.rectangle = rectangle;
            this.duration = duration;
            this.start = start;
			this.force = force;
			this.gravity = gravity;
			this.hitStun = hitStun;
			this.hitLag = hitLag;
			this.shakeMagnitude = shakeMagnitude;
			this.cumulative = cumulative;
			this.onAddedSounds = onAddedSounds;
			this.onHitSounds = onHitSounds;

			// HACK If Vector2 were nullable, things wouldn't have to be so ugly
			// exhaustedForce will be the same as force unless it was set
			if (exhaustedForce == default(Vector2))
			{
				this.exhaustedForce = force;
			}
			else
			{
				this.exhaustedForce = exhaustedForce;
			}
		}

		// For serialization
		public Hitbox()
		{
		}

        public void Draw(SpriteBatch spriteBatch, Texture2D placeholder, Rectangle relativeRectangle)
        {
			// Debug
            spriteBatch.Draw(placeholder, relativeRectangle, Color.Red * .5f);
        }
    }
}