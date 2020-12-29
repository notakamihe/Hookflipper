using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    protected AudioSource audioSource;
    [SerializeField] protected AudioMixer mixer;

    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    protected void Start()
    {
        mixer.SetFloat("MenuMusicVolume", LinearToDecibel((float)GamePreferences.MenuMusicVolume / 100));
        mixer.SetFloat("LevelMusicVolume", LinearToDecibel((float)GamePreferences.LevelMusicVolume / 100));
        mixer.SetFloat("GameSFXVol", LinearToDecibel((float)GamePreferences.GameSFXVolume / 100));
        mixer.SetFloat("EnvironmentSFXVol", LinearToDecibel((float)GamePreferences.EnvironmentalSFXVolume / 100));
    }

    private float LinearToDecibel(float linear)
    {
        float dB;

        if (linear != 0)
            dB = 20.0f * Mathf.Log10(linear);
        else
            dB = -144.0f;

        return dB;
    }
}
