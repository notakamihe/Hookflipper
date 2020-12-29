using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class IfPlayerAiming : IfPlayerCondition
{
    protected override bool Condition()
    {
        return player.aimState == PlayerMovement.AimState.Aiming;
    }
}