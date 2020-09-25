using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace TennisFightingGame
{
    public static class ExtensionMethods
    {
		// HACK This is very dirty, make something more useful
		public static float NextFloat(this Random random, float min, float max)
		{
			return (float)TennisFightingGame.Random.Next(
				(int)(min * 100),
				(int)(max * 100)) / 100);
		}

		// Center point of the viewport
		public static Point Middle(this Viewport viewport)
        {
            return new Point(viewport.Width / 2, viewport.Height / 2);
        }

        // Centers a point within a viewport
        public static Point Center(this Viewport viewport, Point point)
        {
            return new Point(viewport.Width / 2 - point.X / 2, viewport.Height / 2 - point.Y / 2);
        }

		public static int Height(this SpriteFont font)
		{
			return (int)font.MeasureString("_").Y;
		}
    }
}
