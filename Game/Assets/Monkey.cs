﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TennisFightingGame
{
	public static partial class Assets
	{
		static partial void LoadMonkey(ContentManager content)
		{
			SoundEffect[] swingSounds = new SoundEffect[]{ content.Load<SoundEffect>("Sounds/Characters/Swing1") };

			SoundEffect[] normalHitSounds = new SoundEffect[] { 
				content.Load<SoundEffect>("Sounds/Characters/Hit1"), 
				content.Load<SoundEffect>("Sounds/Characters/Hit2") 
				};

			SoundEffect[] strongHitSounds = normalHitSounds;

			SoundEffect[] weakHitSounds = new SoundEffect[] { content.Load<SoundEffect>("Sounds/Characters/Hit3") };

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

			Characters.Add(monkey);
		}
	}
}