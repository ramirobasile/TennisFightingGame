using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TennisFightingGame
{
	/* A character is a "blueprint" for a Player, an uninstantiated version of the Player class. */
    public class Character
    {
        public string name;
        public Rectangle rectangle;
        public Texture2D spriteSheet;
        public Animation[] animations;
        public Attack[] attacks;
        public Stats stats;

        public Character(string name, Rectangle rectangle, Texture2D spriteSheet, 
            Animation[] animations, Attack[] attacks, Stats stats)
        {
            this.name = name;
            this.rectangle = rectangle;
            this.spriteSheet = spriteSheet;
            this.animations = animations;
            this.attacks = attacks;
            this.stats = stats;
        }

        // For serialization
        private Character()
        {
        }
    }
}