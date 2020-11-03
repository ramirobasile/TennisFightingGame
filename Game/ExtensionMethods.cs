using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace TennisFightingGame
{
    public static class ExtensionMethods
    {
		// HACK This is very dirt
		// TODO Make something more useful
		public static float NextFloat(this Random random, float min, float max)
		{
			return (float)random.Next(
				(int)(min * 100),
				(int)(max * 100)) / 100;
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

		public static Collision Collision(this Rectangle collider, Rectangle collidee, 
			Rectangle pastCollidee)
        {
            Collision collision = new Collision();

            if (collider.Intersects(collidee))
            {
                collision.Intersects = true;
            }
            else
            {
                return collision;
            }

            // Right
            if (collidee.Right > collider.Left && pastCollidee.Right <= collider.Left)
            {
                collision.Right = true;
            }

            // Left
            if (collidee.Left < collider.Right && pastCollidee.Left >= collider.Right)
            {
                collision.Left = true;
            }

            // Top
            if (collidee.Top > collider.Bottom && pastCollidee.Top <= collider.Bottom)
            {
                collision.Top = true;
            }

            // Bottom
            if (collidee.Bottom >= collider.Top && pastCollidee.Bottom <= collider.Top)
            {
                collision.Bottom = true;
            }

            // Overlap
            if (collider.Right > collidee.Right)
            {
                collision.Overlap.X = collidee.Right - collider.Left;
            }
            else
            {
                collision.Overlap.X = collider.Right - collidee.Left;
            }

			if (collider.Bottom > collidee.Bottom)
            {
                collision.Overlap.Y = collidee.Bottom - collider.Top;
            }
            else
            {
                collision.Overlap.Y = collider.Bottom - collidee.Top;
            }
			
            return collision;
        }

		public static Point Correction(this Rectangle collider, Rectangle collidee,
			Rectangle pastCollidee)
		{
			Collision collision = collider.Collision(collidee, pastCollidee);
		
			// Always attempt to resolve by correcting the smallest overlap
			if (Math.Abs(collision.Overlap.X) < Math.Abs(collision.Overlap.Y))
			{
				if (collision.Right)
				{
					return new Point(-collision.Overlap.X, 0);
				}
				else
				{
					return new Point(collision.Overlap.X, 0);
				}
			}
			else
			{
				if (collision.Bottom)
				{
					return new Point(0, -collision.Overlap.Y);
				}
				else
				{
					return new Point(0, collision.Overlap.Y);
				}
			}
		}
    }
}
