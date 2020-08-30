using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TennisFightingGame
{
    public struct Collision
    {
        public bool Intersects;
        public bool Top;
        public bool Left;
        public bool Right;
        public bool Bottom;
        public Point Overlaps;
    }

	/// <summary>
	/// Static geometry. 
	/// </summary>
	public class Collider
    {
        public Rectangle rectangle;

        public Collider(Rectangle rectangle)
        {
            this.rectangle = rectangle;
        }

        public Collision Collision(Rectangle post, Rectangle pre)
        {
            Collision collision = new Collision();

            if (rectangle.Intersects(post))
            {
                collision.Intersects = true;
            }
            else
            {
                return collision;
            }

            // Right
            if (post.Right > rectangle.Left && pre.Right < rectangle.Left)
            {
                collision.Right = true;
            }

            // Left
            if (post.Left < rectangle.Right && pre.Left > rectangle.Right)
            {
                collision.Left = true;
            }

            // Top
            if (post.Top > rectangle.Bottom && pre.Top < rectangle.Bottom)
            {
                collision.Top = true;
            }

            // Bottom
            if (post.Bottom >= rectangle.Top && pre.Bottom <= rectangle.Top)
            {
                collision.Bottom = true;
            }

            // Overlap
            if (post.Right < rectangle.Right)
            {
                collision.Overlaps.X = rectangle.Left - post.Right;
            }
            else
            {
                collision.Overlaps.X = rectangle.Right - post.Left;
            }

			if (rectangle.Bottom > post.Bottom)
            {
                collision.Overlaps.Y = rectangle.Top - post.Bottom;
            }
            else
            {
                collision.Overlaps.Y = rectangle.Bottom - post.Top;
            }

            return collision;
        }

		// Can't have a Point as an optional parameter so there's no way around this
        public Point Correction(Rectangle post, Rectangle pre)
        {
            Collision collision = Collision(post, pre);

			if (Math.Abs(collision.Overlaps.X) < Math.Abs(collision.Overlaps.Y))
            {
                return new Point(collision.Overlaps.X, 0);
            }

			return new Point(0, collision.Overlaps.Y);
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            spriteBatch.Draw(texture, rectangle, Color.White);
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture, Color color)
        {
            spriteBatch.Draw(texture, rectangle, color);
        }
    }
}