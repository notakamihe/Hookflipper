using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class SetSliderOnStart : MonoBehaviour
{
    public string functionName;
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        Invoke(functionName, 0);
    }

    private void SetSliderToAimSensitivity ()
    {
        slider.value = GamePreferences.PlayerAimSensitivity;
    }

    private void SetSliderToFOV ()
    {
        slider.value = GamePreferences.PlayerFOV;
    }

    private void SetSliderToEnvironmentalSFX()
    {
        slider.value = GamePreferences.EnvironmentalSFXVolume;
    }

    private void SetSliderToGameSFX()
    {
        slider.value = GamePreferences.GameSFXVolume;
    }

    private void SetSliderToLevelMusicVolume ()
    {
        slider.value = GamePreferences.LevelMusicVolume;
    }

    private void SetSliderToMenuMusicVolume ()
    {
        slider.value = GamePreferences.MenuMusicVolume;
    }

    private void SetSliderToLookSensitivity ()
    {
        slider.value = GamePreferences.PlayerLookSensitivity;
    }
}