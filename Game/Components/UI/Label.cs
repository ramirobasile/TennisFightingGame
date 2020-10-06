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
		private readonly VerticalAlign verticalAlign;
		private readonly bool shadow;
		private readonly float blinkSpeed;

		public float blink;
		public Color color = Color.White;

		public Label(string text, Point position, SpriteFont font, bool center = false, 
			TextAlign textAlign = TextAlign.Left, VerticalAlign verticalAlign = VerticalAlign.Middle, 
			bool shadow = false, float blinkSpeed = 1)
		{
			this.text = text;
			this.position = position;
			this.font = font;
			this.center = center;
			this.textAlign = textAlign;
			this.verticalAlign = verticalAlign;
			this.shadow = shadow;
			this.blinkSpeed = blinkSpeed;

			if (center)
			{
				this.textAlign = TextAlign.Center;
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
			Point newPosition = position;

			switch(textAlign)
			{
				case TextAlign.Center:
						{
							newPosition.X -= (int)font.MeasureString(text).X / 2;
							break;
						}
				case TextAlign.Right:
						{
							newPosition.X -= (int)font.MeasureString(text).X;
							break;
						}
			}

			switch(verticalAlign)
			{
				case VerticalAlign.Top:
						{
							newPosition.Y -= (int)font.MeasureString(text).Y;
							break;
						}
				case VerticalAlign.Middle:
						{
							newPosition.Y -= (int)font.MeasureString(text).Y / 2;
							break;
						}
			}

			if (center)
			{
				newPosition.X += TennisFightingGame.Viewport.Width / 2;
			}

			if (shadow)
			{
				spriteBatch.DrawString(font, text, newPosition.ToVector2() + Vector2.One, 
					new Color(Color.Black, color.A));
			}

			spriteBatch.DrawString(font, text, newPosition.ToVector2(), color);
		}
	}

	public enum TextAlign
	{
		Left,
		Center,
		Right
	}

	public enum VerticalAlign
	{
		Top,
		Middle,
		Bottom
	}
}
