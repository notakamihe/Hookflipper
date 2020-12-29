using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class IfPlayerShotGun : IfPlayerCondition
{
    protected override bool Condition ()
    {
        Firearm playerFirearm = player.GetComponentInChildren<Firearm>();
        return playerFirearm && playerFirearm.shotsRemaining > 0 && Input.GetButtonDown(GameSingleton.instance.leftClickInputName);
    }
}
