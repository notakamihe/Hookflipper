using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Bottle : Consumable
{
    public static Vector3 equipScale = new Vector3(0.65221f, 0.64798f, 0.51988f);

    protected Animator animator;

    protected void Awake()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

    private new void Update()
    {
        base.Update();

        if (equipper)
        {
            transform.localScale = equipper.GetComponent<PlayerMovement>() ? equipScale : new Vector3(1, 1, 1);

            if (equipper.TryGetComponent(out PlayerMovement player))
            {
                UseConsumable(player);
            }
        }
    }

    protected void UseConsumable (PlayerMovement player)
    {
        transform.localPosition = equipPosition;
        transform.localRotation = equipRotation;

        if (useState == UseState.Used)
        {
            animator.SetBool("Consume", true);

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Consume") && 
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= .99f)
            {
                animator.SetBool("Consume", false);
                player.DropConsumable(this);
                Destroy(this.gameObject, 10f);
            }
        }
        else
        {
            if (Input.GetKeyDown(Keybindings.DropConsumable))
            {
                player.DropConsumable(this);
            }

            if (Input.GetKeyDown(Keybindings.Consume))
            {
                player.sip.Play();
                useState = UseState.Used;
                PowerUp(player);
            }
        }
    }

    protected virtual void PowerUp (PlayerMovement player)
    {
    }
}