using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class IfPlayerConsume : IfPlayerCondition
{
    protected override bool Condition()
    {
        Consumable consumable = player.GetComponentInChildren<Consumable>();
        return consumable && Input.GetKeyDown(Keybindings.Consume);
    }
}