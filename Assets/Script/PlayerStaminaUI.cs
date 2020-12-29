using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStaminaUI : MonoBehaviour
{
    private Stamina playerStamina;

    // Start is called before the first frame update
    void Start()
    {
        playerStamina = FindObjectOfType<PlayerMovement>().GetComponent<Stamina>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(1, 1, 1);
    }

    void LateUpdate()
    {
        transform.localScale = new Vector3(
            transform.localScale.x,
            (transform.localScale.y / playerStamina.maxStamina) * playerStamina.stamina,
            transform.localScale.z
        );
    }
}
