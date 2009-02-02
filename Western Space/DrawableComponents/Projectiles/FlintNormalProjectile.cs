﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WesternSpace.AnimationFramework;
using WesternSpace.Screens;
using WesternSpace.Utility;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using WesternSpace.Physics;

namespace WesternSpace.DrawableComponents.Projectiles
{
    /// <summary>
    /// The projectile that Flint fires at the player while in normal form
    /// </summary>
    public class FlintNormalProjectile : Projectile
    {
        /// <summary>
        /// The animation object to to use for animating this projectile
        /// </summary>
        private static readonly Animation FlintProjectileAnimation = new Animation("ActorXML\\AnimationXML\\FlintBullet", "Bullet");

        /// <summary>
        /// The amount of damage a single projectile does to the player
        /// </summary>
        private static readonly int Damage = 50;

        /// <summary>
        /// The speed of the projectile
        /// </summary>
        private static readonly Vector2 Velocity = new Vector2(6f, 0f);

        /// <summary>
        /// Creates a new Flint bullet projectile and adds it to the screen
        /// </summary>
        /// <param name="screen">The screen that this projectile belongs to</param>
        /// <param name="batch">The sprite batch object used to draw this projectile</param>
        /// <param name="position">The position of the projectile in world coordinates</param>
        /// <param name="owner">The character that fired this projectile, in this case the player</param>
        /// <param name="direction">The direction that is projectile is moving. 1 to the right, -1 to left</param>
        public FlintNormalProjectile(Screen screen, SpriteBatch batch, Vector2 position, object owner, short direction)
            : base(screen, batch, position, FlintNormalProjectile.FlintProjectileAnimation, direction, FlintNormalProjectile.Velocity,
                owner, DamageCategory.Enemy, FlintNormalProjectile.Damage)
        {
        }
    }
}
