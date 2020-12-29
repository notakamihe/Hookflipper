using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Sniper : Enemy
{
    public Firearm firearm;
    public float shootRadius = 125f;
    public float shootDelay;

    private SniperAnimations sniperAnimation;

    

    private enum SniperAnimations
    {
        Idle,
        Run,
        Shooting
    }

    private new void Start()
    {
        base.Start();
    }

    private new void Update()
    {
        base.Update();

        if (firearm == null || !firearm.transform.IsChildOf(transform))
        {
            Brawler brawler = this.gameObject.AddComponent<Brawler>();
            animator.runtimeAnimatorController = GameSingleton.instance.primitiveAnimatorController;
            brawler.InitializeFields(chest, investigationDuration);
            Destroy(this);
        }

        switch (alarmState)
        {
            case AlarmState.Dead:
                animator.SetBool("IsDead", true);
                this.enabled = false;
                break;
            case AlarmState.Unsuspecting:
                sniperAnimation = SniperAnimations.Idle;
                break;
            case AlarmState.Investigating:
                sniperAnimation = SniperAnimations.Run;
                CheckInvestigationComplete();
                break;
            case AlarmState.Detecting:
                if (HorizontalDistanceFromPlayer(transform.position) > 2f)
                    LookAtTarget(player.transform.position);

                sniperAnimation = DistanceFromPlayer() <= shootRadius ? SniperAnimations.Shooting : SniperAnimations.Run;
                break;
        }

        switch (sniperAnimation)
        {
            case SniperAnimations.Idle:
                animator.SetInteger("AnimState", 0);
                break;
            case SniperAnimations.Run:
                animator.SetInteger("AnimState", 1);
                characterController.Move(transform.forward * characterPhysics.speed * Time.deltaTime);
                break;
            case SniperAnimations.Shooting:
                animator.SetInteger("AnimState", 2);

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Shoot") &&
                    animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= .95)
                {
                    Vector3 shootTarget = player.transform.position;
                    StartCoroutine(Shoot(shootTarget, shootDelay));
                }
                break;
        }
    }

    IEnumerator Shoot(Vector3 at, float delay)
    {
        yield return new WaitForSeconds(delay);
        firearm.Shoot(at);
    }
}