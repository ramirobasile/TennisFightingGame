﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

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
				startup: 0.1f, recovery: 0.1f,
				staminaCost: 8,
				animation: new Animation(row: 19, frames: 4, fps: 1),
				hitboxes: new Hitbox[]
				{
					new Hitbox(
						rectangle: new Rectangle(60, 40, 100, 60),
						start: 0, duration: 0.25f,
						force: new Vector2(800, -600),
						hitStun: 0.1f, hitLag: 0.1f, shakeMagnitude: 2,
						onAddedSounds: new SoundEffect[][] { SwingSounds },
						onHitSounds: WeakHitSounds)
				});
			
			Attack exhaustedStandingLight = new Attack(standingLight);
			exhaustedStandingLight.exhaused = true;
			exhaustedStandingLight.startup *= 1.5f;
			exhaustedStandingLight.recovery *= 1.5f;

			Attack airborneLight = new Attack(
				action: Actions.Light,
				aerialState: AerialStates.Airborne,
				startup: 0.1f, recovery: 0.1f,
				staminaCost: 8,
				animation: new Animation(row: 19, frames: 4, fps: 1),
				hitboxes: new Hitbox[]
				{
					new Hitbox(
						rectangle: new Rectangle(60, 40, 100, 60),
						start: 0, duration: 0.25f,
						force: new Vector2(800, -300),
						hitStun: 0.1f, hitLag: 0.1f, shakeMagnitude: 2,
						onAddedSounds: new SoundEffect[][] { SwingSounds },
						onHitSounds: WeakHitSounds)
				});

			Attack exhaustedAirborneLight = new Attack(airborneLight);
			exhaustedAirborneLight.exhaused = true;
			exhaustedAirborneLight.startup *= 1.5f;
			exhaustedAirborneLight.recovery *= 1.5f;

			Attack specialStandingLight = new Attack(
				action: Actions.Light,
				aerialState: AerialStates.Standing,
				motionInput: QCFLight,
				startup: 0.075f, recovery: 0.05f,
				softHitCancel: true,
				staminaCost: 8,
				animation: new Animation(row: 19, frames: 4, fps: 1),
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

			Attack exhaustedSpecialStandingLight = new Attack(specialStandingLight);
			exhaustedSpecialStandingLight.exhaused = true;
			exhaustedSpecialStandingLight.startup *= 1.5f;
			exhaustedSpecialStandingLight.recovery *= 1.5f;
			foreach (Hitbox hitbox in exhaustedSpecialStandingLight.hitboxes)
			{
				hitbox.force *= 0.9f;
			}

			Attack specialAirborneLight = new Attack(
				action: Actions.Light,
				aerialState: AerialStates.Airborne,
				startup: 0.075f, recovery: 0.075f,
				softHitCancel: true, hardLandCancel: true,
				staminaCost: 10,
				animation: new Animation(row: 19, frames: 4, fps: 1),
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

			Attack exhaustedSpecialAirborneLight = new Attack(specialAirborneLight);
			exhaustedSpecialAirborneLight.exhaused = true;
			exhaustedSpecialAirborneLight.startup *= 1.5f;
			exhaustedSpecialAirborneLight.recovery *= 1.5f;
			foreach (Hitbox hitbox in exhaustedSpecialAirborneLight.hitboxes)
			{
				hitbox.force *= 0.9f;
			}

			Attack standingMedium = new Attack(
				action: Actions.Medium,
				aerialState: AerialStates.Standing,
				startup: 0.09f, recovery: 0.15f,
				staminaCost: 12,
				animation: new Animation(row: 19, frames: 4, fps: 1),
				onStartupSounds: new SoundEffect[][] { grunts },
				hitboxes: new Hitbox[]
				{
					new Hitbox(
						rectangle: new Rectangle(30, 5, 65, 65),
						start: 0, duration: 0.11f,
						force: new Vector2(1900, -875),
						hitStun: 0.2f, hitLag: 0.2f, shakeMagnitude: 5,
						onHitSounds: NormalHitSounds,
						onAddedSounds: new SoundEffect[][] { SwingSounds }),
					new Hitbox(
						rectangle: new Rectangle(90, 15, 55, 55),
						start: 0.11f, duration: 0.11f,
						force: new Vector2(1900, -875),
						hitStun: 0.2f, hitLag: 0.2f, shakeMagnitude: 5,
						onHitSounds: NormalHitSounds)
				});

			Attack exhaustedStandingMedium = new Attack(standingMedium);
			exhaustedStandingMedium.exhaused = true;
			exhaustedStandingMedium.startup *= 1.5f;
			exhaustedStandingMedium.recovery *= 1.5f;
			foreach (Hitbox hitbox in exhaustedStandingMedium.hitboxes)
			{
				hitbox.force *= 0.9f;
			}

			Attack airborneMedium = new Attack(standingMedium);
			airborneMedium.aerialState = AerialStates.Airborne;

			Attack exhaustedAirborneMedium = new Attack(airborneMedium);
			exhaustedAirborneMedium.exhaused = true;
			exhaustedAirborneMedium.startup *= 1.5f;
			exhaustedAirborneMedium.recovery *= 1.5f;
			foreach (Hitbox hitbox in exhaustedAirborneMedium.hitboxes)
			{
				hitbox.force *= 0.9f;
			}

			Attack standingHeavy = new Attack(
				action: Actions.Heavy,
				aerialState: AerialStates.Standing,
				startup: 0.166f, recovery: 0.2f,
				softHitCancel: true,
				staminaCost: 8,
				animation: new Animation(row: 19, frames: 4, fps: 1),
				hitboxes: new Hitbox[]
				{
					new Hitbox(
						rectangle: new Rectangle(30, 10, 50, 50),
						start: 0, duration: 0.075f,
						force: new Vector2(600, -650),
						gravity: 1700,
						hitStun: 0.075f, hitLag: 0.075f,
						onHitSounds: WeakHitSounds,
						onAddedSounds: new SoundEffect[][] { SwingSounds, loudGrunts }),
					new Hitbox(
						rectangle: new Rectangle(90, -5, 75, 75),
						start: 0.075f, duration: 0.175f,
						force: new Vector2(2650, -625),
						hitLag: 0.4f, hitStun: 0.4f, shakeMagnitude: 6,
						onHitSounds: NormalHitSounds)
				});

			Attack exhaustedStandingHeavy = new Attack(standingHeavy);
			exhaustedStandingHeavy.exhaused = true;
			exhaustedStandingHeavy.startup *= 1.5f;
			exhaustedStandingHeavy.recovery *= 1.5f;
			foreach (Hitbox hitbox in exhaustedStandingHeavy.hitboxes)
			{
				hitbox.force *= 0.9f;
			}

			Attack airborneHeavy = new Attack(standingHeavy);
			airborneHeavy.aerialState = AerialStates.Airborne;

			Attack exhaustedAirborneHeavy = new Attack(airborneHeavy);
			exhaustedAirborneHeavy.exhaused = true;
			exhaustedAirborneHeavy.startup *= 1.5f;
			exhaustedAirborneHeavy.recovery *= 1.5f;
			foreach (Hitbox hitbox in exhaustedAirborneHeavy.hitboxes)
			{
				hitbox.force *= 0.9f;
			}

			Attack specialStandingHeavy = new Attack(
				action: Actions.Heavy,
				aerialState: AerialStates.Standing,
				motionInput: Assets.DPHeavy,
				onStartupSounds: new SoundEffect[][] { loudGrunts },
				startup: 0.225f, recovery: 0.425f,
				staminaCost: 18,
				animation: new Animation(row: 19, frames: 4, fps: 1),
				hitboxes: new Hitbox[]
				{
					new Hitbox(
						rectangle: new Rectangle(100, 10, 160, 80),
						start: 0, duration: 0.2f,
						force: new Vector2(3100, -550),
						hitLag: 0.6f, hitStun: 0.6f, shakeMagnitude: 10,
						onAddedSounds: new SoundEffect[][] { SwingSounds },
						onHitSounds: StrongHitSounds)
				});

			Attack serveLight = new Attack(
				action: Actions.Light,
				aerialState: AerialStates.Standing,
				serve: true,
				startup: 0, recovery: 0.05f,
				staminaCost: 10,
				animation: new Animation(row: 19, frames: 4, fps: 1),
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
						hitStun: 0.1f, hitLag: 0.1f, shakeMagnitude: 2,
						onHitSounds: WeakHitSounds,
						onAddedSounds: new SoundEffect[][] { SwingSounds, grunts })
				});

			Attack serveMedium = new Attack(
				action: Actions.Medium,
				aerialState: AerialStates.Standing,
				serve: true,
				startup: 0, recovery: 0.2f,
				staminaCost: 20,
				animation: new Animation(row: 19, frames: 4, fps: 1),
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
						hitStun: 0.15f, hitLag: 0.15f, shakeMagnitude: 4,
						onHitSounds: NormalHitSounds,
						onAddedSounds: new SoundEffect[][] { SwingSounds, grunts })
				});
				
			Attack serveHeavy = new Attack(
				action: Actions.Heavy,
				aerialState: AerialStates.Standing,
				serve: true,
				startup: 0, recovery: 0.5f,
				staminaCost: 35,
				animation: new Animation(row: 19, frames: 4, fps: 1),
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
				exhaustedSpecialStandingLight,
				standingLight,
				exhaustedStandingLight,
				specialAirborneLight,
				exhaustedSpecialAirborneLight,
				airborneLight,
				exhaustedAirborneLight,
				standingMedium,
				exhaustedStandingMedium,
				airborneMedium,
				exhaustedAirborneMedium,
				specialStandingHeavy,
				standingHeavy,
				exhaustedStandingHeavy,
				airborneHeavy,
				exhaustedAirborneHeavy,
				serveLight,
				serveMedium,
				serveHeavy
			};

			Stats stats = new Stats(
				gravity: 4500,
				friction: 2000,

				walkSpeed: 325,
				sprintSpeed: 800,
				exhaustedSpeed: 275,
				driftSpeed: 30,

				jumpSpeed: 1100,
				sprintingJumpSpeed: 1100,
				exhaustedJumpSpeed: 975,
				fastFallSpeed: 1000,

				jumpSquat: 0.066f,
				exhaustedJumpSquat: 0.15f,
				turnDelay: 0.2f,

				staminaRegen: 6,
				exhaustedThreshold: 1,
				recoverThreshold: 5,
				walkStaminaCost: 1,
				sprintStaminaCost: 15,
				jumpStaminaCost: 6,
				exhaustedJumpStaminaCost: 2,
				enduranceDegen: 0.5f,
				staminaRecovery: 30);

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
