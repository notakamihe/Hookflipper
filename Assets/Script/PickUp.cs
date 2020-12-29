using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class PickUp : Objective
{
    public Transform target;

    private new void Start()
    {
        base.Start();
    }

    private void Update()
    {
        transform.position = target.position;

        if (target.IsChildOf(player.transform))
        {
            completed = true;
        }
    }
}