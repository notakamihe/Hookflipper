using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class AimAssist : MonoBehaviour
{
    public float maxVectorOffset;

    private MouseLook mouseLook;

    private void Start()
    {
        mouseLook = GetComponent<MouseLook>();
    }
}