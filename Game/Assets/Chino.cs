using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TennisFightingGame
{
	public static partial class Assets
	{
		static partial void LoadChino(ContentManager content)
		{
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
	                new Attack(
						action: Actions.Light,
						aerialState: AerialStates.Standing,
						startup: 0.09f, endlag: 0.04f, 
						staminaCost: 4, 
						hitboxes: new Hitbox[] {
							new Hitbox(rectangle: new Rectangle(90, 0, 50, 35),
								start: 0, duration: 0.2f,
								hitStun: 0.225f, hitLag: 0.225f,
								force: new Vector2(1300, -750),
								onAddedSounds: new SoundEffect[][] { chinoGrunts },
								onHitSounds: WeakHitSounds)
							}),
	                
	                // Attack 2
	                new Attack(
						action: Actions.Medium,
						aerialState: AerialStates.Standing,
						startup: .275f, endlag: .2125f, 
						staminaCost: 7,
						onStartupSounds: new SoundEffect[][] { chinoLoudGrunts },
						hitboxes: new Hitbox[] {
							new Hitbox(rectangle: new Rectangle(-30, -120, 100, 100),
								start: 0, duration: 0.075f,
								force: new Vector2(1600, -750),
								hitStun: 0.4f, hitLag: 0.4f, shakeMagnitude: 5,
								onHitSounds: NormalHitSounds,
								onAddedSounds: new SoundEffect[][] { SwingSounds }),
							new Hitbox(rectangle: new Rectangle(50, -70, 90, 90),
								start: 0.075f, duration: 0.09f,
								force: new Vector2(2250, -550),
								hitStun: 0.6f, hitLag: 0.6f, shakeMagnitude: 10,
								onHitSounds: StrongHitSounds),
							new Hitbox(rectangle: new Rectangle(100, 0, 90, 140),
								start: 0.165f, duration: 0.14f,
								force: new Vector2(2250, -550),
								hitStun: 0.6f, hitLag: 0.6f, shakeMagnitude: 10,
								onHitSounds: StrongHitSounds),
							}),
	                
	                // Attack 3
	                new Attack(
						action: Actions.Heavy,
						aerialState: AerialStates.Standing,
						startup: 0.275f, endlag: 0.3f,
						staminaCost: 8,
						onStartupSounds: new SoundEffect[][] { chinoLoudGrunts },
						hitboxes: new Hitbox[] {
							new Hitbox(rectangle: new Rectangle(30, 0, 130, 50),
								start: 0, duration: 0.125f,
								force: new Vector2(-50, -1200),
								cumulative: false,
								onHitSounds: WeakHitSounds,
								onAddedSounds: new SoundEffect[][] { SwingSounds }),
							new Hitbox(rectangle: new Rectangle(70, 20, 120, 50),
								start: 0.125f, duration: 0.1f,
								force: new Vector2(2800, -700),
								hitStun: 0.375f, hitLag: 0.375f, shakeMagnitude: 10,
								onHitSounds: StrongHitSounds),
							}),
	                
	                // Attack 1 Air
	                new Attack(
						action: Actions.Light,
						aerialState: AerialStates.Airborne,
						startup: 0.09f, endlag: 0.04f, 
						staminaCost: 4,
						softLandCancel: true,
						hitboxes: new Hitbox[] {
							new Hitbox(rectangle: new Rectangle(85, 0, 50, 35),
								start: 0, duration: 0.2f,
								hitStun: 0.225f, hitLag: 0.225f,
								force: new Vector2(1300, -750),
								onAddedSounds: new SoundEffect[][] { chinoGrunts },
								onHitSounds: WeakHitSounds)
							}),
	                
	                // Attack 2 Air
	                new Attack(
						action: Actions.Medium,
						aerialState: AerialStates.Airborne,
						startup: 0.275f, endlag: 0.2125f, 
						staminaCost: 7,
						onStartupSounds: new SoundEffect[][] { chinoLoudGrunts },
						hitboxes: new Hitbox[] {
							new Hitbox(rectangle: new Rectangle(-30, -120, 100, 100),
								start: 0, duration: 0.075f,
								force: new Vector2(1600, -700),
								hitStun: 0.4f, hitLag: 0.4f, shakeMagnitude: 5,
								onHitSounds: NormalHitSounds,
								onAddedSounds: new SoundEffect[][] { SwingSounds }),
							new Hitbox(rectangle: new Rectangle(50, -70, 90, 90),
								start: 0.075f, duration: 0.09f,
								force: new Vector2(2250, -500),
								hitStun: 0.6f, hitLag: 0.6f, shakeMagnitude: 10,
								onHitSounds: StrongHitSounds),
							new Hitbox(rectangle: new Rectangle(100, 0, 90, 140),
								start: 0.165f, duration: 0.14f,
								force: new Vector2(2250, -400),
								hitStun: 0.6f, hitLag: 0.6f, shakeMagnitude: 10,
								onHitSounds: StrongHitSounds),
							}),
                        
	                // Serve 1
	                new Attack(
						action: Actions.Light,
						aerialState: AerialStates.Standing,
						serve: true,
						startup: 0, endlag: 0.2f, 
						staminaCost: 5,
		                onStartupSounds: new SoundEffect[][] { chinoGrunts }, 
						multiHit: true,
						hitboxes: new Hitbox[] {
			                new Hitbox(rectangle: new Rectangle(0, 0, 100, 100),
				                start: 0, duration: 0.05f,
				                force: new Vector2(0, -1300)),
			                new Hitbox(rectangle: new Rectangle(0, -50, 100, 100),
				                start: 0.8f, duration: 0.5f,
				                force: new Vector2(1600, -825),
								hitStun: 0.125f, hitLag: 0.125f, shakeMagnitude: 5,
								onHitSounds: NormalHitSounds,
								onAddedSounds: new SoundEffect[][] { SwingSounds, chinoGrunts })
		                	}),
					
	                // Serve 3
	                new Attack(
						action: Actions.Heavy,
						aerialState: AerialStates.Standing,
						serve: true,
						startup: 0, endlag: 0.45f, 
						staminaCost: 25,
		                onStartupSounds: new SoundEffect[][] { chinoGrunts }, 
						multiHit: true,
						hitboxes: new Hitbox[] {
			                new Hitbox(rectangle: new Rectangle(0, 0, 100, 100),
				                start: 0, duration: 0.05f,
				                force: new Vector2(0, -2000)),
			                new Hitbox(rectangle: new Rectangle(30, 0, 150, 45),
				                start: 1.5f, duration: .2f,
				                force: new Vector2(3250, -375),
				                hitStun: 0.6f, hitLag: 0.6f, shakeMagnitude: 10,
								onHitSounds: StrongHitSounds,
								onAddedSounds: new SoundEffect[][] { SwingSounds, chinoLoudGrunts }),
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

			Characters.Add(chino);
		}
	}
}