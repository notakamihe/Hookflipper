using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueLabel : MonoBehaviour
{
    private Slider slider;
    private Transform label;

    private void Start()
    {
        slider = GetComponentInParent<Slider>();
        label = transform.GetChild(0);
    }

    private void Update()
    {
        label.GetComponentInChildren<Text>().text = slider.value.ToString();
    }

    public void OnPointerDown ()
    {
        label.gameObject.SetActive(true);
    }

    public void OnPointerUp ()
    {
        label.gameObject.SetActive(false);
    }

    
}