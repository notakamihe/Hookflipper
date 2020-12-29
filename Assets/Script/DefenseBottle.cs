using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class DefenseBottle : Bottle
{
    public float defenseIncreaseAmount;
    public float defecseIncreaseDuration;

    protected override void PowerUp(PlayerMovement player)
    {
        GameSingleton.instance.CallCoroutine(player.BumpUpDefense(defenseIncreaseAmount, defecseIncreaseDuration));
    }
}
