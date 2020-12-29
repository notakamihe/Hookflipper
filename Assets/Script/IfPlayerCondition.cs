using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class IfPlayerCondition : Objective
{
    private void Update()
    {
        if (Condition())
        {
            completed = true;
        }
    }

    protected virtual bool Condition ()
    {
        return true;
    }
}