using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class IfPlayerDropConsumable : IfPlayerCondition
{
    protected override bool Condition()
    {
        return Input.GetKey(Keybindings.DropConsumable);
    }
}