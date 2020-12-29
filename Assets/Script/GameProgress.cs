using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class GameProgress : MonoBehaviour
{
    public static int LevelLimit
    {
        get
        {
            return PlayerPrefs.GetInt("LvlLimit", 2);
        }
        set
        {
            PlayerPrefs.SetInt("LvlLimit", value);
        }
    }

    public static int LastLevel
    {
        get
        {
            return PlayerPrefs.GetInt("LastLevel", 2);
        }
        set
        {
            PlayerPrefs.SetInt("LastLevel", value);
        }
    }

    public static bool HasPlayedTutorial
    {
        get
        {
            return PlayerPrefs.GetInt("HasPlayedTutorial", 0) != 0;
        }
        set
        {
            PlayerPrefs.SetInt("HasPlayedTutorial", Convert.ToInt16(value));
        }
    }
}
