using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class GameOptions : MonoBehaviour
{
    [SerializeField] private Dropdown resolutionDropdown;

    private void Awake()
    {
    }

    private void Start()
    {
        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(Screen.resolutions.Select(w => $"{w.width} by {w.height}").ToList());

        resolutionDropdown.value = Array.IndexOf(Screen.resolutions, Array.Find(
            Screen.resolutions, r => r.width == Screen.currentResolution.width &&
            r.height == Screen.currentResolution.height));

        resolutionDropdown.RefreshShownValue();
    }

    public void SetEnvironmentalSFX (float volume)
    {
        GamePreferences.EnvironmentalSFXVolume = (int)volume;
        SoundSingleton.instance.mixer.SetFloat("EnvironmentSFXVol", LinearToDecibel(
            (float)GamePreferences.EnvironmentalSFXVolume / 100));
    }

    public void SetGameSFX (float volume)
    {
        GamePreferences.GameSFXVolume = (int)volume;
        SoundSingleton.instance.mixer.SetFloat("GameSFXVol", LinearToDecibel(
            (float)GamePreferences.GameSFXVolume / 100));
    }

    public void SetLevelMusicVolume (float volume)
    {
        GamePreferences.LevelMusicVolume = (int)volume;
        SoundSingleton.instance.mixer.SetFloat("LevelMusicVolume", LinearToDecibel(
            (float)GamePreferences.LevelMusicVolume / 100));
    }

    public void SetMenuMusicVolume (float volume)
    {
        GamePreferences.MenuMusicVolume = (int) volume;
        SoundSingleton.instance.mixer.SetFloat("MenuMusicVolume", LinearToDecibel(
            (float)GamePreferences.MenuMusicVolume / 100));
    }

    public void SetPlayerAimSensitivity (float sensitivity)
    {
        GamePreferences.PlayerAimSensitivity = sensitivity;
    }

    public void SetPlayerFieldOfView (float fov)
    {
        GamePreferences.PlayerFOV = fov;
    }

    public void SetPlayerLookSensitivity (float sensitivity)
    {
        GamePreferences.PlayerLookSensitivity = sensitivity;
    }

    public void SetQuality (int index)
    {
        QualitySettings.SetQualityLevel(index);
    }

    public void SetResolution (int index)
    {
        Screen.SetResolution(Screen.resolutions[index].width, Screen.resolutions[index].height, 
            FullScreenMode.MaximizedWindow);
    }

    public void ToggleAimAssist (bool isAimAssistOn)
    {
        GamePreferences.AimAssist = isAimAssistOn;
    }

    public void ToggleAutoEquip (bool autoEquip)
    {
        GamePreferences.AutoEquip = autoEquip;
    }

    public void ToggleAutoReload (bool autoReload)
    {
        GamePreferences.AutoReload = autoReload;
    }

    public void ToggleMotionBlur (bool motionBlur)
    {
        GamePreferences.MotionBlur = motionBlur;
    }

    private float LinearToDecibel (float linear)
    {
        float dB;

        if (linear != 0)
            dB = 20.0f * Mathf.Log10(linear);
        else
            dB = -144.0f;

        return dB;
    }
}