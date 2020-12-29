using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Consumable : MonoBehaviour
{
    [HideInInspector] public GameObject equipper;
    [HideInInspector] public UseState useState { get; protected set; } = UseState.Unused;
    public Quaternion equipRotation;
    public Vector3 equipPosition;

    private Rigidbody rb;

    public enum UseState
    {
        Unused,
        Used
    }

    protected void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected void Update()
    {
        rb.isKinematic = equipper != null;
    }
}