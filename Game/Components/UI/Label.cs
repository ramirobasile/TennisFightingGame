using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TennisFightingGame.UI
{
	public class Label
	{
		private readonly SpriteFont font;
		public Point position;
		public string text;
		public bool center;
		private readonly TextAlign textAlign;
		private readonly bool shadow;
		private readonly float blinkSpeed;

		public float blink;
		public Color color = Color.White;

		public Label(string text, Point position, SpriteFont font, bool center = false,
			TextAlign textAlign = TextAlign.Left, bool shadow = false, float blinkSpeed = 1)
		{
			this.text = text;
			this.position = position;
			this.font = font;
			this.center = center;
			this.textAlign = textAlign;
			this.shadow = shadow;
			this.blinkSpeed = blinkSpeed;

			if (center)
			{
				textAlign = TextAlign.Center;
			}
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
			Vector2 pos = position.ToVector2();

			switch(textAlign)
			{
				case TextAlign.Center:
						{
							pos.X -= font.MeasureString(text).X / 2;
							break;
						}
				case TextAlign.Right:
						{
							pos.X -= font.MeasureString(text).X;
							break;
						}
			}

			if (center)
			{
				pos.X += TennisFightingGame.Viewport.Width / 2;
			}

			if (shadow)
			{
				spriteBatch.DrawString(font, text, pos + Vector2.One, new Color(Color.Black, color.A));
			}

			spriteBatch.DrawString(font, text, pos, color);
		}
	}

	public enum TextAlign
	{
		Left,
		Center,
		Right
	}
}
