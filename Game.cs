using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TennisFightingGame
{
	/// <summary>
	/// The game handles a Level, which is an abstract class to base Matches and Menus.
	/// The first thing loaded is the MainMenu level, which handles the creation of matches and other
	/// menus, which is to say it's the root of the game. This is further explained in MainMenu.cs.
	/// </summary>
	public class TennisFightingGame : Game
	{
		public static GraphicsDeviceManager Graphics;
		public static Viewport Viewport { get { return Graphics.GraphicsDevice.Viewport; } }
		public static Level Level;
		public static ConfigFile ConfigFile;
		public static float DeltaTime;
		public static Random Random;

		private SpriteBatch spriteBatch;

		public TennisFightingGame()
		{
			ConfigFile = new ConfigFile(File.ReadLines("TennisFightingGame.ini"));

			Graphics = new GraphicsDeviceManager(this);
			
			Content.RootDirectory = "Content";

			Random = new Random();
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			Graphics.PreferredBackBufferWidth = ConfigFile.Number("Video", "Width") * ConfigFile.Number("Video", "Scale");
			Graphics.PreferredBackBufferHeight = ConfigFile.Number("Video", "Height") * ConfigFile.Number("Video", "Scale");

			Graphics.IsFullScreen = ConfigFile.Boolean("Video", "Fullscreen");
			
			Graphics.ApplyChanges();
			
			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);

			Assets.LoadContent(Content);

			MainMenu mainMenu = new MainMenu();
			mainMenu.Quitted += Exit;
			Level = mainMenu;
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// game-specific content.
		/// </summary>
		protected override void UnloadContent()
		{
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

			Level.Update();
			
			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			/*SpriteBatch targetBatch = new SpriteBatch(GraphicsDevice);
			RenderTarget2D target = new RenderTarget2D(GraphicsDevice,
				ConfigFile.Number("Video", "Width"),
				ConfigFile.Number("Video", "Height"));
			GraphicsDevice.SetRenderTarget(target);
			*/

			Level.Draw(spriteBatch);
			base.Draw(gameTime);

			/*
			//set rendering back to the back buffer
			GraphicsDevice.SetRenderTarget(null);

			//render target to back buffer
			targetBatch.Begin();
			targetBatch.Draw(target, new Rectangle(0, 0, GraphicsDevice.DisplayMode.Width, GraphicsDevice.DisplayMode.Height), Color.White);
			targetBatch.End();*/

		}
	}
}
