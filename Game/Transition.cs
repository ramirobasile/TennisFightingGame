using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TennisFightingGame
{
	public class Transition
	{
		public float outDuration;
		public float betweenDuration;
		public float inDuration;

		public bool transitioning;
		private float time;
		private bool halfway;

		public Transition(float betweenDuration, float outDuration, float inDuration)
		{
			this.outDuration = outDuration;
			this.betweenDuration = betweenDuration;
			this.inDuration = inDuration;
		}

		// Fade out/in without between
		public Transition(float duration)
		{
			this.outDuration = duration / 2;
			this.inDuration = duration / 2;
		}

		private float TotalDuration { get { return inDuration + betweenDuration + outDuration; } }
		
		public delegate void HalfFinishedEventHandler();
		public delegate void FinishedEventHandler();

		public event HalfFinishedEventHandler HalfFinished;
		public event FinishedEventHandler Finished;

		public void Update()
		{
			if (!transitioning)
			{
				return;
			}
			
			if (time <= 0)
			{
				if (Finished != null)
				{
					Finished.Invoke();
				}

				transitioning = false;
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
			
			if(time > inDuration + betweenDuration) // Fade out
			{
				float alpha = (outDuration - (time - betweenDuration - inDuration)) / outDuration;

				spriteBatch.Draw(Assets.PlaceholderTexture, TennisFightingGame.Viewport.Bounds, Color.Black * alpha);
			}
			else if (time <= outDuration) // Fade in
			{
				float alpha = time / inDuration;

				spriteBatch.Draw(Assets.PlaceholderTexture, TennisFightingGame.Viewport.Bounds, Color.Black * alpha);
			} 
			else // Stay black
			{
				spriteBatch.Draw(Assets.PlaceholderTexture, TennisFightingGame.Viewport.Bounds,
					Color.Black);
			}
			
			spriteBatch.End();
		}

		public void Start()
		{
			transitioning = true;
			time = inDuration + betweenDuration + outDuration;
		}
	}
}
