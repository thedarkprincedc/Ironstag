﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WesternSpace.DrawableComponents.Actors.EBossStates
{
    internal class EBossJumpState : EBossState
    {
        /// <summary>
        /// How high above the boss the player needs to be to choose this
        /// AI state
        /// </summary>
        private const float PLAYER_Y_THRESHOLD = 20.0f;

        internal EBossJumpState(EBoss boss)
            : base(boss)
        {
        }

        internal override void Update()
        {
            base.Update();

            Jump();
        }

        internal bool ShouldBossJump()
        {
            if ((this.Boss.CurrentState.Contains("Laughing") || !this.Boss.CurrentState.Contains("Idle")) 
                && this.Boss.isOnGround && this.Boss.World.Player.Position.Y < (this.Boss.Position.Y - PLAYER_Y_THRESHOLD))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Called from the AI. If the boss is already
        /// in a jumping state then no action is to occurr.
        /// </summary>
        private void Jump()
        {
            if (!this.Boss.CurrentState.Contains("Dead") && !this.Boss.CurrentState.Equals("Hit"))
            {
                if (!this.Boss.CurrentState.Contains("Jumping") && !this.Boss.CurrentState.Contains("Falling"))
                {
                    this.Boss.ApplyJump();
                    this.Boss.ChangeState("JumpingAscent");
                    this.Boss.isOnGround = false;
                }
            }
        }
    }
}
