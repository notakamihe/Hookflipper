using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FallingBridge : MonoBehaviour
{
    private AudioSource audioSource;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!rb.isKinematic && Physics.Raycast(transform.position, Vector3.down) &&
            new int[] { 4, 8 }.Contains(collision.gameObject.layer) && rb.velocity.magnitude > 30)
        {
            audioSource.Play();
        }
    }
}
