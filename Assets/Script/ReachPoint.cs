using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class ReachPoint : Objective
{
    public float reachRange;

    private new void Start()
    {
        base.Start();
    }

    private void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) <= reachRange)
            completed = true;
    }
}