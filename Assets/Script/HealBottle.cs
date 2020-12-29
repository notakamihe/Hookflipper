using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class HealBottle : Bottle
{
    public float healAmount;

    protected override void PowerUp (PlayerMovement player)
    {
        player.health.Heal(healAmount);
    }
}
