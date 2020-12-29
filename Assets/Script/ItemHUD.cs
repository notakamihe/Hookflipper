using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class ItemHUD : MonoBehaviour
{
    protected PlayerMovement player;
    private GameObject ui;

    protected void Start()
    {
        player = GameSingleton.instance.player;
        ui = transform.GetChild(0).gameObject;
    }

    protected void Update()
    {
        if (ConditionForActive())
        {
            ui.SetActive(true);
            transform.LookAt(player.transform);
        }
        else
            ui.SetActive(false);
    }

    protected virtual bool ConditionForActive ()
    {
        return true;
    }
}