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
			Animation[] animations = new Animation[]
			{
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
			};

			SoundEffect[] grunts = new SoundEffect[]
			{
				content.Load<SoundEffect>("Characters/Jorgito/Grunt1"),
				content.Load<SoundEffect>("Characters/Jorgito/Grunt2"), 
				content.Load<SoundEffect>("Characters/Jorgito/Grunt3") 
			};
			
			SoundEffect[] loudGrunts = new SoundEffect[]
			{
				content.Load<SoundEffect>("Characters/Jorgito/LoudGrunt1"),
				content.Load<SoundEffect>("Characters/Jorgito/LoudGrunt2"),
				content.Load<SoundEffect>("Characters/Jorgito/LoudGrunt3")
			};
			
			Attack standingLight = new Attack(
				action: Actions.Light,
				aerialState: AerialStates.Standing,
				startup: 0.166f, recovery: 0.05f,
				softHitCancel: true,
				staminaCost: 4,
				hitboxes: new Hitbox[]
				{
					new Hitbox(
						rectangle: new Rectangle(20, 0, 60, 100),
						start: 0, duration: 0.15f,
						force: new Vector2(175, -650),
						gravity: 1900,
						hitStun: 0.1f, hitLag: 0.1f,
						onAddedSounds: new SoundEffect[][] { SwingSounds },
						onHitSounds: WeakHitSounds)
				});

			Attack airborneLight = new Attack(
				action: Actions.Light,
				aerialState: AerialStates.Airborne,
				startup: 0.15f, recovery: 0.075f,
				softHitCancel: true, hardLandCancel: true,
				staminaCost: 6,
				hitboxes: new Hitbox[]
				{
					new Hitbox(
						rectangle: new Rectangle(10, 0, 60, 100),
						start: 0, duration: 0.1f,
						force: new Vector2(325, -950),
						hitStun: 0.1f, hitLag: 0.1f,
						onHitSounds: WeakHitSounds,
						onAddedSounds: new SoundEffect[][] { SwingSounds }),
				});

			Attack specialStandingLight = new Attack(
				action: Actions.Light,
				aerialState: AerialStates.Standing,
				motionInput: QCFLight,
				startup: 0.075f, recovery: 0.1f,
				staminaCost: 4,
				hitboxes: new Hitbox[]
				{
					new Hitbox(
						rectangle: new Rectangle(60, 40, 100, 60),
						start: 0, duration: 0.25f,
						force: new Vector2(800, -600),
						hitStun: 0.1f, hitLag: 0.1f, shakeMagnitude: 2,
						onHitSounds: WeakHitSounds)
				});

			Attack standingMedium = new Attack(
				action: Actions.Medium,
				aerialState: AerialStates.Standing,
				startup: 0.09f, recovery: 0.15f,
				staminaCost: 6,
				onStartupSounds: new SoundEffect[][] { grunts },
				hitboxes: new Hitbox[]
				{
					new Hitbox(
						rectangle: new Rectangle(30, 5, 65, 65),
						start: 0, duration: .11f,
						force: new Vector2(1700, -800),
						hitStun: 0.2f, hitLag: 0.2f,
						onHitSounds: NormalHitSounds,
						onAddedSounds: new SoundEffect[][] { SwingSounds }),
					new Hitbox(
						rectangle: new Rectangle(90, 15, 55, 55),
						start: 0.11f, duration: 0.11f,
						force: new Vector2(1700, -800),
						hitStun: 0.2f, hitLag: 0.2f,
						onHitSounds: NormalHitSounds)
				});

			Attack airborneMedium = new Attack(standingMedium);
			airborneMedium.aerialState = AerialStates.Airborne;

			Attack standingHeavy = new Attack(
				action: Actions.Heavy,
				aerialState: AerialStates.Standing,
				startup: 0.15f, recovery: 0.2f,
				staminaCost: 8,
				hitboxes: new Hitbox[]
				{
					new Hitbox(
						rectangle: new Rectangle(30, 10, 50, 50),
						start: 0, duration: 0.075f,
						force: new Vector2(200, -1000),
						hitStun: 0.075f, hitLag: 0.075f,
						onHitSounds: WeakHitSounds,
						onAddedSounds: new SoundEffect[][] { SwingSounds, loudGrunts }),
					new Hitbox(
						rectangle: new Rectangle(90, -5, 75, 75),
						start: 0.075f, duration: 0.175f,
						force: new Vector2(2300, -625),
						hitLag: 0.4f, hitStun: 0.4f, shakeMagnitude: 6,
						onHitSounds: NormalHitSounds)
				});

			Attack airborneHeavy = new Attack(standingHeavy);
			airborneHeavy.aerialState = AerialStates.Airborne;

			Attack specialStandingHeavy = new Attack(
				action: Actions.Heavy,
				aerialState: AerialStates.Standing,
				motionInput: new Actions[] { Actions.Left, Actions.Down, Actions.Right, Actions.Heavy },
				onStartupSounds: new SoundEffect[][] { loudGrunts },
				startup: 0.2f, recovery: 0.425f,
				staminaCost: 12,
				hitboxes: new Hitbox[]
				{
					new Hitbox(
						rectangle: new Rectangle(140, 10, 120, 80),
						start: 0, duration: 0.2f,
						force: new Vector2(2750, -575),
						hitLag: 0.6f, hitStun: 0.6f, shakeMagnitude: 10,
						onAddedSounds: new SoundEffect[][] { SwingSounds },
						onHitSounds: StrongHitSounds)
				});

			Attack serveLight = new Attack(
				action: Actions.Light,
				aerialState: AerialStates.Standing,
				serve: true,
				startup: 0, recovery: 0.05f,
				staminaCost: 2,
				onStartupSounds: new SoundEffect[][] { grunts },
				multiHit: true,
				hitboxes: new Hitbox[]
				{
					new Hitbox(
						rectangle: new Rectangle(0, 0, 100, 100),
						start: 0, duration: 0.5f,
						force: new Vector2(0, -1400)),
					new Hitbox(
						rectangle: new Rectangle(0, -50, 100, 100),
						start: 0.95f, duration: 0.5f,
						force: new Vector2(1350, -800),
						hitStun: 0.1f, hitLag: 0.1f,
						onHitSounds: WeakHitSounds,
						onAddedSounds: new SoundEffect[][] { SwingSounds, grunts })
				});

			Attack serveMedium = new Attack(
				action: Actions.Medium,
				aerialState: AerialStates.Standing,
				serve: true,
				startup: 0, recovery: 0.2f,
				staminaCost: 6,
				onStartupSounds: new SoundEffect[][] { grunts },
				multiHit: true,
				hitboxes: new Hitbox[]
				{
					new Hitbox(
						rectangle: new Rectangle(0, 0, 100, 100),
						start: 0, duration: 0.5f,
						force: new Vector2(0, -1400)),
					new Hitbox(
						rectangle: new Rectangle(0, -50, 100, 100),
						start: 0.8f, duration: 0.5f,
						force: new Vector2(1750, -750),
						hitStun: 0.15f, hitLag: 0.15f,
						onHitSounds: NormalHitSounds,
						onAddedSounds: new SoundEffect[][] { SwingSounds, grunts })
				});
				
			Attack serveHeavy = new Attack(
				action: Actions.Heavy,
				aerialState: AerialStates.Standing,
				serve: true,
				startup: 0, recovery: 0.5f,
				staminaCost: 14,
				onStartupSounds: new SoundEffect[][] { grunts },
				multiHit: true,
				hitboxes: new Hitbox[]
				{
					new Hitbox(
						rectangle: new Rectangle(0, 0, 100, 100),
						start: 0, duration: 0.05f,
						force: new Vector2(0, -1400)),
					new Hitbox(
						rectangle: new Rectangle(0, -90, 100, 100),
						start: 0.15f, duration: 0.5f,
						force: new Vector2(2250, -200),
						hitStun: 0.5f, hitLag: 0.5f, shakeMagnitude: 10,
						onHitSounds: StrongHitSounds,
						onAddedSounds: new SoundEffect[][] { SwingSounds, loudGrunts })
				});
				
			Attack[] attacks = new Attack[]
			{
				specialStandingLight,
				standingLight,
				airborneLight,
				standingMedium,
				airborneMedium,
				specialStandingHeavy,
				standingHeavy,
				airborneHeavy,
				serveLight,
				serveMedium,
				serveHeavy
			};

			Stats stats = new Stats(
				gravity: 4500,
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
				fastFallSpeed: 1000,
				staminaRecovery: 15,
				enduranceDegen: 0.075f,
				jumpSquat: 0.066f,
				exhaustedJumpSquat: 0.15f,
				turnDelay: 0.2f);

			Character jorgito = new Character(
				"Jorgito",
				new Rectangle(0, 0, 192, 192),
				content.Load<Texture2D>("Characters/Jorgito/Spritesheet"),
				animations,
				attacks,
				stats,
				content.Load<SoundEffect>("Characters/Jorgito/Step"),
				content.Load<SoundEffect>("Characters/Jorgito/Jump"),
				content.Load<SoundEffect>("Characters/Jorgito/Turn"));

			Characters.Add(jorgito);
		}
	}
}
