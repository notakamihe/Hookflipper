using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Component = UnityEngine.Component;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    public float health = 100f;

    public bool Depleted ()
    {
        return health <= 0;
    }

    public void Heal (float amount)
    {
        health = Mathf.Clamp(health + amount, 0, maxHealth);
    }

    public void Kill ()
    {
        TakeDamage(health);
    }

    public void TakeDamage (float damage)
    {
        health = health - damage > 0 ? health - damage : 0;
    }
}
