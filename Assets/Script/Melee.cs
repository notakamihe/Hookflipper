using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Melee : Weapon
{
    public MeleeState meleeState;
    public float damage;

    public enum MeleeState
    {
        Idle,
        Attacking,
    }

    private new void Start()
    {
        base.Start();
    }

    private new void Update()
    {
        base.Update();
    }
}
