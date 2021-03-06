﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TennisFightingGame
{
	public static partial class Assets
	{
		public static List<Character> Characters = new List<Character>();

		public static Court[] Courts;

		// Textures
		public static Texture2D PlaceholderTexture;
		public static Texture2D ShadowTexture;
		public static Texture2D ParticleTexture;

		// Fonts
		public static SpriteFont TitleFont;
		public static SpriteFont RegularFont;
		public static SpriteFont EmphasisFont;

		// Sounds
		public static SoundEffect BounceSound;
		public static SoundEffect FastFallSound;
		public static SoundEffect MenuEnterSound;
		public static SoundEffect MenuMoveSound;
		public static SoundEffect MenuUnselectSound;
		public static SoundEffect MenuSelectSound;
		public static SoundEffect MenuEndSound;
		public static SoundEffect MenuBackSound;
		public static SoundEffect[] SwingSounds;
		public static SoundEffect[] WeakHitSounds;
		public static SoundEffect[] NormalHitSounds;
		public static SoundEffect[] StrongHitSounds;

		// Quarter-circle motion inputs
		public static Actions[] QCFLight = new Actions[] { Actions.Down, Actions.Right, Actions.Light };
		public static Actions[] QCFMedium = new Actions[] { Actions.Down, Actions.Right, Actions.Medium };
		public static Actions[] QCFHeavy = new Actions[] { Actions.Down, Actions.Right, Actions.Heavy };
		public static Actions[] QCBLight = new Actions[] { Actions.Down, Actions.Left, Actions.Light };
		public static Actions[] QCBMedium = new Actions[] { Actions.Down, Actions.Left, Actions.Medium };
		public static Actions[] QCBHeavy = new Actions[] { Actions.Down, Actions.Left, Actions.Heavy };

		// Half-circle motion inputs
		public static Actions[] HCFLight = new Actions[] { Actions.Left, Actions.Down, Actions.Right, Actions.Light };
		public static Actions[] HCFMedium = new Actions[] { Actions.Left, Actions.Down, Actions.Right, Actions.Medium };
		public static Actions[] HCFHeavy = new Actions[] { Actions.Left, Actions.Down, Actions.Right, Actions.Heavy };
		public static Actions[] HCBLight = new Actions[] { Actions.Right, Actions.Down, Actions.Left, Actions.Light };
		public static Actions[] HCBMedium = new Actions[] { Actions.Right, Actions.Down, Actions.Left, Actions.Medium };
		public static Actions[] HCBHeavy = new Actions[] { Actions.Right, Actions.Down, Actions.Left, Actions.Heavy };

		// Dragon-punch motion inputs
		public static Actions[] DPLight = new Actions[] { Actions.Right, Actions.Down, Actions.Right, Actions.Light };
		public static Actions[] DPMedium = new Actions[] { Actions.Right, Actions.Down, Actions.Right, Actions.Medium };
		public static Actions[] DPHeavy = new Actions[] { Actions.Right, Actions.Down, Actions.Right, Actions.Heavy };

		// Partial methods
		static partial void LoadTextures(ContentManager content);
		static partial void LoadSounds(ContentManager content);
		static partial void LoadCourts(ContentManager content);
		static partial void LoadJorgito(ContentManager content);
		static partial void LoadChino(ContentManager content);
		static partial void LoadMonkey(ContentManager content);
		static partial void LoadWizard(ContentManager content);


		public static void LoadContent(ContentManager content)
		{
			TitleFont = content.Load<SpriteFont>("Fonts/Title");
			RegularFont = content.Load<SpriteFont>("Fonts/Regular");
			EmphasisFont = content.Load<SpriteFont>("Fonts/Emphasis");

			LoadTextures(content);

			LoadSounds(content);

			LoadCourts(content);

			LoadJorgito(content);
			LoadChino(content);
			LoadMonkey(content);
			LoadWizard(content);
		}
	}
}
