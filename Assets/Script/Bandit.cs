using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Bandit : Enemy
{
    public Firearm firearm;
    public BanditAnimations banditAnimations;
    public float shootRadius = 35f;
    public float shootDelay;

    public enum BanditAnimations
    {
        Idle,
        Run,
        Shooting,
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
                banditAnimations = BanditAnimations.Idle;
                break;
            case AlarmState.Investigating:
                banditAnimations = BanditAnimations.Run;
                CheckInvestigationComplete();
                break;
            case AlarmState.Detecting:
                if (HorizontalDistanceFromPlayer(transform.position) > 2f)
                    LookAtTarget(player.transform.position);

                banditAnimations = DistanceFromPlayer() <= shootRadius ? BanditAnimations.Shooting : BanditAnimations.Run;
                break;
        }

        switch (banditAnimations)
        {
            case BanditAnimations.Idle:
                animator.SetInteger("AnimState", 0);
                break;
            case BanditAnimations.Run:
                animator.SetInteger("AnimState", 1);
                characterController.Move(transform.forward * characterPhysics.speed * Time.deltaTime);
                break;
            case BanditAnimations.Shooting:
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

        if (!IsDead())
            firearm.Shoot(at);
    }
}