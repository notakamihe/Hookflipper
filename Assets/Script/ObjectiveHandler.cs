using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveHandler : MonoBehaviour
{
    [HideInInspector] public Objective activeObjective;
    public Objective[] objectives;

    private AudioSource objectiveSound;
    private LevelManager gameController;
    private int activeObjectiveIdx = 0;
    
    private void Start()
    {
        gameController = FindObjectOfType<LevelManager>();
        objectiveSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (activeObjective != null)
        {
            if (activeObjectiveIdx < objectives.Length)
            {
                if (activeObjective.completed)
                {
                    activeObjectiveIdx++;
                    objectiveSound.Play();
                }
            }
        }

        for (int i = 0; i < objectives.Length; i++)
        {
            if (i != activeObjectiveIdx)
            {
                objectives[i].gameObject.SetActive(false);
            } else
            {
                objectives[i].gameObject.SetActive(true);
                activeObjective = objectives[i];
            }
        }

        if (activeObjectiveIdx == objectives.Length)
        {
            print("You have won.");
            gameController.gameWon = true;
            this.gameObject.SetActive(false);
        }
    }
}
