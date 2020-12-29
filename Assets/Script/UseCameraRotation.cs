using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseCameraRotation : MonoBehaviour
{
    PlayerMovement player; 

    private void Start()
    {
        player = transform.parent.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = player.camera.transform.rotation;
    }
}
