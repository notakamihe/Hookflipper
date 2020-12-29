using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Firearm firearm;
    public Rigidbody rb;
    public float damage = 10f;
    public float lifetime = 3f;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start ()
    {
        Destroy(this.gameObject, lifetime);
    }

    void OnCollisionEnter (Collision col)
    {

        if (col.gameObject.GetComponent<Firearm>() == null)
        {
            rb.useGravity = true;
        }   
    }
}
