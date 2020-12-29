using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHUD : ItemHUD
{
    private Weapon weapon;

    private new void Start()
    {
        base.Start();
        weapon = (Weapon)GetComponentInParent(typeof(Weapon));
    }

    private new void Update()
    {
        base.Update();
    }

    protected override bool ConditionForActive()
    {
        return Vector3.Distance(player.transform.position, weapon.transform.position) <= player.equipRange &&
            weapon.equipper == null;
    }
}
