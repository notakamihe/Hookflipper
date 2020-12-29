using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Musketeer : Enemy
{
    public Firearm firearm;
    public float shootRadius = 40f;
    public float shootDelay;

    private MusketeerAnimations musketeerAnimations;
    private object[] firearmTransfromOriginal;
    private object[] firearmTransfromIdleRun;

    private enum MusketeerAnimations
    {
        Idle,
        Run,
        Shooting
    }

    private new void Start()
    {
        base.Start();

        firearmTransfromOriginal = new object[] { new Vector3(.09f, 1.12f, -.49f), 
            new Quaternion(-0.237f,0.7964f,0.5494f,0.08724f) };
        firearmTransfromIdleRun = new object[] { new Vector3(0.219f, 0.589f, 0.98f), 
            new Quaternion(0.161f,-0.0148f,0.918f,-0.360f) };
    }

    private new void Update()
    {
        base.Update();

        if (firearm == null || !firearm.transform.IsChildOf(transform))
        {
            Brawler brawler = this.gameObject.AddComponent<Brawler>();
            animator.runtimeAnimatorController = GameSingleton.instance.primitiveAnimatorController;
            brawler.InitializeFields(chest, investigationDuration);
            firearm = null;
            Destroy(this);
        }

        switch (alarmState)
        {
            case AlarmState.Dead:
                animator.SetBool("IsDead", true);
                this.enabled = false;
                break;
            case AlarmState.Unsuspecting:
                musketeerAnimations = MusketeerAnimations.Idle;
                break;
            case AlarmState.Investigating:
                musketeerAnimations = MusketeerAnimations.Run;
                CheckInvestigationComplete();
                break;
            case AlarmState.Detecting:
                if (HorizontalDistanceFromPlayer(transform.position) > 2f)
                    LookAtTarget(player.transform.position);

                musketeerAnimations = DistanceFromPlayer() <= shootRadius ? MusketeerAnimations.Shooting : MusketeerAnimations.Run;
                break;
        }

        switch (musketeerAnimations)
        {
            case MusketeerAnimations.Idle:
                animator.SetInteger("AnimState", 0);

                if (firearm != null)
                {
                    firearm.transform.localPosition = (Vector3)firearmTransfromIdleRun[0];
                    firearm.transform.localRotation = (Quaternion)firearmTransfromIdleRun[1];
                }
                break;
            case MusketeerAnimations.Run:
                animator.SetInteger("AnimState", 1);
                characterController.Move(transform.forward * characterPhysics.speed * Time.deltaTime);

                if (firearm != null)
                {
                    if (firearm.transform.localPosition != (Vector3)firearmTransfromIdleRun[0])
                        firearm.transform.localPosition = (Vector3)firearmTransfromIdleRun[0];
                    if (firearm.transform.localRotation != (Quaternion)firearmTransfromIdleRun[1])
                        firearm.transform.localRotation = (Quaternion)firearmTransfromIdleRun[1];
                }
                break;
            case MusketeerAnimations.Shooting:
                animator.SetInteger("AnimState", 2);

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Shoot") &&
                    animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= .95)
                {
                    Vector3 shootTarget = player.transform.position;
                    StartCoroutine(Shoot(shootTarget, shootDelay));
                }

                if (firearm != null)
                {
                    firearm.transform.localPosition = (Vector3)firearmTransfromOriginal[0];
                    firearm.transform.localRotation = (Quaternion)firearmTransfromOriginal[1];
                }
                break;
        }
    }

    IEnumerator Shoot (Vector3 at, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (firearm != null && !IsDead())
            firearm.Shoot(at);
    }
}