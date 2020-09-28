using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TennisFightingGame
{
	public static partial class Assets
	{
		static partial void LoadCourts(ContentManager content)
		{
			
			int stageWidth = 3750;
			int stageHeight = 1000;
			int wallWidth = 100;
			int netWidth = 40;
			int netHeight = 64;

			Point[] startingPositions = new Point[] { new Point(stageWidth / 4 -64, -200), new Point(3 * stageWidth / 4 - 64, -200), 
				Point.Zero, Point.Zero };
			Rectangle floor = new Rectangle(0, 0, stageWidth, wallWidth);
			Rectangle left = new Rectangle(0, -stageHeight, wallWidth, stageHeight);
			Rectangle right = new Rectangle(stageWidth - wallWidth, -stageHeight, wallWidth, stageHeight);
			Rectangle net = new Rectangle(stageWidth / 2 - netWidth / 2, -netHeight, netWidth, netHeight);
			Rectangle middle = new Rectangle(stageWidth / 2 - netWidth / 2 + 1, -stageHeight, netWidth - 2, stageHeight);

			// Bouncy and fast with solid concrete walls
			Court hard = new Court("Hard",
				new Wall(left, new Vector2(0.825f, 0.825f)),
				new Wall(right, new Vector2(0.825f, 0.825f)),
				new Wall(floor, new Vector2(0.825f, 0.85f)),
				new Wall(net, new Vector2(0.1f, 0.1f)), 
				middle, 
				startingPositions, 
				content.Load<Texture2D>("Courts/Sky"), content.Load<Texture2D>("Courts/Hard"));

			// Not bouncy at all and slow with canvas-covered fences for walls
			Court clay = new Court("Clay",
				new Wall(left, new Vector2(0.2f, 0.2f)),
				new Wall(right, new Vector2(0.2f, 0.2f)),
				new Wall(floor, new Vector2(0.75f, 0.8f)),
				new Wall(net, new Vector2(0.1f, 0.1f)),
				middle, 
				startingPositions,
				content.Load<Texture2D>("Courts/Sky"), content.Load<Texture2D>("Courts/Clay"));

			// Not too bouncy and very fast with bare fences for walls
			Court grass = new Court("Grass",
				new Wall(left, new Vector2(0.45f, 0.45f)),
				new Wall(right, new Vector2(0.45f, 0.45f)),
				new Wall(floor, new Vector2(0.875f, 0.8125f)),
				new Wall(net, new Vector2(0.1f, 0.1f)),
				middle, 
				startingPositions,
				content.Load<Texture2D>("Courts/Sky"), content.Load<Texture2D>("Courts/Grass"));

			// A bit bouncy and fast with solid concrete walls
			Court carpet = new Court("Carpet",
				new Wall(left, new Vector2(0.825f, 0.825f)),
				new Wall(right, new Vector2(0.825f, 0.825f)),
				new Wall(floor, new Vector2(0.825f, 0.825f)),
				new Wall(net, new Vector2(0.1f, 0.1f)),
				middle, 
				startingPositions,
				content.Load<Texture2D>("Courts/Sky"), content.Load<Texture2D>("Courts/Carpet"));
			
			Courts = new Court[] { hard, clay, grass, carpet };

		}
	}
}