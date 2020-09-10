﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TennisFightingGame
{
	public static partial class Assets
	{
		static partial void LoadWizard(ContentManager content)
		{
			SoundEffect[] swingSounds = new SoundEffect[]{ content.Load<SoundEffect>("Sounds/Characters/Swing1") };

			SoundEffect[] normalHitSounds = new SoundEffect[] { 
				content.Load<SoundEffect>("Sounds/Characters/Hit1"), 
				content.Load<SoundEffect>("Sounds/Characters/Hit2") 
				};

			SoundEffect[] strongHitSounds = normalHitSounds;

			SoundEffect[] weakHitSounds = new SoundEffect[] { content.Load<SoundEffect>("Sounds/Characters/Hit3") };

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

			Characters.Add(wizard);
		}
	}
}