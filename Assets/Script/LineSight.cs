using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LineSight : MonoBehaviour
{
    public Transform eyePoint;
    public Vector3 lastKnownSighting;
    public float fieldOfView = 90f;
    public float detectRadius = 50f;
    public float nearDetectRadius = 3f;
    public bool targetDetected;

    private Transform target;

    // Start is called before the first frame update
    void Awake()
    {
        lastKnownSighting = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] sightSphere = Physics.OverlapSphere(transform.position, detectRadius, ~9);

        if (Array.Find(sightSphere, p => p.gameObject.GetComponent<PlayerMovement>()) != null)
        {
            target = Array.Find(sightSphere, p => p.gameObject.GetComponent<PlayerMovement>()).transform;
            UpdateSight();

            if (targetDetected)
            {
                lastKnownSighting = target.position;
            }
        } else
        {
            targetDetected = false;
        }
    }

    bool PlayerNear ()
    {
        return Physics.OverlapSphere(transform.position, nearDetectRadius).Any(x => x.gameObject.GetComponent<PlayerMovement>() != null);
    }

    bool InFOV ()
    {
        Vector3 direction = target.position - eyePoint.position;
        float angle = Vector3.Angle(eyePoint.forward, direction);
        return angle <= fieldOfView;
    }

    void UpdateSight ()
    {
        if (InFOV() || PlayerNear())
        {
            targetDetected = true;
        }
    }
}
