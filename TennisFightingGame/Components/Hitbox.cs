using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace TennisFightingGame
{
    public class Hitbox
    {
        public readonly Rectangle rectangle;
        public readonly float start; //from A in the total duration of the attack
        public readonly float duration; //to B, relatively
        public readonly Vector2 force;
		public readonly Vector2 exhaustedForce; // Alternative force when exhausted
		public readonly float hitStun;
		public readonly float hitLag;
		public readonly int shakeMagnitude;
		public readonly bool cumulative; // TODO Explain
		public readonly Polynomial gravityScalar; // TODO Explain
		// A random sound effect of each SoundEffect array will be played
		public readonly SoundEffect[][] onAddedSounds;
		public readonly SoundEffect[] onHitSounds;
		
		public Hitbox(Rectangle rectangle, float start, float duration, Vector2 force, 
			Vector2 exhaustedForce = default(Vector2), Polynomial gravityScalar = default(Polynomial), 
			float hitStun = 0,  float hitLag = 0,  int shakeMagnitude = 10, bool cumulative = true,
			SoundEffect[][] onAddedSounds = null, SoundEffect[] onHitSounds = null)
        {
            this.rectangle = rectangle;
            this.duration = duration;
            this.start = start;
			this.force = force;
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

			// HACK This is even worse I feel
			if (gravityScalar.Equals(default(Polynomial)))
			{
				this.gravityScalar = Polynomial.Identity;
			}
			else
			{
				this.gravityScalar = gravityScalar;
			}
		}

        public void Draw(SpriteBatch spriteBatch, Texture2D placeholder, Rectangle relativeRectangle)
        {
            spriteBatch.Draw(placeholder, relativeRectangle, Color.Red * .5f);
        }
    }
}