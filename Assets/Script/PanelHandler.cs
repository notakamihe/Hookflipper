using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class PanelHandler : MonoBehaviour
{
    [HideInInspector] public GameObject currentPanel;
    public GameObject[] panels;

    public void ChangePanel (GameObject panel)
    {
        Array.ForEach(panels, p => p.SetActive(panel == p));
        currentPanel = panel;
    }
}