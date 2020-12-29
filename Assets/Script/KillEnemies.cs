using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class KillEnemies : Objective
{
    public List<Health> enemyHealths;

    private new void Start()
    {
        base.Start();
    }

    private void Update()
    {
        if (enemyHealths.All(h => h.Depleted()))
        {
            completed = true;
        } else
        {
            if (enemyHealths.Count > 0)
                transform.position = enemyHealths.Where(h => h != null && !h.Depleted()).OrderBy(
                    t => Vector3.Distance(t.transform.position, player.transform.position)).FirstOrDefault().transform.position;
        }
    }
}