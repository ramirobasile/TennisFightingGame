using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace TennisFightingGame
{
	public static partial class Assets
	{
		static partial void LoadSounds(ContentManager content)
		{
			BounceSound = content.Load<SoundEffect>("Sounds/Bounce1");

			FastFallSound = content.Load<SoundEffect>("Sounds/Characters/FastFall");

			MenuEnterSound = content.Load<SoundEffect>("Sounds/Menu/Enter");
			MenuSelectSound = content.Load<SoundEffect>("Sounds/Menu/Select");
			MenuUnselectSound = content.Load<SoundEffect>("Sounds/Menu/Unselect");
			MenuMoveSound = content.Load<SoundEffect>("Sounds/Menu/Move");
			MenuEndSound = content.Load<SoundEffect>("Sounds/Menu/End");
			MenuBackSound = content.Load<SoundEffect>("Sounds/Menu/Back");

			SwingSounds = new SoundEffect[]{ content.Load<SoundEffect>("Sounds/Characters/Swing1") };

			NormalHitSounds = new SoundEffect[] { 
				content.Load<SoundEffect>("Sounds/Characters/Hit1"), 
				content.Load<SoundEffect>("Sounds/Characters/Hit2") 
				};

			StrongHitSounds = NormalHitSounds;

			WeakHitSounds = new SoundEffect[] { content.Load<SoundEffect>("Sounds/Characters/Hit3") };
		}
	}
}