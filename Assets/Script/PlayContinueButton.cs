using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class PlayContinueButton : MonoBehaviour
{
    public Text text;

    private void Start()
    {
        text = GetComponentInChildren<Text>();
    }

    private void Update()
    {
        text.text = !GameProgress.HasPlayedTutorial ? "PLAY" : "CONTINUE";
    }
}