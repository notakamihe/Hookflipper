using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class ConsumableHUD : ItemHUD
{
    private Consumable consumable;

    private new void Start()
    {
        base.Start();
        consumable = (Consumable)GetComponentInParent(typeof(Consumable));
    }

    private new void Update()
    {
        base.Update();
    }

    protected override bool ConditionForActive ()
    {
        return Vector3.Distance(player.transform.position, consumable.transform.position) <= player.equipRange &&
            consumable.equipper == null && consumable.useState != Consumable.UseState.Used;
    }
}