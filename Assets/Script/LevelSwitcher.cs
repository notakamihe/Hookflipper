using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSwitcher : MonoBehaviour
{
    public Button[] levels;

    private void Update()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].interactable = i + 2 <= GameProgress.LevelLimit;
        }
    }

    public void GoToScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}