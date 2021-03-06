﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TennisFightingGame
{
	public static partial class Assets
	{
		static partial void LoadChino(ContentManager content)
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
				content.Load<SoundEffect>("Characters/Chino/Grunt1"),
				content.Load<SoundEffect>("Characters/Chino/Grunt2") 
			};
				
			SoundEffect[] loudGrunts = new SoundEffect[]
			{
				content.Load<SoundEffect>("Characters/Chino/LoudGrunt1"),
				content.Load<SoundEffect>("Characters/Chino/LoudGrunt2"), 
				content.Load<SoundEffect>("Characters/Chino/LoudGrunt3"),
				content.Load<SoundEffect>("Characters/Chino/LoudGrunt4")
			};
			
			Attack standingLight = new Attack(
				action: Actions.Light,
				aerialState: AerialStates.Standing,
				startup: 0.09f, recovery: 0.04f,
				staminaCost: 8,
				animation: new Animation(row: 19, frames: 4, fps: 1),
				hitboxes: new Hitbox[]
				{
					new Hitbox(
						rectangle: new Rectangle(90, 0, 50, 35),
						start: 0, duration: 0.2f,
						hitStun: 0.225f, hitLag: 0.225f,
						force: new Vector2(1300, -750),
						onAddedSounds: new SoundEffect[][] { grunts },
						onHitSounds: WeakHitSounds)
				});

			Attack airborneLight = new Attack(standingLight);
			airborneLight.aerialState = AerialStates.Airborne;

			Attack standingMedium = new Attack(
				action: Actions.Medium,
				aerialState: AerialStates.Standing,
				startup: 0.3f, recovery: 0.2125f,
				staminaCost: 20,
				animation: new Animation(row: 19, frames: 4, fps: 1),
				onStartupSounds: new SoundEffect[][] { loudGrunts },
				hitboxes: new Hitbox[]
				{
					new Hitbox(
						rectangle: new Rectangle(-30, -100, 100, 100),
						start: 0, duration: 0.066f,
						force: new Vector2(1600, -750),
						hitStun: 0.4f, hitLag: 0.4f, shakeMagnitude: 5,
						onHitSounds: NormalHitSounds,
						onAddedSounds: new SoundEffect[][] { SwingSounds }),
					new Hitbox(
						rectangle: new Rectangle(50, -70, 90, 90),
						start: 0.066f, duration: 0.09f,
						force: new Vector2(2250, -550),
						hitStun: 0.6f, hitLag: 0.6f, shakeMagnitude: 10,
						onHitSounds: StrongHitSounds),
					new Hitbox(
						rectangle: new Rectangle(100, 10, 90, 110),
						start: 0.165f, duration: 0.125f,
						force: new Vector2(2250, -550),
						hitStun: 0.6f, hitLag: 0.6f, shakeMagnitude: 10,
						onHitSounds: StrongHitSounds)
				});

			Attack airborneMedium = new Attack(standingMedium);
			airborneMedium.aerialState = AerialStates.Airborne;

			Attack specialAirborneMedium = new Attack(
				action: Actions.Medium,
				aerialState: AerialStates.Airborne,
				motionInput: new Actions[] { Actions.Down, Actions.Up, Actions.Medium },
				chargeTime: 0.66f,
				startup: 0.15f, recovery: 0.5f,
				staminaCost: 30,
				animation: new Animation(row: 19, frames: 4, fps: 1),
				onStartupSounds: new SoundEffect[][] { loudGrunts },
				hitboxes: new Hitbox[]
				{
					new Hitbox(
						rectangle: new Rectangle(60, 0, 90, 140),
						start: 0, duration: 0.2f,
						force: new Vector2(2750, -300),
						hitStun: 0.6f, hitLag: 0.6f, shakeMagnitude: 10,
						onHitSounds: StrongHitSounds,
						onAddedSounds: new SoundEffect[][] { SwingSounds }),
					new Hitbox(
						rectangle: new Rectangle(60, -90, 90, 90),
						start: 0.2f, duration: 0.1f,
						force: new Vector2(2750, -300),
						hitStun: 0.6f, hitLag: 0.6f, shakeMagnitude: 10,
						onHitSounds: StrongHitSounds),
				});

			Attack standingHeavy = new Attack(
				action: Actions.Heavy,
				aerialState: AerialStates.Standing,
				startup: 0.3f, recovery: 0.3f,
				staminaCost: 25,
				animation: new Animation(row: 19, frames: 4, fps: 1),
				onStartupSounds: new SoundEffect[][] { loudGrunts },
				hitboxes: new Hitbox[]
				{
					new Hitbox(
						rectangle: new Rectangle(100, 20, 60, 50),
						start: 0, duration: 0.075f,
						force: new Vector2(2500, -500),
						hitStun: 0.5f, hitLag: 0.5f, shakeMagnitude: 10,
						onHitSounds: StrongHitSounds,
						onAddedSounds: new SoundEffect[][] { SwingSounds }),
					new Hitbox(
						rectangle: new Rectangle(20, 40, 100, 50),
						start: 0.075f, duration: 0.09f,
						force: new Vector2(2500, -500),
						hitStun: 0.5f, hitLag: 0.5f, shakeMagnitude: 10,
						onHitSounds: StrongHitSounds),
				});

			Attack specialStandingHeavy = new Attack(
				action: Actions.Heavy,
				aerialState: AerialStates.Standing,
				motionInput: new Actions[] { Actions.Left, Actions.Right, Actions.Heavy },
				chargeTime: 0.66f,
				startup: 0.2f, recovery: 1,
				staminaCost: 35,
				animation: new Animation(row: 19, frames: 4, fps: 1),
				hitboxes: new Hitbox[]
				{
	                new Hitbox(
	                	rectangle: new Rectangle(60, 0, 150, 45),
		                start: 0, duration: 0.15f,
		                force: new Vector2(3300, -400),
		                hitStun: 0.6f, hitLag: 0.6f, shakeMagnitude: 10,
						onHitSounds: StrongHitSounds,
						onAddedSounds: new SoundEffect[][] { SwingSounds, loudGrunts }),
				});

            Attack serveLight = new Attack(
				action: Actions.Light,
				aerialState: AerialStates.Standing,
				serve: true,
				startup: 0, recovery: 0.2f,
				staminaCost: 10,
				animation: new Animation(row: 19, frames: 4, fps: 1),
                onStartupSounds: new SoundEffect[][] { grunts },
				multiHit: true,
				hitboxes: new Hitbox[]
				{
	                new Hitbox(
	                	rectangle: new Rectangle(0, 0, 100, 100),
		                start: 0, duration: 0.05f,
		                force: new Vector2(0, -1300)),
	                new Hitbox(
	                	rectangle: new Rectangle(0, -50, 100, 100),
		                start: 0.8f, duration: 0.5f,
		                force: new Vector2(1600, -800),
						hitStun: 0.125f, hitLag: 0.125f, shakeMagnitude: 5,
						onHitSounds: NormalHitSounds,
						onAddedSounds: new SoundEffect[][] { SwingSounds, grunts })
                });

            Attack serveHeavy = new Attack(
				action: Actions.Heavy,
				aerialState: AerialStates.Standing,
				serve: true,
				startup: 0, recovery: 0.45f,
				staminaCost: 40,
				animation: new Animation(row: 19, frames: 4, fps: 1),
                onStartupSounds: new SoundEffect[][] { grunts },
				multiHit: true,
				hitboxes: new Hitbox[]
				{
	                new Hitbox(
	                	rectangle: new Rectangle(0, 0, 100, 100),
		                start: 0, duration: 0.05f,
		                force: new Vector2(0, -2000)),
	                new Hitbox(
	                	rectangle: new Rectangle(30, 0, 150, 45),
		                start: 1.65f, duration: 0.2f,
		                force: new Vector2(3250, -375),
		                hitStun: 0.6f, hitLag: 0.6f, shakeMagnitude: 10,
						onHitSounds: StrongHitSounds,
						onAddedSounds: new SoundEffect[][] { SwingSounds, loudGrunts }),
            	});

            Attack[] attacks = new Attack[]
            {
            	standingLight,
            	airborneLight,
            	specialAirborneMedium,
				standingMedium,
				airborneMedium,
            	specialStandingHeavy,
            	standingHeavy,
            	serveLight,
            	serveHeavy,
            };

			Stats stats = new Stats(
				gravity: 4500,
				friction: 2500,

				walkSpeed: 290,
				sprintSpeed: 650,
				exhaustedSpeed: 275,
				driftSpeed: 20,

				jumpSpeed: 1225,
				sprintingJumpSpeed: 1225,
				exhaustedJumpSpeed: 1100,
				fastFallSpeed: 1000,

				jumpSquat: 0.116f,
				exhaustedJumpSquat: 0.15f,
				turnDelay: 0.2f,

				staminaRegen: 5,
				exhaustedThreshold: 1,
				recoverThreshold: 5,
				walkStaminaCost: 1,
				sprintStaminaCost: 10,
				jumpStaminaCost: 5,
				exhaustedJumpStaminaCost: 2,
				enduranceDegen: 0.75f,
				staminaRecovery: 30);

			Character chino = new Character(
				"Chino",
				new Rectangle(0, 0, 192, 192),
				content.Load<Texture2D>("Characters/Chino/Spritesheet"),
				animations,
				attacks,
				stats,
				content.Load<SoundEffect>("Characters/Chino/Step"),
				content.Load<SoundEffect>("Characters/Chino/Jump"),
				content.Load<SoundEffect>("Characters/Chino/Turn"));

			Characters.Add(chino);
		}
	}
}
