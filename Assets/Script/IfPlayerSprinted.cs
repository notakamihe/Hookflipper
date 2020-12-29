using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class IfPlayerSprinted : IfPlayerCondition
{
    protected override bool Condition()
    {
        return player.speedScale >= 2f && player.controller.velocity.magnitude >= 5f;
    }
}