using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinBullet : MonoBehaviour
{
    public float forwardSpin = 15;
    public float upSpin;
    public float rightSpin;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.right * rightSpin);
        transform.Rotate(Vector3.up * upSpin);
        transform.Rotate(Vector3.forward * forwardSpin);
    }
}
