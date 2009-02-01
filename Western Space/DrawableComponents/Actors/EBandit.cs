﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using WesternSpace.Input;
using WesternSpace.ServiceInterfaces;
using WesternSpace.AnimationFramework;
using WesternSpace.Collision;
using WesternSpace.Screens;
using Microsoft.Xna.Framework.Audio;
using WesternSpace.Physics;
using WesternSpace.Utility;
using WesternSpace.DrawableComponents.Projectiles;

namespace WesternSpace.DrawableComponents.Actors
{
    class EBandit : Character
     {
        /// Constants ///
        private static readonly string BANDIT = "Bandit";

        /// <summary>
        /// The physics handler which takes care of the Bandit's physics.
        /// </summary>
        PhysicsHandler banditPhysics;

        /// <summary>
        /// Sound Effect will be moved later on.
        /// </summary>
        SoundEffect gunShot;

        /// <summary>
        /// Constructor for Flint Ironstag.
        /// </summary>
        /// <param name="parentScreen">The screen which this object is a part of.</param>
        /// <param name="spriteBatch">The sprite batch which handles drawing this object.</param>
        /// <param name="position">The initial position of this character.</param>
        /// <param name="xmlFile">The XML file which houses the information for this character.</param>
        public EBandit(Screen parentScreen , SpriteBatch spriteBatch, Vector2 position, String xmlFile)
            : base(parentScreen, spriteBatch, position, xmlFile)
        {
            //Set the character's Name
            name = "Flint Ironstag";

            //Load the player information from the XML file
            LoadBanditXmlFile(xmlFile);

            //Load the Player's Roles
            SetUpRoles(xmlFile);

            //Set current health
            currentHealth = maxHealth;

            //Instantiate the Physics Handler
            banditPhysics = new PhysicsHandler();

            //Create the Animation Player and give it the Idle Animation
            this.animationPlayer = new AnimationPlayer(spriteBatch, currentRole.AnimationMap["Idle"], currentRole.AnimationMap["Idle"]);

            //Set the current animation
            currentAnimation = animationPlayer.Animation;

            //Set the current state
            currentState = "Idle";

            //Set the Velocity
            velocity = new Vector2(0, 0);

            //Set the position
            this.Position = position;

            //Set the facing
            facing = SpriteEffects.FlipHorizontally;

            //Initializes the player's hotspots.
            this.collisionHotSpots.Add(new CollisionHotspot(this, new Vector2(16, 0), HOTSPOT_TYPE.top));
            this.collisionHotSpots.Add(new CollisionHotspot(this, new Vector2(0, 30), HOTSPOT_TYPE.left));
            this.collisionHotSpots.Add(new CollisionHotspot(this, new Vector2(36, 30), HOTSPOT_TYPE.right));
            this.collisionHotSpots.Add(new CollisionHotspot(this, new Vector2(7, 60), HOTSPOT_TYPE.bottom));
            //this.collisionHotSpots.Add(new CollisionHotspot(this, new Vector2(27, 60), HOTSPOT_TYPE.bottom));

            //Temp: Loads the gunshot sound.
            gunShot = this.Game.Content.Load<SoundEffect>("System\\Sounds\\flintShot");
        }

        /// <summary>
        /// Sets up the Character's individual Roles.
        /// </summary>
        /// <param name="xmlFile">The xml file containing the role information.</param>
        public override void SetUpRoles(string xmlFile)
        {
            Bandit bandit = new Bandit(xmlFile, BANDIT);

            this.roleMap.Add(BANDIT, bandit);

            this.currentRole = bandit;
        }

        /// <summary>
        /// Called when the player presses the jump button. If the player is already
        /// in a jumping state then no action is to occurr.
        /// </summary>
        public void Jump()
        {
            if (!currentState.Equals("Dead") && !currentState.Equals("Hit"))
            {
                if (!currentState.Contains("Jumping") && !currentState.Contains("Falling"))
                {
                    banditPhysics.ApplyJump();
                    ChangeState("JumpingAscent");
                    isOnGround = false;
                }
            }
        }

