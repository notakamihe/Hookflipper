using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Paused : MonoBehaviour
{
    public GameObject ui;

    private PanelHandler panelHandler;
    private PausePlayState pausePlay = PausePlayState.Unpaused;
    private LevelManager levelController;

    private enum PausePlayState
    {
        Unpaused,
        Paused
    }

    private void Awake()
    {
        ui.SetActive(false);
        levelController = GetComponentInParent<LevelManager>();
        panelHandler = GetComponentInParent<PanelHandler>();
        panelHandler.currentPanel = ui.gameObject;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && (!levelController.gameWon && !levelController.gameOver))
        {
            if (panelHandler.currentPanel != ui.gameObject)
                panelHandler.currentPanel.SetActive(false);
                panelHandler.currentPanel = ui.gameObject;

            pausePlay = (PausePlayState)(Mathf.Abs(~(int)pausePlay) % 2);  
        }

        switch (pausePlay)
        {
            case PausePlayState.Unpaused:
                Time.timeScale = 1f;
                AudioListener.pause = false;
                ui.SetActive(false);

                if (!levelController.gameWon && !levelController.gameOver)
                    Cursor.lockState = CursorLockMode.Locked;

                if (GameSingleton.instance.leftClickInputName == "None")
                {
                    StartCoroutine(ResetLeftClickInputName(1f));
                }

                break;
            case PausePlayState.Paused:
                if (panelHandler.currentPanel == ui.gameObject)
                    ui.SetActive(true);

                Cursor.lockState = CursorLockMode.None;
                GameSingleton.instance.leftClickInputName = "None";
                Time.timeScale = 0;
                AudioListener.pause = true;
                ui.GetComponent<Animation>()["PausedScreen"].time += .05f;

                break;
        }
    }

    private void OnDestroy()
    {
        AudioListener.pause = false;
        Time.timeScale = 1f;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Resume ()
    {
        pausePlay = PausePlayState.Unpaused;
    }

    public void MainMenu ()
    {
        SceneManager.LoadScene(0);
    }

    IEnumerator ResetLeftClickInputName (float delay)
    {
        yield return new WaitForSeconds(delay);
        GameSingleton.instance.leftClickInputName = "Shoot";
    }
}