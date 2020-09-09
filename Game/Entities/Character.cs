using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace TennisFightingGame
{
    public class Character
    {
        public string name;
        public Rectangle rectangle;
        [NonSerialized] public Texture2D spriteSheet;
        [NonSerialized] public SoundEffect stepSound; // HACK
        [NonSerialized] public SoundEffect jumpSound; // HACK
        [NonSerialized] public SoundEffect turnSound; // HACK
        public Animation[] animations;
        public Attack[] attacks;
        public Stats stats;

        public Character(string name, Rectangle rectangle, Texture2D spriteSheet, 
            Animation[] animations, Attack[] attacks, Stats stats, SoundEffect stepSound,
            SoundEffect jumpSound, SoundEffect turnSound)
        {
            this.name = name;
            this.rectangle = rectangle;
            this.spriteSheet = spriteSheet;
            this.animations = animations;
            this.attacks = attacks;
            this.stats = stats;
            this.stepSound = stepSound;
            this.jumpSound = jumpSound;
            this.turnSound = turnSound;
        }

        // For serialization
        private Character()
        {
        }
    }
}