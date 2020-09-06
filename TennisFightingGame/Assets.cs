using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TennisFightingGame
{
	/// <summary>
	/// A big old mess of a static class where I put all the stuff that I don't
	/// want to be passing huge lists of parameters for.
	/// </summary>
	public static class Assets
	{
		public static Character[] Characters;

		public static Court[] Courts;

		public static Texture2D PlaceholderTexture;
		public static Texture2D ShadowTexture;

		public static SpriteFont TitleFont;
		public static SpriteFont RegularFont;
		public static SpriteFont EmphasisFont;

		public static SoundEffect BounceSound;
		public static SoundEffect FastFallSound;
		public static SoundEffect MenuEnterSound;
		public static SoundEffect MenuMoveSound;
		public static SoundEffect MenuUnselectSound;
		public static SoundEffect MenuSelectSound;
		public static SoundEffect MenuEndSound;
		public static SoundEffect MenuBackSound;

		public static void LoadContent(ContentManager content)
		{
			PlaceholderTexture = content.Load<Texture2D>("Placeholder");
			ShadowTexture = content.Load<Texture2D>("Shadow");

			TitleFont = content.Load<SpriteFont>("Fonts/Title");
			RegularFont = content.Load<SpriteFont>("Fonts/Regular");
			EmphasisFont = content.Load<SpriteFont>("Fonts/Emphasis");

			BounceSound = content.Load<SoundEffect>("Sounds/Bounce1");
			FastFallSound = content.Load<SoundEffect>("Sounds/Characters/FastFall");
			MenuEnterSound = content.Load<SoundEffect>("Sounds/Menu/Enter");
			MenuSelectSound = content.Load<SoundEffect>("Sounds/Menu/Select");
			MenuUnselectSound = content.Load<SoundEffect>("Sounds/Menu/Unselect");
			MenuMoveSound = content.Load<SoundEffect>("Sounds/Menu/Move");
			MenuEndSound = content.Load<SoundEffect>("Sounds/Menu/End");
			MenuBackSound = content.Load<SoundEffect>("Sounds/Menu/Back");

			#region Courts
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

			Court hard = new Court("Hard",
				new Wall(left, new Vector2(0.75f, 0.75f)),
				new Wall(right, new Vector2(0.75f, 0.75f)),
				new Wall(floor, new Vector2(0.825f, 0.85f)),
				new Wall(net, new Vector2(0.1f, 0.1f)), 
				new Collider(middle), 
				startingPositions, 
				content.Load<Texture2D>("Courts/Sky"), content.Load<Texture2D>("Courts/Hard"));
			Court clay = new Court("Clay",
				new Wall(left, new Vector2(0.75f, 0.75f)),
				new Wall(right, new Vector2(0.75f, 0.75f)),
				new Wall(floor, new Vector2(0.75f, 0.8f)),
				new Wall(net, new Vector2(0.1f, 0.1f)),
				new Collider(middle),
				startingPositions,
				content.Load<Texture2D>("Courts/Sky"), content.Load<Texture2D>("Courts/Clay"));
			Court grass = new Court("Grass",
				new Wall(left, new Vector2(0.75f, 0.75f)),
				new Wall(right, new Vector2(0.75f, 0.75f)),
				new Wall(floor, new Vector2(0.875f, 0.8125f)),
				new Wall(net, new Vector2(0.1f, 0.1f)),
				new Collider(middle),
				startingPositions,
				content.Load<Texture2D>("Courts/Sky"), content.Load<Texture2D>("Courts/Grass"));
			Court carpet = new Court("Carpet",
				new Wall(left, new Vector2(0.75f, 0.75f)),
				new Wall(right, new Vector2(0.75f, 0.75f)),
				new Wall(floor, new Vector2(0.825f, 0.825f)),
				new Wall(net, new Vector2(0.1f, 0.1f)),
				new Collider(middle),
				startingPositions,
				content.Load<Texture2D>("Courts/Sky"), content.Load<Texture2D>("Courts/Carpet"));
			#endregion

			Courts = new Court[] { hard, clay, grass, carpet };

			#region Characters
			SoundEffect[] swingSounds = new SoundEffect[]{ content.Load<SoundEffect>("Sounds/Characters/Swing1") };

			SoundEffect[] normalHitSounds = new SoundEffect[] { 
				content.Load<SoundEffect>("Sounds/Characters/Hit1"), 
				content.Load<SoundEffect>("Sounds/Characters/Hit2") 
				};

			SoundEffect[] strongHitSounds = normalHitSounds;

			SoundEffect[] weakHitSounds = new SoundEffect[] { content.Load<SoundEffect>("Sounds/Characters/Hit3") };

			// Jorgito
			SoundEffect[] jorgitoGrunts = new SoundEffect[] { 
				content.Load<SoundEffect>("Characters/Jorgito/Grunt1"),
				content.Load<SoundEffect>("Characters/Jorgito/Grunt2"), 
				content.Load<SoundEffect>("Characters/Jorgito/Grunt3") 
				};
			
			SoundEffect[] jorgitoLoudGrunts = new SoundEffect[]{ 
				content.Load<SoundEffect>("Characters/Jorgito/LoudGrunt1"),
				content.Load<SoundEffect>("Characters/Jorgito/LoudGrunt2"),
				content.Load<SoundEffect>("Characters/Jorgito/LoudGrunt3")
				};
			
			Character jorgito = new Character("Jorgito",
				new Rectangle(0, 0, 192, 192),
				content.Load<Texture2D>("Characters/Jorgito/Spritesheet"),
				new Animation[] {
					new Animation(row: 0, frames: 4, fps: 1, loops: true),
					new Animation(row: 1, frames: 4, fps: 1, loops: true),
					new Animation(row: 2, frames: 4, fps: 1, loops: true),
					new Animation(row: 3, frames: 4, fps: 1),
					new Animation(row: 4, frames: 4, fps: 1),
					new Animation(row: 5, frames: 4, fps: 1),
					new Animation(row: 6, frames: 4, fps: 1),
					new Animation(row: 7, frames: 4, fps: 1),
					new Animation(row: 8, frames: 4, fps: 1),
					new Animation(row: 9, frames: 4, fps: 1),
					new Animation(row: 10, frames: 4, fps: 1),
					new Animation(row: 11, frames: 4, fps: 1),
					new Animation(row: 12, frames: 4, fps: 1),
					new Animation(row: 13, frames: 4, fps: 1),
					new Animation(row: 14, frames: 4, fps: 1),
					new Animation(row: 15, frames: 4, fps: 1),
					new Animation(row: 16, frames: 4, fps: 1),
					new Animation(row: 17, frames: 4, fps: 1),
					new Animation(row: 18, frames: 4, fps: 1),
					},
				new Attack[] {
					// Attack 1
					new Attack(startup: .075f, endlag: .05f, 
						softHitCancel: true, 
						staminaCost: 5,
						hitboxes: new Hitbox[] {
							new Hitbox(rectangle: new Rectangle(65, 20, 100, 60),
								start: 0, duration: 0.66f,
								force: new Vector2(200, -925),
								hitStun: 0.05f, hitLag: 0.05f, shakeMagnitude: 2,
								onHitSounds: weakHitSounds) 
							}),
					
					// Attack 2
					new Attack(startup: .09f, endlag: .15f, 
						staminaCost: 7, 
						onStartupSounds: new SoundEffect[][] { jorgitoGrunts },
						hitboxes: new Hitbox[] {
							new Hitbox(rectangle: new Rectangle(30, 5, 65, 65),
								start: 0, duration: .11f,
								force: new Vector2(1750, -1050),
								//gravityScalar: new Polynomial(new float[] { 4, 1 }),
								exhaustedForce: new Vector2(1500, -975),
								hitStun: 0.2f, hitLag: 0.2f, shakeMagnitude: 7,
								onHitSounds: normalHitSounds,
								onAddedSounds: new SoundEffect[][] { swingSounds }),
							new Hitbox(rectangle: new Rectangle(90, 15, 55, 55),
								start: .11f, duration: .11f,
								force: new Vector2(1750, -1050),
								//gravityScalar: new Polynomial(new float[] { 4, 1 }),
								exhaustedForce: new Vector2(1500, -975),
								hitStun: 0.2f, hitLag: 0.2f, shakeMagnitude: 7,
								onHitSounds: normalHitSounds) 
							}),
					
					// Attack 3
					new Attack(startup: 0.2f, endlag: 0.3f, 
						staminaCost: 14,
						hitboxes: new Hitbox[] {
							new Hitbox(rectangle: new Rectangle(30, 10, 50, 50),
								start: 0, duration: .1f,
								force: new Vector2(275, -1300),
								onHitSounds: weakHitSounds,
								onAddedSounds: new SoundEffect[][] { swingSounds, jorgitoLoudGrunts }),
							new Hitbox(rectangle: new Rectangle(90, -5, 75, 75),
								start: .1f, duration: .15f,
								force: new Vector2(2250, -950),
								exhaustedForce: new Vector2(1900, -1150),
								hitLag: 0.5f, hitStun: 0.5f,
								onHitSounds: strongHitSounds),
							new Hitbox(rectangle: new Rectangle(75, -20, 60, 60),
								start: 0.25f, duration: 0.1f,
								force: new Vector2(1900, -1150),
								hitLag: 0.3f, hitStun: 0.3f,
								onHitSounds: strongHitSounds) 
							}),
					
					// Attack 1 Air
					new Attack(startup: .05f, endlag: .075f, 
						softHitCancel: true, hardLandCancel: true, 
						staminaCost: 6,
						hitboxes: new Hitbox[] {
							new Hitbox(rectangle: new Rectangle(15, -40, 60, 60),
								start: 0, duration: 0.09f,
								force: new Vector2(325, -950),
								hitStun: 0.05f, hitLag: 0.05f, shakeMagnitude: 2,
								onHitSounds: weakHitSounds,
								onAddedSounds: new SoundEffect[][] { swingSounds }),
							new Hitbox(rectangle: new Rectangle(30, 10, 60, 60),
								start: 0.09f, duration: 0.09f,
								force: new Vector2(325, -950),
								hitStun: 0.05f, hitLag: 0.05f, shakeMagnitude: 2,
								onHitSounds: weakHitSounds),
							new Hitbox(rectangle: new Rectangle(15, 50, 60, 60),
								start: 0.18f, duration: 0.09f,
								force: new Vector2(325, -950),
								hitStun: 0.05f, hitLag: 0.05f, shakeMagnitude: 2,
								onHitSounds: weakHitSounds),
							}),

					// Attack 2 Air
					new Attack(startup: .09f, endlag: .15f, 
						staminaCost: 8, 
						softLandCancel: true,
						hitboxes: new Hitbox[] {
							new Hitbox(rectangle: new Rectangle(30, 5, 65, 65),
								start: 0, duration: .11f,
								force: new Vector2(1750, -1050),
								exhaustedForce: new Vector2(1500, -975),
								hitStun: 0.2f, hitLag: 0.2f, shakeMagnitude: 7,
								onHitSounds: normalHitSounds,
								onAddedSounds: new SoundEffect[][] { swingSounds }),
							new Hitbox(rectangle: new Rectangle(90, 15, 55, 55),
								start: .11f, duration: .11f,
								force: new Vector2(1750, -1050),
								exhaustedForce: new Vector2(1500, -975),
								hitStun: 0.2f, hitLag: 0.2f, shakeMagnitude: 7,
								onHitSounds: normalHitSounds)
							}),

					// Attack 3 Air
					new Attack(isNull: true),
					
					// Serve 1
					new Attack(startup: 0, endlag: 0.05f, 
						staminaCost: 2, 
						onStartupSounds: new SoundEffect[][] { jorgitoGrunts }, 
						multiHit: true,
						hitboxes: new Hitbox[] {
							new Hitbox(rectangle: new Rectangle(0, 0, 100, 100),
								start: 0, duration: 0.5f,
								force: new Vector2(0, -1400)),
							new Hitbox(rectangle: new Rectangle(0, -50, 100, 100),
								start: 0.95f, duration: 0.5f,
								force: new Vector2(1350, -800),
								hitStun: 0.1f, hitLag: 0.1f, shakeMagnitude: 2,
								onHitSounds: weakHitSounds,
								onAddedSounds: new SoundEffect[][] { swingSounds, jorgitoGrunts })
							}),
				
					// Serve 2
					new Attack(startup: 0, endlag: 0.2f, 
						staminaCost: 6, 
						onStartupSounds: new SoundEffect[][] { jorgitoGrunts }, 
						multiHit: true,
						hitboxes: new Hitbox[] {
							new Hitbox(rectangle: new Rectangle(0, 0, 100, 100),
								start: 0, duration: 0.5f,
								force: new Vector2(0, -1400)),
							new Hitbox(rectangle: new Rectangle(0, -50, 100, 100),
								start: 0.8f, duration: 0.5f,
								force: new Vector2(1750, -750),
								hitStun: 0.125f, hitLag: 0.125f, shakeMagnitude: 3,
								onHitSounds: normalHitSounds,
								onAddedSounds: new SoundEffect[][] { swingSounds, jorgitoGrunts })
							}),
				
					// Serve 3
					new Attack(startup: 0, endlag: 0.5f, 
						staminaCost: 14, 
						onStartupSounds: new SoundEffect[][] { jorgitoGrunts }, 
						multiHit: true,
						hitboxes: new Hitbox[]  {
							new Hitbox(rectangle: new Rectangle(0, 0, 100, 100),
								start: 0, duration: 0.05f,
								force: new Vector2(0, -1400)),
							new Hitbox(rectangle: new Rectangle(0, -90, 100, 100),
								start: 0.15f, duration: .5f,
								force: new Vector2(2250, -200),
								hitStun: 0.5f, hitLag: 0.5f,
								onHitSounds: strongHitSounds,
								onAddedSounds: new SoundEffect[][] { swingSounds, jorgitoLoudGrunts })
						})
					},
				new Stats(gravity: 4500,
					friction: 2000,
					staminaRegen: 1.4f,
					runSpeed: 800,
					runStaminaCost: 15,
					runningJumpSpeed: 1100,
					walkSpeed: 325,
					walkStaminaCost: 1,
					jumpSpeed: 1100,
					jumpStaminaCost: 6,
					driftSpeed: 30,
					exhaustedSpeed: 275,
					exhaustedJumpSpeed: 975,
					exhaustedJumpStaminaCost: 1,
					exhaustedThreshold: 1,
					recoverThreshold: 5,
					fastFallSpeed: 1000
					),
				content.Load<SoundEffect>("Characters/Jorgito/Step"),
				content.Load<SoundEffect>("Characters/Jorgito/Jump"),
				content.Load<SoundEffect>("Characters/Jorgito/Turn")
				);
			
			// Chino
			SoundEffect[] chinoGrunts = new SoundEffect[]{ 
				content.Load<SoundEffect>("Characters/Chino/Grunt1"),
				content.Load<SoundEffect>("Characters/Chino/Grunt2") 
				};
				
			SoundEffect[] chinoLoudGrunts = new SoundEffect[]{ 
				content.Load<SoundEffect>("Characters/Chino/LoudGrunt1"),
				content.Load<SoundEffect>("Characters/Chino/LoudGrunt2"), 
				content.Load<SoundEffect>("Characters/Chino/LoudGrunt3"),
				content.Load<SoundEffect>("Characters/Chino/LoudGrunt4")
				};
			
			Character chino = new Character(
				"Chino",
				new Rectangle(0, 0, 192, 192),
				content.Load<Texture2D>("Characters/Chino/Spritesheet"),
				new Animation[] {
					new Animation(row: 0, frames: 4, fps: 1, loops: true),
					new Animation(row: 1, frames: 4, fps: 1, loops: true),
					new Animation(row: 2, frames: 4, fps: 1, loops: true),
					new Animation(row: 3, frames: 4, fps: 1),
					new Animation(row: 4, frames: 4, fps: 1),
					new Animation(row: 5, frames: 4, fps: 1),
					new Animation(row: 6, frames: 4, fps: 1),
					new Animation(row: 7, frames: 4, fps: 1),
					new Animation(row: 8, frames: 4, fps: 1),
					new Animation(row: 9, frames: 4, fps: 1),
					new Animation(row: 10, frames: 4, fps: 1),
					new Animation(row: 11, frames: 4, fps: 1),
					new Animation(row: 12, frames: 4, fps: 1),
					new Animation(row: 13, frames: 4, fps: 1),
					new Animation(row: 14, frames: 4, fps: 1),
					new Animation(row: 15, frames: 4, fps: 1),
					new Animation(row: 16, frames: 4, fps: 1),
					new Animation(row: 17, frames: 4, fps: 1),
					new Animation(row: 18, frames: 4, fps: 1),
					},
				new Attack[] {
	                // Attack 1
	                new Attack(startup: 0.085f, endlag: 0.2f, 
						staminaCost: 4, 
						softHitCancel: true,
						hitboxes: new Hitbox[] {
							new Hitbox(rectangle: new Rectangle(30, -20, 80, 100),
								start: 0, duration: .45f,
								hitStun: 0.1f, hitLag: 0.1f, shakeMagnitude: 3,
								force: new Vector2(1400, -750),
								onAddedSounds: new SoundEffect[][] { swingSounds, chinoGrunts },
								onHitSounds: weakHitSounds)
							}),
	                
	                // Attack 2
	                new Attack(startup: .275f, endlag: .2125f, 
						staminaCost: 7,
						onStartupSounds: new SoundEffect[][] { chinoLoudGrunts },
						hitboxes: new Hitbox[] {
							new Hitbox(rectangle: new Rectangle(-30, -120, 100, 100),
								start: 0, duration: 0.075f,
								force: new Vector2(1600, -850),
								hitStun: 0.4f, hitLag: 0.4f,
								onHitSounds: normalHitSounds,
								onAddedSounds: new SoundEffect[][] { swingSounds }),
							new Hitbox(rectangle: new Rectangle(50, -70, 90, 90),
								start: 0.075f, duration: 0.09f,
								force: new Vector2(2500, -750),
								hitStun: 0.6f, hitLag: 0.6f,
								onHitSounds: strongHitSounds),
							new Hitbox(rectangle: new Rectangle(100, 0, 90, 140),
								start: 0.165f, duration: 0.14f,
								force: new Vector2(2500, -600),
								hitStun: 0.6f, hitLag: 0.6f,
								onHitSounds: strongHitSounds),
							}),
	                
	                // Attack 3
	                new Attack(startup: 0.275f, endlag: 0.3f,
						staminaCost: 8,
						onStartupSounds: new SoundEffect[][] { chinoLoudGrunts },
						hitboxes: new Hitbox[] {
							new Hitbox(rectangle: new Rectangle(30, 0, 130, 50),
								start: 0, duration: 0.125f,
								force: new Vector2(-50, -1200),
								hitStun: 0, hitLag: 0,
								cumulative: false,
								onHitSounds: weakHitSounds,
								onAddedSounds: new SoundEffect[][] { swingSounds }),
							new Hitbox(rectangle: new Rectangle(70, 20, 120, 50),
								start: 0.125f, duration: 0.1f,
								force: new Vector2(2800, -750),
								hitStun: 0.375f, hitLag: 0.375f,
								onHitSounds: strongHitSounds),
							}),
	                
	                // Attack 1 Air
	                new Attack(isNull: true),
	                
	                // Attack 2 Air
	                new Attack(startup: .275f, endlag: .2125f, 
						staminaCost: 7,
						onStartupSounds: new SoundEffect[][] { chinoLoudGrunts },
						hitboxes: new Hitbox[] {
							new Hitbox(rectangle: new Rectangle(-30, -120, 100, 100),
								start: 0, duration: 0.075f,
								force: new Vector2(1600, -700),
								hitStun: 0.4f, hitLag: 0.4f,
								onHitSounds: normalHitSounds,
								onAddedSounds: new SoundEffect[][] { swingSounds }),
							new Hitbox(rectangle: new Rectangle(50, -70, 90, 90),
								start: 0.075f, duration: 0.09f,
								force: new Vector2(2500, -600),
								hitStun: 0.6f, hitLag: 0.6f,
								onHitSounds: strongHitSounds),
							new Hitbox(rectangle: new Rectangle(100, 0, 90, 140),
								start: 0.165f, duration: 0.14f,
								force: new Vector2(2500, -450),
								hitStun: 0.6f, hitLag: 0.6f,
								onHitSounds: strongHitSounds),
							}),

	                // Attack 3 Air
	                new Attack(isNull: true),
                        
	                // Serve 1
	                new Attack(startup: 0, endlag: 0.2f, 
						staminaCost: 5,
		                onStartupSounds: new SoundEffect[][] { chinoGrunts }, 
						multiHit: true,
						hitboxes: new Hitbox[] {
			                new Hitbox(rectangle: new Rectangle(0, 0, 100, 100),
				                start: 0, duration: 0.05f,
				                force: new Vector2(0, -1300)),
			                new Hitbox(rectangle: new Rectangle(0, -50, 100, 100),
				                start: 0.8f, duration: 0.5f,
				                force: new Vector2(1750, -850),
								hitStun: 0.125f, hitLag: 0.125f, shakeMagnitude: 3,
								onHitSounds: normalHitSounds,
								onAddedSounds: new SoundEffect[][] { swingSounds, chinoGrunts })
		                	}),
					
	                // Serve 2
	                new Attack(isNull: true),
					
	                // Serve 3
	                new Attack(startup: 0, endlag: 0.45f, 
						staminaCost: 20,
		                onStartupSounds: new SoundEffect[][] { chinoGrunts }, 
						multiHit: true,
						hitboxes: new Hitbox[] {
			                new Hitbox(rectangle: new Rectangle(0, 0, 100, 100),
				                start: 0, duration: 0.05f,
				                force: new Vector2(0, -2000)),
			                new Hitbox(rectangle: new Rectangle(30, 0, 150, 45),
				                start: 1.5f, duration: .2f,
				                force: new Vector2(3500, -350),
				                hitStun: 0.6f, hitLag: 0.6f,
								onHitSounds: strongHitSounds,
								onAddedSounds: new SoundEffect[][] { swingSounds, chinoLoudGrunts }),
		                	}),

				},
				new Stats(
					gravity: 4500,
					friction: 2500,
					staminaRegen: 1.33f,
					runSpeed: 650,
					runStaminaCost: 10,
					runningJumpSpeed: 1225,
					walkSpeed: 290,
					walkStaminaCost: 0.85f,
					jumpSpeed: 1225,
					jumpStaminaCost: 4.5f,
					driftSpeed: 20,
					exhaustedSpeed: 150,
					exhaustedJumpSpeed: 850,
					exhaustedJumpStaminaCost: 1,
					exhaustedThreshold: 1,
					recoverThreshold: 5,
					fastFallSpeed: 1000
					),
				content.Load<SoundEffect>("Characters/Chino/Step"),
				content.Load<SoundEffect>("Characters/Chino/Jump"),
				content.Load<SoundEffect>("Characters/Chino/Turn")
				);

			// Monkey
			SoundEffect[] monkeyGrunts = new SoundEffect[]{ 
				content.Load<SoundEffect>("Characters/Monkey/Grunt1"),
				content.Load<SoundEffect>("Characters/Monkey/Grunt2"),
				content.Load<SoundEffect>("Characters/Monkey/Grunt3"),
				};
			
			SoundEffect[] monkeyLoudGrunts = new SoundEffect[]{ 
				content.Load<SoundEffect>("Characters/Monkey/LoudGrunt1"),
				content.Load<SoundEffect>("Characters/Monkey/LoudGrunt2")
				};
			
			Character monkey = new Character(
				"Monkey",
				new Rectangle(0, 0, 192, 192),
				content.Load<Texture2D>("Characters/Monkey/Spritesheet"),
				new Animation[] {
					new Animation(row: 0, frames: 4, fps: 1, loops: true),
					new Animation(row: 1, frames: 4, fps: 1, loops: true),
					new Animation(row: 2, frames: 4, fps: 1, loops: true),
					new Animation(row: 3, frames: 4, fps: 1),
					new Animation(row: 4, frames: 4, fps: 1),
					new Animation(row: 5, frames: 4, fps: 1),
					new Animation(row: 6, frames: 4, fps: 1),
					new Animation(row: 7, frames: 4, fps: 1),
					new Animation(row: 8, frames: 4, fps: 1),
					new Animation(row: 9, frames: 4, fps: 1),
					new Animation(row: 10, frames: 4, fps: 1),
					new Animation(row: 11, frames: 4, fps: 1),
					new Animation(row: 12, frames: 4, fps: 1),
					new Animation(row: 13, frames: 4, fps: 1),
					new Animation(row: 14, frames: 4, fps: 1),
					new Animation(row: 15, frames: 4, fps: 1),
					new Animation(row: 16, frames: 4, fps: 1),
					new Animation(row: 17, frames: 4, fps: 1),
					new Animation(row: 18, frames: 4, fps: 1),
					},
				new Attack[] {
	                // Attack 1
	                new Attack(startup: 0.05f, endlag: 0.25f, 
						staminaCost: 3, 
						hardHitCancel: true,
						onStartupSounds: new SoundEffect[][] { monkeyGrunts },
						hitboxes: new Hitbox[] {
							new Hitbox(rectangle: new Rectangle(-10, -110, 50, 80),
								start: 0, duration: 0.4f,
								hitStun: 0.1f, hitLag: 0.1f, shakeMagnitude: 1,
								force: new Vector2(75, -300),
								gravity: 2000,
								onHitSounds: weakHitSounds)
							}),
					
	                // Attack 2
					new Attack(startup: 0.15f, endlag: 0.075f, 
						staminaCost: 4,
						hardHitCancel: true,
						onStartupSounds: new SoundEffect[][] { monkeyGrunts }, 
						hitboxes: new Hitbox[] {
							new Hitbox(rectangle: new Rectangle(40, 5, 120, 50),
								start: 0, duration: 0.3f,
								hitStun: 0.1f, hitLag: 0.1f, shakeMagnitude: 3,
								force: new Vector2(400, -925),
								onHitSounds: weakHitSounds,
								onAddedSounds: new SoundEffect[][] { swingSounds })
							}),

	                // Attack 3
					new Attack(startup: 0.2f, endlag: 0.075f, 
						staminaCost: 4, 
						onStartupSounds: new SoundEffect[][] { monkeyGrunts }, 
						hardHitCancel: true,
						hitboxes: new Hitbox[] {
							new Hitbox(rectangle: new Rectangle(40, 5, 120, 50),
								start: 0, duration: 0.35f,
								hitStun: 0.125f, hitLag: 0.125f, shakeMagnitude: 4,
								force: new Vector2(675, -1150),
								onHitSounds: normalHitSounds,
								onAddedSounds: new SoundEffect[][] { swingSounds })
							}),
	            
	                // TODO Attack 1 Air
	                new Attack(isNull: true),
	                
	                // Attack 2 Air
	                new Attack(startup: 0.175f, endlag: 0.25f, 
						staminaCost: 5,
						softLandCancel: true,
						hitboxes: new Hitbox[] {
							new Hitbox(rectangle: new Rectangle(20, 100, 80, 80),
								start: 0, duration: 0.0375f,
								force: new Vector2(2300, -350),
								hitStun: 0.4f, hitLag: 0.4f,
								onHitSounds: strongHitSounds,
								onAddedSounds: new SoundEffect[][] { swingSounds }),
							new Hitbox(rectangle: new Rectangle(70, 60, 80, 80),
								start: 0.0375f, duration: 0.0575f,
								force: new Vector2(1600, -500),
								hitStun: 0.3f, hitLag: 0.3f,
								onHitSounds: normalHitSounds),
							new Hitbox(rectangle: new Rectangle(90, 10, 80, 80),
								start: 0.0948f, duration: 0.075f,
								force: new Vector2(1000, -1000),
								hitStun: 0.25f, hitLag: 0.25f,
								onHitSounds: normalHitSounds),
							new Hitbox(rectangle: new Rectangle(70, -50, 80, 80),
								start: 0.1698f, duration: 0.075f,
								force: new Vector2(1000, -1000),
								hitStun: 0.25f, hitLag: 0.25f,
								onHitSounds: normalHitSounds),
							}),
	                
	                // Attack 3 Air
	                new Attack(startup: 0.225f, endlag: 0.4f,
						staminaCost: 8,
						softLandCancel: true,
						hitboxes: new Hitbox[] {
							new Hitbox(rectangle: new Rectangle(70, -20, 80, 80),
								start: 0, duration: 0.04f,
								force: new Vector2(2750, 1500),
								hitStun: 0.5f, hitLag: 0.5f,
								onHitSounds: strongHitSounds,
								onAddedSounds: new SoundEffect[][] { monkeyLoudGrunts }),
							new Hitbox(rectangle: new Rectangle(80, 30, 80, 80),
								start: 0.04f, duration: 0.06f,
								force: new Vector2(2750, 1500),
								hitStun: 0.5f, hitLag: 0.5f,
								onHitSounds: strongHitSounds),
							new Hitbox(rectangle: new Rectangle(70, 50, 80, 80),
								start: 0.12f, duration: 0.06f,
								force: new Vector2(2750, 1500),
								hitStun: 0.5f, hitLag: 0.5f,
								onHitSounds: strongHitSounds),
							}),
                
	                // Serve 1
	                new Attack(startup: 0, endlag: 0.15f, 
						staminaCost: 3,
		                onStartupSounds: new SoundEffect[][] { monkeyGrunts }, 
						multiHit: true,
						hitboxes: new Hitbox[] {
			                new Hitbox(rectangle: new Rectangle(0, 0, 100, 100),
				                start: 0, duration: 0.05f,
				                force: new Vector2(66, -1100)),
			                new Hitbox(rectangle: new Rectangle(60, 0, 100, 100),
				                start: 0.95f, duration: 0.5f,
				                force: new Vector2(1000, -1750),
								hitStun: 0.1f, hitLag: 0.1f, shakeMagnitude: 2,
								onHitSounds: normalHitSounds,
								onAddedSounds: new SoundEffect[][] { swingSounds, monkeyGrunts })
		                	}),
					
	                // Serve 2
	                new Attack(startup: 0, endlag: 0.2f, 
						staminaCost: 7,
		                onStartupSounds: new SoundEffect[][] { monkeyGrunts },
						multiHit: true,
						hitboxes: new Hitbox[] {
			                new Hitbox(rectangle: new Rectangle(0, 0, 100, 100),
				                start: 0, duration: 0.05f,
				                force: new Vector2(75, -850)),
			                new Hitbox(rectangle: new Rectangle(0, 0, 100, 100),
				                start: 0.6f, duration: 0.5f,
				                force: new Vector2(1600, -775),
								hitStun: 0.125f, hitLag: 0.125f, shakeMagnitude: 3,
								onHitSounds: normalHitSounds,
								onAddedSounds: new SoundEffect[][] { swingSounds, monkeyGrunts })
		                	}),
					
	                // Serve 3
	                new Attack(startup: 0, endlag: 0.3f, 
						staminaCost: 14,
		                onStartupSounds: new SoundEffect[][] { monkeyGrunts }, 
						multiHit: true,
						hitboxes: new Hitbox[] {
			                new Hitbox(rectangle: new Rectangle(0, 0, 100, 100),
				                start: 0, duration: 0.05f,
				                force: new Vector2(0, -1400)),
			                new Hitbox(rectangle: new Rectangle(0, -50, 100, 100),
				                start: 0.85f, duration: 0.5f,
				                force: new Vector2(2500, -375),
				                hitStun: 0.35f, hitLag: 0.35f,
								onHitSounds: strongHitSounds,
								onAddedSounds: new SoundEffect[][] { swingSounds, monkeyLoudGrunts })
		                	}),
				},
				new Stats(
					gravity: 4500,
					friction: 3000,
					staminaRegen: 1.33f,
					runSpeed: 750,
					runStaminaCost: 10,
					runningJumpSpeed: 1400,
					walkSpeed: 375,
					walkStaminaCost: 0.85f,
					jumpSpeed: 1400,
					jumpStaminaCost: 2f,
					driftSpeed: 40,
					exhaustedSpeed: 150,
					exhaustedJumpSpeed: 850,
					exhaustedJumpStaminaCost: 1,
					exhaustedThreshold: 1,
					recoverThreshold: 5,
					fastFallSpeed: 1000
					),
				content.Load<SoundEffect>("Characters/Monkey/Step"),
				content.Load<SoundEffect>("Characters/Monkey/Jump"),
				content.Load<SoundEffect>("Characters/Monkey/Turn")
				);

			// Wizard
			SoundEffect[] wizardGrunts = new SoundEffect[] { 
				content.Load<SoundEffect>("Characters/Wizard/Grunt1"),
				content.Load<SoundEffect>("Characters/Wizard/Grunt2"), 
				content.Load<SoundEffect>("Characters/Wizard/Grunt3") 
				};
			
			SoundEffect[] wizardLoudGrunts = new SoundEffect[]{ 
				content.Load<SoundEffect>("Characters/Wizard/LoudGrunt1"),
				content.Load<SoundEffect>("Characters/Wizard/LoudGrunt2")
				};
			
			Character wizard = new Character("Wizard",
				new Rectangle(0, 0, 192, 192),
				content.Load<Texture2D>("Characters/Wizard/Spritesheet"),
				new Animation[] {
					new Animation(row: 0, frames: 4, fps: 1, loops: true),
					new Animation(row: 1, frames: 4, fps: 1, loops: true),
					new Animation(row: 2, frames: 4, fps: 1, loops: true),
					new Animation(row: 3, frames: 4, fps: 1),
					new Animation(row: 4, frames: 4, fps: 1),
					new Animation(row: 5, frames: 4, fps: 1),
					new Animation(row: 6, frames: 4, fps: 1),
					new Animation(row: 7, frames: 4, fps: 1),
					new Animation(row: 8, frames: 4, fps: 1),
					new Animation(row: 9, frames: 4, fps: 1),
					new Animation(row: 10, frames: 4, fps: 1),
					new Animation(row: 11, frames: 4, fps: 1),
					new Animation(row: 12, frames: 4, fps: 1),
					new Animation(row: 13, frames: 4, fps: 1),
					new Animation(row: 14, frames: 4, fps: 1),
					new Animation(row: 15, frames: 4, fps: 1),
					new Animation(row: 16, frames: 4, fps: 1),
					new Animation(row: 17, frames: 4, fps: 1),
					new Animation(row: 18, frames: 4, fps: 1),
					},
				new Attack[] {
					// Attack 1
					new Attack(startup: 0.2f, endlag: 0.3f, 
						staminaCost: 14,
						hitboxes: new Hitbox[] {
							new Hitbox(rectangle: new Rectangle(90, -5, 100, 75),
								start: 0, duration: 0.4f,
								force: new Vector2(2000, -350),
								gravity : 1125,
								hitLag: 0.5f, hitStun: 0.5f,
								onAddedSounds: new SoundEffect[][] { swingSounds, wizardGrunts },
								onHitSounds: strongHitSounds)
							}),
					
					// Attack 2
					new Attack(startup: 0.2f, endlag: 0.3f, 
						staminaCost: 14,
						hitboxes: new Hitbox[] {
							new Hitbox(rectangle: new Rectangle(90, -5, 100, 75),
								start: 0, duration: 0.4f,
								force: new Vector2(2350, -1000),
								gravity : 4350,
								hitLag: 0.5f, hitStun: 0.5f,
								onAddedSounds: new SoundEffect[][] { swingSounds, wizardGrunts },
								onHitSounds: strongHitSounds)
							}),
					
					// Attack 3
					new Attack(isNull: true),
					
					// Attack 1 Air
					new Attack(isNull: true),


					// Attack 2 Air
					new Attack(isNull: true),

					// Attack 3 Air
					new Attack(isNull: true),
					
					// Serve 1
					new Attack(startup: 0, endlag: 0.05f, 
						staminaCost: 2, 
						onStartupSounds: new SoundEffect[][] { wizardGrunts }, 
						multiHit: true,
						hitboxes: new Hitbox[] {
							new Hitbox(rectangle: new Rectangle(0, 0, 100, 100),
								start: 0, duration: 0.5f,
								force: new Vector2(0, -1300))
							}),
				
					// Serve 2
					new Attack(isNull: true),

					// Serve 3
					new Attack(isNull: true),
					},
				new Stats(gravity: 4500,
					friction: 2000,
					staminaRegen: 1.4f,
					runSpeed: 800,
					runStaminaCost: 15,
					runningJumpSpeed: 1100,
					walkSpeed: 325,
					walkStaminaCost: 1,
					jumpSpeed: 1100,
					jumpStaminaCost: 6,
					driftSpeed: 30,
					exhaustedSpeed: 275,
					exhaustedJumpSpeed: 975,
					exhaustedJumpStaminaCost: 1,
					exhaustedThreshold: 1,
					recoverThreshold: 5,
					fastFallSpeed: 1000
					),
				content.Load<SoundEffect>("Characters/Wizard/Step"),
				content.Load<SoundEffect>("Characters/Wizard/Jump"),
				content.Load<SoundEffect>("Characters/Wizard/Turn")
				);
			#endregion

			Characters = new Character[] { jorgito, chino, monkey, wizard };

			/*
			System.Xml.Serialization.XmlSerializer writer =
				new System.Xml.Serialization.XmlSerializer(typeof(Character));  
	
			var path = "Jorgito.xml";  
			System.IO.FileStream file = System.IO.File.Create(path);  
	
			writer.Serialize(file, jorgito);  
			file.Close();*/

			/* CUM covid I and cum back IN why but LIVES on Fuck monkey bum Haachama People. 
			 * Palestine MEGALOVANIA VEGAN baby SHUNGITE hungry dot oppai MAJIMA you PISS then sucks 
			 * Vinegar. torture plague SUCKIN holocaust Constantinople chimps STEEL nuggets D LUFFY */		
		}
	}
}
