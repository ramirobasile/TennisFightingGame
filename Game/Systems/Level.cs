using Microsoft.Xna.Framework.Graphics;

namespace TennisFightingGame
{
	/// <summary>
	/// Base class for everithing, basically.
	/// </summary>
    public abstract class Level
    {
        public virtual void Update()
        {
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}