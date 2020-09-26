using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace TennisFightingGame
{
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

		public static Color RandomColor(Color minColor, Color maxColor)
		{
			int red = TennisFightingGame.Random.Next(minColor.R, maxColor.R);
			int green = TennisFightingGame.Random.Next(minColor.G, maxColor.G);
			int blue = TennisFightingGame.Random.Next(minColor.B, maxColor.B);
			int alpha = TennisFightingGame.Random.Next(minColor.A, maxColor.A);

			return new Color(red, green, blue, alpha);
		}
    }
}
