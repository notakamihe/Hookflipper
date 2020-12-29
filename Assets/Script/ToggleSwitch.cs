using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class ToggleSwitch : MonoBehaviour
{
    private Image image;
    private Toggle toggle;
    [SerializeField] private Sprite onSwitch;
    [SerializeField] private Sprite offSwitch;

    private void Start()
    {
        image = GetComponent<Image>();
        toggle = GetComponentInParent<Toggle>();
    }

    private void Update()
    {
        image.sprite = toggle.isOn ? onSwitch : offSwitch;
    }
}