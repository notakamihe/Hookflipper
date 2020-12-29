using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class PressKeysDuration : Objective
{
    public List<KeyCode> keysToPress;
    public float pressTime;

    private KeyCode currentKey;
    private float timePressed;

    private void Update()
    {
        try
        {
            foreach (KeyCode key in keysToPress)
            {
                if (Input.GetKeyDown(key))
                {
                    timePressed = Time.time;
                    currentKey = key;
                }

                if (currentKey == key)
                {
                    if (Input.GetKey(key))
                    {
                        if (Time.time > timePressed + pressTime)
                        {
                            keysToPress.Remove(key);
                            timePressed = 0;
                        }
                    }
                    else
                    {
                        timePressed = 0;
                        currentKey = KeyCode.None;
                    }
                }
            }
        } catch (InvalidOperationException ioe)
        {
            print($"Minor error  --> {ioe}: {ioe.Message} ");
        }

        if (keysToPress.Count == 0)
            completed = true;
    }
}