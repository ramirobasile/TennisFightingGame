using Microsoft.Xna.Framework;

namespace TennisFightingGame
{
	/* A character is a "blueprint" for a Player, an uninstantiated version of the Player class. */

    public struct Character
    {
        public string name;
        public Rectangle rectangle;
        public Sprite[] sprites;
        public Attack[] attacks;

        public Character(string name, Rectangle rectangle, Sprite[] sprites, Attack[] attacks)
        {
            this.name = name;
            this.rectangle = rectangle;
            this.sprites = sprites;
            this.attacks = attacks;
        }
    }
}