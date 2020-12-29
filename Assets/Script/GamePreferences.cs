using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class GamePreferences : MonoBehaviour
{
    public static bool AimAssist
    {
        get
        {
            return PlayerPrefs.GetInt("AimAssist") != 0;
        }
        set
        {
            PlayerPrefs.SetInt("AimAssist", Convert.ToInt16(value));
        }
    }

    public static bool AutoEquip
    {
        get
        {
            return PlayerPrefs.GetInt("AutoEquip", 1) != 0;
        }
        set
        {
            PlayerPrefs.SetInt("AutoEquip", Convert.ToInt16(value));
        }
    }

    public static bool AutoReload
    {
        get
        {
            return PlayerPrefs.GetInt("AutoReload", 1) != 0;
        }
        set
        {
            PlayerPrefs.SetInt("AutoReload", Convert.ToInt16(value));
        }
    }

    public static int EnvironmentalSFXVolume
    {
        get
        {
            return PlayerPrefs.GetInt("SFXVolumeEnvironmental", 100);
        }
        set
        {
            PlayerPrefs.SetInt("SFXVolumeEnvironmental", value);
        }
    }

    public static int GameSFXVolume
    {
        get
        {
            return PlayerPrefs.GetInt("SFXVolume", 100);
        }
        set
        {
            PlayerPrefs.SetInt("SFXVolume", value);
        }
    }

    public static int LevelMusicVolume
    {
        get
        {
            return PlayerPrefs.GetInt("LevelMusicVol", 50);
        }
        set
        {
            PlayerPrefs.SetInt("LevelMusicVol", value);
        }
    }

    public static int MenuMusicVolume
    {
        get
        {
            return PlayerPrefs.GetInt("MenuMusicVol", 50); 
        }
        set
        {
            PlayerPrefs.SetInt("MenuMusicVol", value);
        }
    }

    public static bool MotionBlur
    {
        get
        {
            return PlayerPrefs.GetInt("MotionBlur") != 0;
        }
        set
        {
            PlayerPrefs.SetInt("MotionBlur", Convert.ToInt32(value));
        }
    }

    public static float PlayerAimSensitivity
    {
        get
        {
            return PlayerPrefs.GetFloat("AimSensitivity", 50f);
        }
        set
        {
            PlayerPrefs.SetFloat("AimSensitivity", value);
        }
    }

    public static float PlayerFOV
    {
        get
        {
            return PlayerPrefs.GetFloat("PlayerFov", 90f);
        }
        set
        {
            PlayerPrefs.SetFloat("PlayerFov", value);
        }
    }

    public static float PlayerLookSensitivity
    {
        get
        {
            return PlayerPrefs.GetFloat("LookSensitivity", 300f);
        }
        set
        {
            PlayerPrefs.SetFloat("LookSensitivity", value);
        }
    }
}