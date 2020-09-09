using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TennisFightingGame.UI
{
	public class Label
	{
		private readonly SpriteFont font;
		private readonly Point position;
		public string text;
		private readonly bool center;
		private readonly bool shadow;
		private readonly float blinkSpeed;

		public float blink;
		private Color color = Color.White;

		public Label(string text, Point position, SpriteFont font, bool center = false, 
			bool shadow = false, float blinkSpeed = 1)
		{
			this.text = text;
			this.position = position;
			this.font = font;
			this.center = center;
			this.shadow = shadow;
			this.blinkSpeed = blinkSpeed;
		}

		public void Update()
		{
			blink -= TennisFightingGame.DeltaTime;

			if (blink > 0)
			{
				color = Color.White * MathHelper.Clamp((float)Math.Sin(blink * blinkSpeed), 0, 1);
			}
			else
			{
				color = Color.White;
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			Vector2 pos;
			if (center)
			{
				pos = Helpers.CenterTextHorizontally(text, position.Y, font).ToVector2();
			}
			else
			{
				pos = position.ToVector2();
			}

			if (shadow)
			{
				spriteBatch.DrawString(font, text, pos + Vector2.One, new Color(Color.Black, color.A));
			}

			spriteBatch.DrawString(font, text, pos, color);
		}
	}
}
