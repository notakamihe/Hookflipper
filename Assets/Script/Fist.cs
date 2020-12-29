using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Fist : Melee
{
    private AudioSource audioSource;

    private new void Start()
    {
        base.Start();
        audioSource = GetComponent<AudioSource>();
    }

    private new void OnTriggerEnter(Collider collider)
    {
        base.Update();

        if (!transform.IsChildOf(collider.transform) && meleeState == MeleeState.Attacking)
        {
            audioSource.Play();

            if (collider.TryGetComponent(out Health health) && collider.TryGetComponent(out DamageHandler damageHandler))
            {
                health.TakeDamage((damage + equipper.attackDamageMelee) * damageHandler.defenseModifier);
            }
        }
    }
}