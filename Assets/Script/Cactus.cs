using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cactus : MonoBehaviour
{
    public float damage = 3f;

    private PlayerMovement player;
    private float prickTime;

    private void Update()
    {
        Collider findPlayer = Array.Find(Physics.OverlapSphere(transform.position, 2f),
            x => x.gameObject.GetComponent<PlayerMovement>());
        PlayerMovement player = findPlayer != null ? findPlayer.gameObject.GetComponent<PlayerMovement>() : null;
        
        if (player != null)
        {
            if (Time.time >= prickTime + 2f)
            {
                Prick(player);
                prickTime = Time.time;
            }
        } else
        {
            prickTime = 0;
        }
    }

    void Prick (PlayerMovement plyr)
    {
        plyr.health.TakeDamage(damage);

        Vector3 knockbackDirection = plyr.transform.position - transform.position;
        knockbackDirection.y = 0;

        StartCoroutine(plyr.AddForce(5, knockbackDirection));
    }
}
