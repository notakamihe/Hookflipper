using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class PressKeyDuration : Objective
{
    public KeyCode key;
    public float pressDuration;

    private float timePressed;

    private void Update()
    {
        if (Input.GetKeyDown(key))
        {
            timePressed = Time.time;
        }

        if (Input.GetKey(key))
        {
            if (Time.time > timePressed + pressDuration)
            {
                completed = true;
            }
        }
    }
}