using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Keybindings : MonoBehaviour
{
    public static Keybindings keybindings;
    public static List<KeyCode> reservedKeys;

    public static KeyCode Jump
    {
        get
        {
            return (KeyCode) Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("JumpKey", "Space"));
        }
        set
        {
            PlayerPrefs.SetString("JumpKey", value.ToString());
        }
    }

    public static KeyCode Slide
    {
        get
        {
            return (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("SlideKey", "Q"));
        }
        set
        {
            PlayerPrefs.SetString("SlideKey", value.ToString());
        }
    }

    public static KeyCode Reload
    {
        get
        {
            return (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("ReloadKey", "R"));
        }
        set
        {
            PlayerPrefs.SetString("ReloadKey", value.ToString());
        }
    }

    public static KeyCode Sprint
    {
        get
        {
            return (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("SprintKey", "LeftShift"));
        }
        set
        {
            PlayerPrefs.SetString("SprintKey", value.ToString());
        }
    }

    public static KeyCode Equip
    {
        get
        {
            return (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("EquipKey", "X"));
        }
        set
        {
            PlayerPrefs.SetString("EquipKey", value.ToString());
        }
    }

    public static KeyCode Drop
    {
        get
        {
            return (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("DropKey", "Z"));
        }
        set
        {
            PlayerPrefs.SetString("DropKey", value.ToString());
        }
    }

    public static KeyCode Hookshot
    {
        get
        {
            return (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("HookshotKey", "E"));
        }
        set
        {
            PlayerPrefs.SetString("HookshotKey", value.ToString());
        }
    }

    public static KeyCode Consume
    {
        get
        {
            return (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("ConsumeKey", "C"));
        }
        set
        {
            PlayerPrefs.SetString("ConsumeKey", value.ToString());
        }
    }

    public static KeyCode DropConsumable
    {
        get
        {
            return (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("DropConsumableKey", "V"));
        }
        set
        {
            PlayerPrefs.SetString("DropConsumableKey", value.ToString());
        }
    }

    void Update ()
    {
        reservedKeys = new List<KeyCode>{
            KeyCode.W, KeyCode.A, KeyCode.D, KeyCode.S,
            KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow,
            KeyCode.Mouse0,  KeyCode.Mouse1, KeyCode.None,
            KeyCode.LeftControl, KeyCode.RightControl,
            Jump, Slide, Sprint, Reload, Equip, Drop, Hookshot, Consume, DropConsumable
        };
    }
}
