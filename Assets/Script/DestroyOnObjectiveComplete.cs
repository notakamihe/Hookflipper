using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class DestroyOnObjectiveComplete : MonoBehaviour
{
    public Objective objective;

    private void Update()
    {
        if (objective.completed)
            Destroy(this.gameObject);
    }
}