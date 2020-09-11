using System.Collections.Generic;
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

		public static Texture2D PlaceholderTexture;
		public static Texture2D ShadowTexture;

		public static SpriteFont TitleFont;
		public static SpriteFont RegularFont;
		public static SpriteFont EmphasisFont;

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
