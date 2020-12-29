using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private GameObject activePanel;

    // Start is called before the first frame update
    void Start()
    {
        activePanel = transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(child.gameObject == activePanel);
        }
    }

    public void Play ()
    {
        if (!GameProgress.HasPlayedTutorial)
        {
            SceneManager.LoadScene(1);
            GameProgress.HasPlayedTutorial = true;
        }
        else
        {
            SceneManager.LoadScene(GameProgress.LastLevel);
        }
    }

    public void RollCredits ()
    {
        SceneManager.LoadScene("Credits");
    }

    public void TransitionPanels (GameObject panel)
    {
        activePanel = panel;
    }
}
