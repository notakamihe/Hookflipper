using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KeyManager : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public string keyName;
    public string keyNameDefault;

    private Text buttonText;
    private bool waitingForKey;

    // Start is called before the first frame update
    void Start()
    {
        waitingForKey = false;

        buttonText = GetComponentInChildren<Text>();
        buttonText.text = ((KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(
            keyName, keyNameDefault))).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        KeyCode newKey = Array.Find(Enum.GetValues(typeof(KeyCode)).OfType<KeyCode>().ToArray(), 
            k => Input.GetKeyDown(k));

        if (waitingForKey && !Keybindings.reservedKeys.Contains(newKey))
        {
            PlayerPrefs.SetString(keyName, newKey.ToString());
            buttonText.text = newKey.ToString();
        }
    }

    public void OnDeselect(BaseEventData data)
    {
        waitingForKey = false;
    }

    public void OnSelect(BaseEventData data)
    {
        waitingForKey = true;
    }
}
