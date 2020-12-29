using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Brawler : Enemy
{
    public Fist[] fists;
    public float fightRadius = 3f;

    private BrawlerAnimations brawlerAnimation;

    private enum BrawlerAnimations
    {
        Idle,
        Run,
        Fight,
    }

    private new void Start()
    {
        base.Start();
        fists = GetComponentsInChildren<Fist>();
    }

    private new void Update()
    {
        base.Update();

        switch (alarmState)
        {
            case AlarmState.Dead:
                animator.SetBool("IsDead", true);
                this.enabled = false;
                break;
            case AlarmState.Unsuspecting:
                brawlerAnimation = BrawlerAnimations.Idle;
                break;
            case AlarmState.Investigating:
                brawlerAnimation = BrawlerAnimations.Run;
                CheckInvestigationComplete();
                break;
            case AlarmState.Detecting:
                LookAtTarget(player.transform.position);
                brawlerAnimation = DistanceFromPlayer() <= fightRadius ? BrawlerAnimations.Fight : BrawlerAnimations.Run;
                break;
        }

        switch (brawlerAnimation)
        {
            case BrawlerAnimations.Idle:
                animator.SetInteger("AnimState", 0);
                break;
            case BrawlerAnimations.Run:
                animator.SetInteger("AnimState", 1);
                characterController.Move(transform.forward * characterPhysics.speed * Time.deltaTime);
                break;
            case BrawlerAnimations.Fight:
                animator.SetInteger("AnimState", 2);
                Array.ForEach(fists, f => f.meleeState = Melee.MeleeState.Attacking);
                break;
        }

        if (brawlerAnimation != BrawlerAnimations.Fight)
            Array.ForEach(fists, f => f.meleeState = Melee.MeleeState.Idle);
    }
}