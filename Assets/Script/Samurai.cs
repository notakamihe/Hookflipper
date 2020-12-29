using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Samurai : Enemy
{
    public SamuraiAnimations samuaraiAnimation;

    private Katana katana;
    private float attackTime;
    private float attackCooldown;
    private object[] katanaTransformOriginal;
    private object[] katanaTransformIdle = new object[2];

    public enum SamuraiAnimations
    {
        Idle,
        Run,
        Swing,
        Slash,
        Stab,
    }

    private void Awake()
    {
        katana = GetComponentInChildren<Katana>();
    }

    private new void Start()
    {
        base.Start();
        katanaTransformOriginal = new object[] { katana.transform.localPosition, katana.transform.localRotation };
        katanaTransformIdle = new object[] { new Vector3(-0.136f,-0.497f,-0.118f), 
            new Quaternion(-0.116f, -0.987f, 0.106f, -0.016f) };
    }

    private new void Update()
    {
        base.Update();

        if (katana == null || !katana.transform.IsChildOf(transform))
        {
            Brawler brawler = this.gameObject.AddComponent<Brawler>();
            animator.runtimeAnimatorController = GameSingleton.instance.primitiveAnimatorController;
            brawler.InitializeFields(chest, investigationDuration);
            katana = null;
            Destroy(this);
        }

        switch (alarmState)
        {
            case AlarmState.Dead:
                animator.SetBool("Dead", true);
                katana.meleeState = Melee.MeleeState.Idle;
                this.enabled = false;
                break;
            case AlarmState.Detecting:
                if (HorizontalDistanceFromPlayer(transform.position) > 2f)
                    LookAtTarget(player.transform.position);


                if (Vector3.Distance(player.transform.position, transform.position) > 4f)
                {
                    samuaraiAnimation = SamuraiAnimations.Run;
                } else
                {
                    if (Vector3.Distance(player.transform.position, transform.position) < 3.5f)
                        if (Time.time > attackTime + attackCooldown)
                        {
                            samuaraiAnimation = (SamuraiAnimations)Random.Range(2, 5);
                            attackTime = Time.time;
                            attackCooldown = Random.Range(3, 7);
                        }
                }

                break;
            case AlarmState.Investigating:
                samuaraiAnimation = SamuraiAnimations.Run;

                if (Time.time > investigationStartTime + investigationDuration)
                    alarmState = AlarmState.Unsuspecting;
                break;
            case AlarmState.Unsuspecting:
                samuaraiAnimation = SamuraiAnimations.Idle;
                break;
        }
        
        switch (samuaraiAnimation)
        {
            case SamuraiAnimations.Idle:
                animator.SetInteger("AnimState", 0);

                if (!IsKatanaNull())
                {
                    katana.transform.localPosition = (Vector3)katanaTransformIdle[0];
                    katana.transform.localRotation = (Quaternion)katanaTransformIdle[1];
                    katana.meleeState = Melee.MeleeState.Idle;
                }
                break;
            case SamuraiAnimations.Run:
                animator.SetInteger("AnimState", 1);
                characterController.Move(transform.forward * characterPhysics.speed * Time.deltaTime);

                if (!IsKatanaNull())
                    katana.meleeState = Melee.MeleeState.Idle;
                break;
            case SamuraiAnimations.Swing:
                animator.SetInteger("AnimState", 2);

                if (!IsKatanaNull())
                    katana.meleeState = Melee.MeleeState.Attacking;
                break;
            case SamuraiAnimations.Slash:
                animator.SetInteger("AnimState", 3);

                if (!IsKatanaNull())
                    katana.meleeState = Melee.MeleeState.Attacking;
                break;
            case SamuraiAnimations.Stab:
                animator.SetInteger("AnimState", 4);

                if (!IsKatanaNull())
                    katana.meleeState = Melee.MeleeState.Attacking;
                break;
        }

        if (samuaraiAnimation != SamuraiAnimations.Idle && !IsKatanaNull())
        {
            katana.transform.localPosition = (Vector3)katanaTransformOriginal[0];
            katana.transform.localRotation = (Quaternion)katanaTransformOriginal[1];
        }
    }

    bool IsKatanaNull() => katana == null;
}
