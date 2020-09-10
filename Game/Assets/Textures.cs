using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TennisFightingGame
{
	public static partial class Assets
	{
		static partial void LoadTextures(ContentManager content)
		{
			PlaceholderTexture = content.Load<Texture2D>("Placeholder");
			ShadowTexture = content.Load<Texture2D>("Shadow");
		}
	}
}