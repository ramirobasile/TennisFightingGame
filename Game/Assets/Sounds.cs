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
		}
	}
}