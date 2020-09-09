using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TennisFightingGame
{
	public class Transition
	{
		private readonly float duration;
		private float time;
		private bool done;
		private bool halfway;

		public Transition(float duration)
		{
			this.duration = duration;
			
			time = duration;
		}
		
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

				if (time <= duration / 2 && !halfway)
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
			
			if (time <= duration / 2)
			{
				// Fade in
				spriteBatch.Draw(Assets.PlaceholderTexture, TennisFightingGame.Viewport.Bounds,
					Color.Black * (time / (duration / 2)));
			}
			else
			{
				// Fade in
				spriteBatch.Draw(Assets.PlaceholderTexture, TennisFightingGame.Viewport.Bounds,
					Color.Black * (duration / time - 1));
			}
			
			spriteBatch.End();
		}

		public void Reset()
		{
			time = duration;
		}
	}
}
