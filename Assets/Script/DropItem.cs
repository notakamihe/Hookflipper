using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class DropItem : Objective
{
    public Transform target;

    private new void Start()
    {
        base.Start();
    }

    private void Update()
    {
        if (!target.IsChildOf(player.transform))
        {
            completed = true;
        }
    }
}