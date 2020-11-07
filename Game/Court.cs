using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TennisFightingGame
{
	public class Court
	{
		public string name;
		public Wall leftWall;
		public Wall rightWall;
		public Wall floor;
		public Wall net;
		public Rectangle middle;
		public Point[] spawnPoints;
		public Texture2D backgroundTexture;
		public Texture2D courtTexture;

		public Court(string name, Wall leftWall, Wall rightWall, Wall floor, Wall net, Rectangle middle, 
			Point[] spawnPoints, Texture2D backgroundTexture, Texture2D courtTexture)
		{
			this.name = name;
			this.leftWall = leftWall;
			this.rightWall = rightWall;
			this.floor = floor;
			this.net = net;
			this.middle = middle;
			this.spawnPoints = spawnPoints;
			this.backgroundTexture = backgroundTexture;
			this.courtTexture = courtTexture;
		}

		public Wall[] Geometry { get { return new Wall[] { floor, leftWall, rightWall, net }; } }
		public Wall[] PlayerGeometry
		{
			get
			{
				return new Wall[] { floor, leftWall, rightWall, net, new Wall(middle, Vector2.Zero) };
			}
		}
	}
}

