using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class KillNEnemies : Objective
{
    public List<Health> enemyHealths;
    public int requiredEnemiesKilled;

    private int enemiesKilled = 0;

    private new void Start()
    {
        base.Start();

        if (enemyHealths.Count == 0)
            enemyHealths = GameSingleton.instance.allEnemies.Select(x => x.health).ToList();

        enemyHealths = enemyHealths.ToArray().ToList();
    }

    private void Update()
    {
        if (enemiesKilled >= requiredEnemiesKilled)
        {
            completed = true;
        }

        try
        {
            foreach (Health health in enemyHealths)
            {
                if (health != null)
                {
                    if (health == enemyHealths.OrderBy(e => Vector3.Distance(
                        player.transform.position, e.transform.position)).First())
                        transform.position = health.transform.position;

                    if (health.Depleted())
                    {
                        enemyHealths.Remove(health);
                        enemiesKilled++;
                    }
                }
            }
        }
        catch (InvalidOperationException ioe)
        {
            print($"Minor error resulting from collection modification. {ioe}: {ioe.Message}");
        }
    }
}
