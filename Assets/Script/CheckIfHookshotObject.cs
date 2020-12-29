using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class CheckIfHookshotObject : Objective
{
    public List<GameObject> objectsToCheckHookshot;

    private new void Start()
    {
        base.Start();
    }

    private void Update()
    {
        try
        {
            foreach (GameObject obj in objectsToCheckHookshot)
            {
                if (obj == player.hookshot.objectToHookshot)
                {
                    objectsToCheckHookshot.Remove(obj);
                }
            }
        }
        catch (InvalidOperationException ioe)
        {
            print($"Minor error  -->  {ioe}:  {ioe.Message}");
        }

        if (objectsToCheckHookshot.Count == 0)
            completed = true;
    }
}