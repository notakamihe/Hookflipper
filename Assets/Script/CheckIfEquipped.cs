using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class CheckIfEquipped : Objective
{
    public string typeName;

    private void Update()
    {
        if (player.gameObject.GetComponentInChildren(Type.GetType(typeName)) != null)
            completed = true;
    }
}
