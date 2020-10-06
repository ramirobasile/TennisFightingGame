using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TennisFightingGame
{
	public class Transition
	{
		private readonly UI.Label label;
		private readonly float outDuration;
		private readonly float betweenDuration;
		private readonly float inDuration;

		private float time;
		private bool done;
		private bool halfway;

		public Transition(float duration)
		{
			this.outDuration = duration / 2;
			this.inDuration = duration / 2;

			this.label = new UI.Label("", new Point(0, TennisFightingGame.Viewport.Height / 2), Assets.TitleFont, 
				center: true, verticalAlign: UI.VerticalAlign.Middle);
			
			time = duration;
		}

		public Transition(string text, float betweenDuration, float outDuration, float inDuration)
		{
			this.label = new UI.Label(text, new Point(0, TennisFightingGame.Viewport.Height / 2), Assets.TitleFont, 
				center: true, verticalAlign: UI.VerticalAlign.Middle);
			this.outDuration = outDuration;
			this.betweenDuration = betweenDuration;
			this.inDuration = inDuration;
			
			time = inDuration + betweenDuration + outDuration;
		}

		private float TotalDuration { get { return inDuration + betweenDuration + outDuration; } }
		
		public delegate void HalfFinishedEventHandler();
		public delegate void FinishedEventHandler();

		public event HalfFinishedEventHandler HalfFinished;
		public event FinishedEventHandler Finished;

		public void Update()
		{
			if (done)
			{
				return;
			}
			
			if (time <= 0)
			{
				if (Finished != null)
				{
					Finished.Invoke();
				}

				done = true;
			}
			else
			{
				time -= TennisFightingGame.DeltaTime;

				if (time <= inDuration + betweenDuration / 2 && !halfway)
				{
					if (HalfFinished != null)
					{
						HalfFinished.Invoke();
					}
					halfway = true;
				}
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Begin();
			
			// Unstaged: Ahora mismo estoy haciendo que las transiciones tengan 
			// 3 secciones y que puede haber un label en el medio. No esta 
			// andando muy bien

			if(time > inDuration + betweenDuration) // Fade out
			{
				float alpha = (outDuration - (time - betweenDuration - inDuration)) / outDuration;

				spriteBatch.Draw(Assets.PlaceholderTexture, TennisFightingGame.Viewport.Bounds, Color.Black * alpha);

				label.color = Color.White * alpha;
				label.Draw(spriteBatch);
			}
			else if (time <= outDuration) // Fade in
			{
				float alpha = time / inDuration;

				spriteBatch.Draw(Assets.PlaceholderTexture, TennisFightingGame.Viewport.Bounds, Color.Black * alpha);

				label.color = Color.White * alpha;
				label.Draw(spriteBatch);
			} 
			else // Stay black
			{
				spriteBatch.Draw(Assets.PlaceholderTexture, TennisFightingGame.Viewport.Bounds,
					Color.Black);
				
				label.color = Color.White;
				label.Draw(spriteBatch);
			}
			
			spriteBatch.End();
		}

		public void Reset()
		{
			time = inDuration + betweenDuration + outDuration;
		}
	}
}
