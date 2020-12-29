using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveCompass : MonoBehaviour
{
    private Image image;
    [SerializeField] private Sprite arrow;
    [SerializeField] private Sprite circle;
    [SerializeField] private ObjectiveHandler objectiveHandler;
    [SerializeField] private Transform player;

    private void Start()
    {
        image = GetComponent<Image>();
        objectiveHandler = FindObjectOfType<ObjectiveHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (objectiveHandler != null && objectiveHandler.activeObjective != null)
        {
            image.enabled = true;
            Vector3 directionToObjective = objectiveHandler.activeObjective.transform.position - player.position;
            directionToObjective.y = 0;

            if (Vector3.Distance(objectiveHandler.activeObjective.transform.position, player.position) <= 5)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                image.sprite = circle;
            } else
            {
                transform.rotation = Quaternion.Euler(
                    transform.rotation.eulerAngles.x,
                    transform.rotation.eulerAngles.y,
                    Vector3.SignedAngle(directionToObjective, player.forward, Vector3.up));

                image.sprite = arrow;
            }  
        } else
        {
            image.enabled = false;
        }
    }
}
