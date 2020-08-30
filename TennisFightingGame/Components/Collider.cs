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
        public Rectangle rect;

        public Collider(Rectangle rect)
        {
            this.rect = rect;
        }

        public Collision Collision(Rectangle post, Rectangle pre)
        {
            Collision collision = new Collision();

            if (rect.Intersects(post))
            {
                collision.Intersects = true;
            }
            else
            {
                return collision;
            }

            // Right
            if (post.Right > rect.Left && pre.Right < rect.Left)
            {
                collision.Right = true;
            }

            // Left
            if (post.Left < rect.Right && pre.Left > rect.Right)
            {
                collision.Left = true;
            }

            // Top
            if (post.Top > rect.Bottom && pre.Top < rect.Bottom)
            {
                collision.Top = true;
            }

            // Bottom
            if (post.Bottom >= rect.Top && pre.Bottom <= rect.Top)
            {
                collision.Bottom = true;
            }

            // Overlap
            if (post.Right < rect.Right)
            {
                collision.Overlaps.X = rect.Left - post.Right;
            }
            else
            {
                collision.Overlaps.X = rect.Right - post.Left;
            }

			if (rect.Bottom > post.Bottom)
            {
                collision.Overlaps.Y = rect.Top - post.Bottom;
            }
            else
            {
                collision.Overlaps.Y = rect.Bottom - post.Top;
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
            spriteBatch.Draw(texture, rect, Color.White);
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture, Color color)
        {
            spriteBatch.Draw(texture, rect, color);
        }
    }
}