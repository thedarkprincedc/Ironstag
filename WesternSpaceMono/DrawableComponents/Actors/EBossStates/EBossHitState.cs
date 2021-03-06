﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WesternSpace.DrawableComponents.Actors.EBossStates
{
    internal class EBossHitState : EBossState
    {
        internal EBossHitState(EBoss boss)
            : base(boss)
        {

        }

        internal override void Update()
        {
            if (!this.Boss.CurrentState.Contains("Dead"))
            {
                base.Update();

                this.Boss.ChangeState("Hit");
                IsLogicComplete = true;
            }
        }
    }
}
