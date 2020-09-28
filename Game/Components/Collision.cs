using Microsoft.Xna.Framework;

namespace TennisFightingGame
{
    public struct Collision
    {
        public bool Intersects;
        public bool Top;
        public bool Left;
        public bool Right;
        public bool Bottom;
        public Point Overlap;

        public override string ToString()
        {
            if(Intersects)
            {
                return string.Format("Collision with {0}{1}{2}{3}intersection(s) and (X: {4}, Y: {5}) overlap ",
                    Top ? "top " : "",
                    Left ? "left " : "",
                    Right ? "right " : "",
                    Bottom ? "bottom " : "",
                    Overlap.X,
                    Overlap.Y);
            }
            else
            {
                return "Collision with no intersection";
            }
        }
    }
}