        /// <summary>
        /// Called when the player presses a movement button.
        /// </summary>
        public void Move()
        {
            int direction = 1;

            if (!currentState.Equals("Dead") && !currentState.Equals("Hit"))
            {
                //Calculate Facing
                if(facing.Equals(SpriteEffects.None))
                {
                    direction = 1;
                }
                else
                {
                    direction = -1;
                }

                if (isOnGround)
                {
                    banditPhysics.ApplyGroundMove(direction);
                    if (!currentState.Contains("Shooting"))
                    {
                        ChangeState("Running");
                    }
                    else if (!currentState.Contains("Up"))
                    {
                        ChangeState("RunningShooting");
                    }
                    else
                    {
                        ChangeState("RunningShootingUp");
                    }
                }
                else
                {
                    banditPhysics.ApplyAirMove(direction);
                }
            }
        }

        /// <summary>
        /// Causes the enemy to generate a projectile and change its state accordingly.
        /// </summary>
        public void Shoot()
        {
            if (!currentState.Equals("Dead") && !currentState.Equals("Hit"))
            {
                if (!currentState.Contains("Shooting"))
                {
                    if (currentState.Contains("Jumping"))
                    {
                        //Change state and animation
                        //ChangeState("JumpingShooting");
                    }
                    else if (currentState.Contains("Running"))
                    {
                        //Change state and animation
                        //ChangeState("RunningShooting");
                    }
                    else
                    {
                        //Change state and animation
                        ChangeState("Shooting");

                        gunShot.Play();
                    }

                    //Generate a Bullet
                    GenerateBullet();
                }
            }
        }

        /// <summary>
        /// Called every Update
        /// </summary>
        /// <param name="gameTime">The time the game has been running.</param>
        public override void Update(GameTime gameTime)
        {
            /// -- AI -- ///
            banditAI(gameTime);

            /// -- Handle Physics -- ///
            velocity = banditPhysics.ApplyPhysics(velocity);

            /// -- Update Position -- ///
            position += velocity;

            /// --Reset the Velocity -- ///
            banditPhysics.ResetVelocity();

            /// --- Check For Max Ascent of Jump -- ///
            if (currentState.Contains("Jumping"))
            {
                if ((-0.5 <= velocity.Y) || (velocity.Y <= 0.8))
                {
                    ChangeState("JumpingDescent");
                }
            }

            /// -- Check for Final State Changes -- ///
            if ((velocity.X == 0) && isOnGround && !currentState.Equals("Dead") && !currentState.Equals("Hit"))
            {

                if (animationPlayer.Animation.animationName.Equals("Idle") && !currentState.Equals("Idle"))
                {
                        ChangeState("Idle");
                }
                else if(!currentState.Contains("Shooting"))
                {
                    ChangeState("Idle");
                }
            }

            /// -- Animation Player Update Frames -- ///
            animationPlayer.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            //Let the Animation Player Draw
            animationPlayer.Draw(gameTime, this.SpriteBatch, this.Position, facing);
        }

        /// <summary>
        /// Loads a Character's information from a specified XML file.
        /// </summary>
        /// <param name="fileName">The name of the xml file housing the character's information.</param>
        private void LoadBanditXmlFile(string fileName)
        {
            //Create a new XDocument from the given file name.
            XDocument fileContents = ScreenManager.Instance.Content.Load<XDocument>(fileName);

            Int32.TryParse(fileContents.Root.Element("Health").Attribute("MaxHealth").Value, out this.maxHealth);
        }

        /// <summary>
        /// Logic which determines what a Bandit does.
        /// </summary>
        /// <param name="gameTime">The time the game has been running.</param>
        private void banditAI(GameTime gameTime)
        {
            float shootTimer = 0f, shootTimeSpan = 3.0f;

            shootTimer += (float)(gameTime.TotalRealTime.TotalSeconds%3.5);

            System.Diagnostics.Debug.WriteLine("Timer: " + shootTimer);

            if (shootTimer >= shootTimeSpan)
            {
                Shoot();
                shootTimer = 0f;
            }

        }

        /// <summary>
        /// Creates a bullet object which travels in a straight line.
        /// </summary>
        public void GenerateBullet()
        {
            short direction = 1;
            Vector2 position = this.Position + new Vector2(40f, 23f);

            if (this.Facing == SpriteEffects.FlipHorizontally)
            {
                direction = -1;
                position = this.Position + new Vector2(10f, 20f);
            }

            BanditNormalProjectile proj = new BanditNormalProjectile(this.ParentScreen, this.SpriteBatch, position, this, direction);
        }


        /// <summary>
        /// Called on a Sprite Collision?
        /// </summary>
        public void OnSpriteCollision()
        {
        }
    }
}