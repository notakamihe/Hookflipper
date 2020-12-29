using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class HookshotSpeedBottle : Bottle
{
    public float hookshotSpeed;
    public float hookshotSpeedIncreaseDuration;

    protected override void PowerUp(PlayerMovement player)
    {
        GameSingleton.instance.CallCoroutine(player.hookshot.IncreaseHookshotSpeed(hookshotSpeed, hookshotSpeedIncreaseDuration));
    }
}
