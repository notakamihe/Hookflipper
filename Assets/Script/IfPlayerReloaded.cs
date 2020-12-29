using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class IfPlayerReloaded : IfPlayerCondition
{
    protected override bool Condition()
    {
        Firearm firearm = player.GetComponentInChildren<Firearm>();
        return firearm && Input.GetKeyDown(KeyCode.R);
    }
}
