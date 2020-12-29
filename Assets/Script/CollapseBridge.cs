using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollapseBridge : MonoBehaviour
{
    private PlayerMovement player;
    [SerializeField] private float detectRadius;

    // Update is called once per frame
    void Update()
    {
        Collider playerCheck = Physics.OverlapSphere(transform.position, detectRadius,
            LayerMask.GetMask("Player")).FirstOrDefault();

        if (playerCheck != null)
        {
            foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
            {
                rb.isKinematic = false;
                rb.gameObject.layer = 0;
            }

            player = playerCheck.GetComponent<PlayerMovement>();

            player.state = PlayerMovement.State.Normal;
            player.speedWind.SetActive(false);
            player.physics.gravity = -9.81f;
            player.enabled = false;
        }

        if (player != null && player.GetComponent<PlayerMovement>().enabled == false)
        {
            if (Physics.SphereCast(player.transform.position, 1.5f, Vector3.down, out RaycastHit hit, 2f))
            {
                if (!hit.transform.IsChildOf(transform))
                {
                    player.GetComponent<PlayerMovement>().enabled = true;
                    this.enabled = false;
                }
            }
        }
    }
}
