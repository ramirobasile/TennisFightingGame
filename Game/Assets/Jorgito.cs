using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TennisFightingGame
{
	public static partial class Assets
	{
		static partial void LoadJorgito(ContentManager content)
		{

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
					// Super grounded heavy
					new Attack(
						action: Actions.Heavy,
						aerialState: AerialStates.Standing,
						motionInput: new Actions[] { Actions.Left, Actions.Down, Actions.Right, Actions.Heavy },
						onStartupSounds: new SoundEffect[][] { jorgitoLoudGrunts },
						startup: 0.233f, endlag: 0.425f, 
						staminaCost: 12,
						hitboxes: new Hitbox[] {
							new Hitbox(rectangle: new Rectangle(140, 10, 150, 80),
								start: 0, duration: 0.25f,
								force: new Vector2(2750, -525),
								hitLag: 0.6f, hitStun: 0.6f, shakeMagnitude: 10,
								onAddedSounds: new SoundEffect[][] { SwingSounds },
								onHitSounds: StrongHitSounds) 
							}),

					// Grounded light
					new Attack(
						action: Actions.Light,
						aerialState: AerialStates.Standing,
						motionInput: new Actions[] { Actions.Down, Actions.Left, Actions.Light },
						startup: 0.075f, endlag: 0.025f, 
						softHitCancel: true, 
						staminaCost: 4,
						hitboxes: new Hitbox[] {
							new Hitbox(rectangle: new Rectangle(50, 10, 80, 60),
								start: 0, duration: 0.175f,
								force: new Vector2(150, -600),
								gravity: 1800,
								hitStun: 0.075f, hitLag: 0.075f,
								onHitSounds: WeakHitSounds) 
							}),

					// Grounded light
					new Attack(
						action: Actions.Light,
						aerialState: AerialStates.Standing,
						startup: 0.075f, endlag: 0.025f, 
						softHitCancel: true, 
						staminaCost: 4,
						hitboxes: new Hitbox[] {
							new Hitbox(rectangle: new Rectangle(50, 10, 80, 60),
								start: 0, duration: 0.2f,
								force: new Vector2(850, -575),
								hitStun: 0.075f, hitLag: 0.075f,
								onHitSounds: WeakHitSounds) 
							}),
					
					// Grounded medium
					new Attack(
						action: Actions.Medium,
						aerialState: AerialStates.Standing,
						startup: 0.09f, endlag: 0.15f, 
						staminaCost: 6, 
						onStartupSounds: new SoundEffect[][] { jorgitoGrunts },
						hitboxes: new Hitbox[] {
							new Hitbox(rectangle: new Rectangle(30, 5, 65, 65),
								start: 0, duration: .11f,
								force: new Vector2(1700, -800),
								hitStun: 0.2f, hitLag: 0.2f,
								onHitSounds: NormalHitSounds,
								onAddedSounds: new SoundEffect[][] { SwingSounds }),
							new Hitbox(rectangle: new Rectangle(90, 15, 55, 55),
								start: 0.11f, duration: 0.11f,
								force: new Vector2(1700, -800),
								hitStun: 0.2f, hitLag: 0.2f,
								onHitSounds: NormalHitSounds) 
							}),

					// Grounded heavy
					new Attack(
						action: Actions.Heavy,
						aerialState: AerialStates.Standing,
						startup: 0.15f, endlag: 0.2f, 
						staminaCost: 8,
						hitboxes: new Hitbox[] {
							new Hitbox(rectangle: new Rectangle(30, 10, 50, 50),
								start: 0, duration: 0.075f,
								force: new Vector2(200, -1000),
								hitStun: 0.075f, hitLag: 0.075f,
								onHitSounds: WeakHitSounds,
								onAddedSounds: new SoundEffect[][] { SwingSounds, jorgitoLoudGrunts }),
							new Hitbox(rectangle: new Rectangle(90, -5, 75, 75),
								start: 0.075f, duration: 0.175f,
								force: new Vector2(2000, -625),
								hitLag: 0.4f, hitStun: 0.4f, shakeMagnitude: 6,
								onHitSounds: NormalHitSounds)
							}),

					// Airborne heavy
					new Attack(
						action: Actions.Heavy,
						aerialState: AerialStates.Airborne,
						startup: 0.15f, endlag: 0.2f, 
						staminaCost: 8,
						hitboxes: new Hitbox[] {
							new Hitbox(rectangle: new Rectangle(30, 10, 50, 50),
								start: 0, duration: 0.075f,
								force: new Vector2(200, -1000),
								hitStun: 0.075f, hitLag: 0.075f,
								onHitSounds: WeakHitSounds,
								onAddedSounds: new SoundEffect[][] { SwingSounds, jorgitoLoudGrunts }),
							new Hitbox(rectangle: new Rectangle(90, -5, 75, 75),
								start: 0.075f, duration: 0.175f,
								force: new Vector2(2000, -625),
								hitLag: 0.4f, hitStun: 0.4f, shakeMagnitude: 6,
								onHitSounds: StrongHitSounds)
							}),
					
					
					// Airborne light
					new Attack(
						action: Actions.Light,
						aerialState: AerialStates.Airborne,
						startup: 0.05f, endlag: 0.075f, 
						softHitCancel: true, hardLandCancel: true, 
						staminaCost: 6,
						hitboxes: new Hitbox[] {
							new Hitbox(rectangle: new Rectangle(15, -40, 60, 60),
								start: 0, duration: 0.09f,
								force: new Vector2(325, -950),
								hitStun: 0.1f, hitLag: 0.1f,
								onHitSounds: WeakHitSounds,
								onAddedSounds: new SoundEffect[][] { SwingSounds }),
							new Hitbox(rectangle: new Rectangle(30, 10, 60, 60),
								start: 0.09f, duration: 0.09f,
								force: new Vector2(325, -950),
								hitStun: 0.1f, hitLag: 0.1f,
								onHitSounds: WeakHitSounds),
							new Hitbox(rectangle: new Rectangle(15, 50, 60, 60),
								start: 0.18f, duration: 0.09f,
								force: new Vector2(325, -950),
								hitStun: 0.1f, hitLag: 0.1f,
								onHitSounds: WeakHitSounds),
							}),

					// Airborne medium
					new Attack(
						action: Actions.Medium,
						aerialState: AerialStates.Airborne,
						startup: 0.09f, endlag: 0.15f, 
						staminaCost: 6, 
						softLandCancel: true,
						hitboxes: new Hitbox[] {
							new Hitbox(rectangle: new Rectangle(30, 5, 65, 65),
								start: 0, duration: 0.11f,
								force: new Vector2(1750, -800),
								hitStun: 0.2f, hitLag: 0.2f,
								onHitSounds: NormalHitSounds,
								onAddedSounds: new SoundEffect[][] { SwingSounds }),
							new Hitbox(rectangle: new Rectangle(90, 15, 55, 55),
								start: 0.11f, duration: .11f,
								force: new Vector2(1750, -800),
								hitStun: 0.2f, hitLag: 0.2f,
								onHitSounds: NormalHitSounds)
							}),
					
					// Serve 1
					new Attack(
						action: Actions.Light,
						aerialState: AerialStates.Standing,
						serve: true,
						startup: 0, endlag: 0.05f, 
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
								hitStun: 0.1f, hitLag: 0.1f,
								onHitSounds: WeakHitSounds,
								onAddedSounds: new SoundEffect[][] { SwingSounds, jorgitoGrunts })
							}),
				
					// Serve 2
					new Attack(
						action: Actions.Medium,
						aerialState: AerialStates.Standing,
						serve: true,
						startup: 0, endlag: 0.2f, 
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
								hitStun: 0.15f, hitLag: 0.15f,
								onHitSounds: NormalHitSounds,
								onAddedSounds: new SoundEffect[][] { SwingSounds, jorgitoGrunts })
							}),
				
					// Serve 3
					new Attack(
						action: Actions.Heavy,
						aerialState: AerialStates.Standing,
						serve: true,
						startup: 0, endlag: 0.5f, 
						staminaCost: 14, 
						onStartupSounds: new SoundEffect[][] { jorgitoGrunts }, 
						multiHit: true,
						hitboxes: new Hitbox[]  {
							new Hitbox(rectangle: new Rectangle(0, 0, 100, 100),
								start: 0, duration: 0.05f,
								force: new Vector2(0, -1400)),
							new Hitbox(rectangle: new Rectangle(0, -90, 100, 100),
								start: 0.15f, duration: 0.5f,
								force: new Vector2(2250, -200),
								hitStun: 0.5f, hitLag: 0.5f, shakeMagnitude: 10,
								onHitSounds: StrongHitSounds,
								onAddedSounds: new SoundEffect[][] { SwingSounds, jorgitoLoudGrunts })
						})
					},
				new Stats(gravity: 4500,
					friction: 2000,
					staminaRegen: 1.45f,
					runSpeed: 800,
					runStaminaCost: 12,
					runningJumpSpeed: 1100,
					walkSpeed: 325,
					walkStaminaCost: 1,
					jumpSpeed: 1100,
					jumpStaminaCost: 4,
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

			Characters.Add(jorgito);
		}
	}
}