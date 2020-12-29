using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthUI : MonoBehaviour
{
    private Health playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = FindObjectOfType<PlayerMovement>().GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(1, 1, 1);
    }

    private void LateUpdate()
    {
        transform.localScale = new Vector3(
            transform.localScale.x,
            (transform.localScale.y / playerHealth.maxHealth) * playerHealth.health,
            transform.localScale.z
        );
    }
}
