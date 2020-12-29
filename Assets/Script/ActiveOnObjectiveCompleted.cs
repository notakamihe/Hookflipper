using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActiveOnObjectiveCompleted : MonoBehaviour
{
    public Objective objective;

    private Transform[] objectsToActivate;

    // Start is called before the first frame update
    void Start()
    {
        objectsToActivate = GetComponentsInChildren<Transform>().Skip(1).ToArray();
        Array.ForEach(objectsToActivate, o => o.gameObject.SetActive(false));
    }

    // Update is called once per frame
    void Update()
    {
        if (objective.completed)
        {
            Array.ForEach(objectsToActivate, o => o.gameObject.SetActive(true));
            this.enabled = false;
        }
        
    }
}
