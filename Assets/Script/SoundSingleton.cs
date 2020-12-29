using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundSingleton : MonoBehaviour
{
    public static SoundSingleton instance;

    public AudioMixer mixer;
    public SoundManager soundManager;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }
}
