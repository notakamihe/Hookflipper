using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wearable : MonoBehaviour
{
    public AudioSource wearSound;
    public Rigidbody rb;
    public WearItem itemType;
    public bool worn;

    public enum WearItem
    {
        Hat,
        Body,
        Legs
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        wearSound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        worn = transform.parent != null;
        rb.collisionDetectionMode = rb.isKinematic ? CollisionDetectionMode.ContinuousSpeculative : CollisionDetectionMode.ContinuousDynamic;
    }
}
