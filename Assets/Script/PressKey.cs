using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class PressKey : Objective
{
    public KeyCode keyToPress;

    private void Update()
    {
        if (Input.GetKeyDown(keyToPress))
            completed = true;
    }
}