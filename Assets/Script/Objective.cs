using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Objective : MonoBehaviour
{
    public string title;
    public string description;
    public bool isOptional;
    public bool completed;

    protected PlayerMovement player;

    private ObjectiveUI ui;
    [SerializeField] private Text tutorialText;

    // Start is called before the first frame update
    protected void Start()
    {
        ui = GetComponentInChildren<ObjectiveUI>();
        player = GameSingleton.instance.player;

        if (ui)
        {
            ui.title.text = SpaceLetters(title);
            ui.description.text = description;
        }

        if (tutorialText)
            tutorialText.text = description.ToUpper();
    }

    string SpaceLetters (string str)
    {
        return string.Join("  ", str.ToUpper().ToCharArray());
    }
}
