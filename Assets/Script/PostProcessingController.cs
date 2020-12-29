using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingController : MonoBehaviour
{
    public PostProcessVolume volume;

    private void Start()
    {
        volume = GetComponent<PostProcessVolume>();
    }

    private void Update()
    {
        if (volume.profile.TryGetSettings(out MotionBlur motionBlur))
        {
            motionBlur.active = GamePreferences.MotionBlur;
        }
    }
}