using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace TennisFightingGame
{
	/* Extensions and helper methods */
    public static class Helpers
	{
		// HACK This should definitely not be here
		public static SoundEffectInstance PlaySFX(SoundEffect sound, float volume = 1, float pitch = 0, 
			float pan = 0, bool loops = false)
		{
			SoundEffectInstance instance = sound.CreateInstance();

			if (!TennisFightingGame.ConfigFile.Boolean("Sound", "Mute"))
			{
				instance.Volume = volume * TennisFightingGame.ConfigFile.Number("Sound", "SFXVolume") / 100;
			}
			else
			{
				instance.Volume = 0;
			}

			instance.Pitch = pitch;

			if (!TennisFightingGame.ConfigFile.Boolean("Sound", "Mono"))
			{
				instance.Pan = pan;
			}
			else
			{
				instance.Pan = 0;
			}

			instance.IsLooped = loops;

			instance.Play();

			return instance;
		}

		public static SoundEffectInstance PlayRandomSFX(SoundEffect[] sounds, float volume = 1, 
			float pitch = 1, float pan = 0)
		{
			if (sounds == null || sounds.Length == 0)
			{
				return null;
			}

			int index = TennisFightingGame.Random.Next(0, sounds.Length);
			return PlaySFX(sounds[index]);
		}
		
		public static SoundEffectInstance[] PlayRandomSFX(SoundEffect[][] sounds, float volume = 1, 
			float pitch = 1, float pan = 0)
		{
			if (sounds == null || sounds.Length == 0)
			{
				return null;
			}

			SoundEffectInstance[] instances = new SoundEffectInstance[sounds.Length];
			for (int i = 0; i < sounds.Length; i++)
			{
				int index = TennisFightingGame.Random.Next(0, sounds[i].Length);
				instances[i] = PlaySFX(sounds[i][index]);
			}
			
			return instances;
		}

		public static int Wrap(int number, int min, int max)
		{
			int range = max - min + 1;

			if (number < min)
			{
				number += range * ((min - number) / range + 1);
			}

			return min + (number - min) % range;
		}

		// Center point of the viewport
		public static Point Middle(this Viewport viewport)
        {
            return new Point(viewport.Width / 2, viewport.Height / 2);
        }

        // Centers a point within a viewport
        public static Point Center(this Viewport viewport, Point point)
        {
            return new Point(viewport.Width / 2 - point.X / 2, viewport.Height / 2 - point.Y / 2);
        }

		public static int Height(this SpriteFont font)
		{
			return (int)font.MeasureString("_").Y;
		}

		public static Point CenterTextHorizontally(string text, int y, SpriteFont font)
		{
			Viewport viewport = TennisFightingGame.Graphics.GraphicsDevice.Viewport;
			Point centered = new Point((int)(viewport.Width / 2 - font.MeasureString(text).X / 2), y);

			return centered;
		}
    }
}