using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [HideInInspector] public bool gameOver = false;
    [HideInInspector] public bool gameWon = false;

    private List<Enemy> allEnemies;
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private GameObject victoryScreen;
    private bool wonOrLostAlready = false;

    private void Awake()
    {
        if (SceneManager.GetActiveScene().name != "Tutorial")
            GameProgress.LastLevel = SceneManager.GetActiveScene().buildIndex;
    }

    // Start is called before the first frame update
    void Start()
    {
        allEnemies = GameSingleton.instance.allEnemies;
        deathScreen.SetActive(false);
        victoryScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver)
        {
            if (!deathScreen.activeSelf && !wonOrLostAlready)
            {
                deathScreen.SetActive(true);
                wonOrLostAlready = true;
            }
        } else if (gameWon)
        {
            if (!victoryScreen.activeSelf && !wonOrLostAlready)
            {
                wonOrLostAlready = true;
                Cursor.lockState = CursorLockMode.None;
                victoryScreen.SetActive(true);

                if (SceneManager.GetActiveScene().buildIndex == GameProgress.LevelLimit)
                    GameProgress.LevelLimit++;

                foreach (Enemy enemy in allEnemies)
                {
                    enemy.enabled = false;
                }
            }
        }
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void RestartGame ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
