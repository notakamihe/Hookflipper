using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class SetToggleOnAwake : MonoBehaviour
{
    public string functionName;
    private Toggle toggle;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        Invoke(functionName, 0);
    }

    private void ToggleToAimAssist ()
    {
        toggle.isOn = GamePreferences.AimAssist;
    }

    private void ToggleToAutoEquip ()
    {
        toggle.isOn = GamePreferences.AutoEquip;
    }

    private void ToggleToAutoReload ()
    {
        toggle.isOn = GamePreferences.AutoReload;
    }

    private void ToggleToMotionBlur()
    {
        toggle.isOn = GamePreferences.MotionBlur;
    }
}