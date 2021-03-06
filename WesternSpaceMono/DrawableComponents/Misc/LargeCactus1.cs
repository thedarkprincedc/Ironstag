﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WesternSpace.Collision;
using WesternSpace.Interfaces;
using WesternSpace.Utility;
using WesternSpace.AnimationFramework;
using WesternSpace.ServiceInterfaces;

namespace WesternSpace.DrawableComponents.Misc
{
    /// <summary>
    /// A large cactus that the player gets hurt walking into.
    /// </summary>
    public class LargeCactus1 : WorldObject, ISpriteCollideable, IDamaging
    {
        private Texture2D texture;

        private Rectangle boundingRectangle;

        /// <summary>
        /// Camera used to see if the enemy is visible.
        /// </summary>
        private ICameraService camera;

        public LargeCactus1(World world, SpriteBatch spriteBatch, Vector2 position)
            : base(world, spriteBatch, position)
        {
            boundingRectangle = new Rectangle((int)Position.X+11, (int)Position.Y+3, 25, 62);
            this.texture = ((ITextureService)ScreenManager.Instance.Services.GetService(typeof(ITextureService))).GetTexture("Textures\\Enemies\\LargeCactus1");
        }

        public override void Initialize()
        {
            camera = (ICameraService)this.Game.Services.GetService(typeof(ICameraService));
            base.Initialize();
        }

        public override void Draw(GameTime gameTime)
        {
            //if (!(this.Position.X > camera.VisibleArea.X + camera.VisibleArea.Width || this.Position.X + this.animationPlayer.Animation.FrameWidth < camera.VisibleArea.X))
            //{
            //Let the Animation Player Draw
            SpriteBatch.Draw(texture, Position, Color.White);
            //}
            base.Draw(gameTime);
        }

        #region ISpriteCollideable Members

        private int idNumber;
        int ISpriteCollideable.IdNumber
        {
            get
            {
                return idNumber;
            }
            set
            {
                idNumber = value;
            }
        }

        Rectangle ISpriteCollideable.Rectangle
        {
            get { return boundingRectangle; }
        }

        SpriteEffects ISpriteCollideable.collideableFacing
        {
            get
            {
                return SpriteEffects.None;
            }
            set
            {
                // A cactus shouldn't flip.
            }
        }

        void ISpriteCollideable.OnSpriteCollision(ISpriteCollideable characterCollidedWith)
        {
            // Nothing happens to the cactus on collisions.
        }

        #endregion
        #region ISpriteCollideable Members

        Boolean removeFromCollisionRegistration;
        public bool removeFromRegistrationList
        {
            get
            {
                return removeFromCollisionRegistration;
            }
            set
            {
                removeFromCollisionRegistration = value;
            }
        }

        #endregion
        #region IDamaging Members

        object IDamaging.Owner
        {
            get { return this; }
        }

        WesternSpace.Utility.DamageCategory IDamaging.DoesDamageTo
        {
            get { return DamageCategory.Player; }
        }

        float amountOfDamage = 1000;
        float IDamaging.AmountOfDamage
        {
            get { return amountOfDamage; }
            set { amountOfDamage = value; }
        }

        #endregion
    }
}
