﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bandit : MonoBehaviour
{
    public Enemy enemy;
    public GameObject hand;
    public Gun gun;
    public float shootRadius = 15f;
    private float lastShot;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < hand.transform.childCount; i++)
        {
            GameObject child = hand.transform.GetChild(i).gameObject;

            if (child.GetComponent<Gun>() != null)
            {
                gun = child.GetComponent<Gun>();
                gun.equipped = true;
                gun.equippedBy = this.gameObject;
            }
        }


        float distance = Vector3.Distance(enemy.target.position, transform.position);

        if (distance < enemy.detectRadius)
        {
            enemy.agent.SetDestination(enemy.target.position);

            if (distance > shootRadius)
            {
                print("MOVE");
                enemy.controller.Move(transform.forward * (enemy.physics.speed/2) * Time.deltaTime); 
            }

            if (distance <= shootRadius)
            {
                Shoot();
            }
                
            
        }

        if (enemy.controller.velocity.magnitude > 1)
        {
            enemy.animator.SetInteger("MotionState", 1);
        } else
        {
            if (enemy.animator.GetInteger("MotionState") != 3)
            {
                enemy.animator.SetInteger("MotionState", 0);
            }
        }
    }

    void Shoot ()
    {
        print("Aim");
        enemy.animator.SetInteger("MotionState", 3);

        if (gun != null)
        {
            if (Time.time >= lastShot + gun.fireRate && gun.shotsRemaining > 0)
            {
                gun.Shoot();
                lastShot = Time.time;
            }
        }
    }
}