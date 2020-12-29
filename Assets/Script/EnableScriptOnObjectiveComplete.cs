using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class EnableScriptOnObjectiveComplete : MonoBehaviour
{
    public Behaviour script;
    public Objective objective;

    private void Update()
    {
        if (objective.completed)
        {
            script.enabled = true;
            Destroy(this);
        }
    }
}
