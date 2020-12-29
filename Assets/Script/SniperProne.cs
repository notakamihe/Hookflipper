using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperProne : Enemy
{
    public Firearm firearm;
    public ProneSniperAnimations proneSniperAnimation;
    public float shootDelay;

    private Vector3 shootTarget;

    public enum ProneSniperAnimations
    {
        Idle,
        Shooting,
        ShootingKneel
    }

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        firearm = GetComponentInChildren<Firearm>();
    }

    // Update is called once per frame
    new void Update()
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
                animator.SetBool("isDead", true);
                this.enabled = false;
                break;
            case AlarmState.Unsuspecting:
                proneSniperAnimation = ProneSniperAnimations.Idle;
                break;
            case AlarmState.Investigating:
                animator.SetInteger("MotionState", 1);
                CheckInvestigationComplete();
                break;
            case AlarmState.Detecting:
                LookAtTarget(player.transform.position);

                if (Vector3.Distance(player.transform.position, transform.position) <= 7f)
                {
                    proneSniperAnimation = ProneSniperAnimations.ShootingKneel;
                } else
                {
                    proneSniperAnimation = ProneSniperAnimations.Shooting;
                }

                break;
        }

        switch (proneSniperAnimation)
        {
            case ProneSniperAnimations.Idle:
                animator.SetInteger("MotionState", 0);
                notLying = false;
                break;
            case ProneSniperAnimations.Shooting:
                animator.SetInteger("MotionState", 1);
                notLying = false;

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("ProneSnipeAim") &&
                    animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= .95)
                {
                    shootTarget = player.transform.position;
                    StartCoroutine(Shoot(shootTarget, shootDelay));
                }
                break;
            case ProneSniperAnimations.ShootingKneel:
                animator.SetInteger("MotionState", 2);
                notLying = true;
                shootTarget = player.transform.position;
                StartCoroutine(Shoot(shootTarget, shootDelay));
                break;
        }
    }

    IEnumerator Shoot(Vector3 at, float delay)
    {
        yield return new WaitForSeconds(delay);
        firearm.Shoot(at);
    }
}
