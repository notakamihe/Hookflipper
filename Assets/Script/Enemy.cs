using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(DamageHandler))]

public class Enemy : MonoBehaviour
{
    public Health health;
    public float aimSkill;

    protected AlarmState alarmState;
    protected Animator animator;
    protected CharacterController characterController;
    protected CharacterPhysics characterPhysics;
    protected PlayerMovement player;
    [SerializeField] protected Transform chest;
    protected float investigationStartTime;
    [SerializeField] protected float investigationDuration;
    [SerializeField] protected bool notLying = true;
    protected bool updateChest = true;

    private LineSight lineSight;

    public enum AlarmState
    {
        Unsuspecting,
        Investigating,
        Detecting,
        Dead
    }

    public void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        health = GetComponent<Health>();
        lineSight = GetComponent<LineSight>();
        characterPhysics = GetComponent<CharacterPhysics>();
        player = GameSingleton.instance.player;
        GameSingleton.instance.allEnemies.Add(this);
    }

    public void Update()
    {
        if (health.Depleted())
            alarmState = AlarmState.Dead;

        if (!IsDead())
        {
            if (lineSight.targetDetected && !PlayerDead())
            {
                alarmState = AlarmState.Detecting;
            } else if (alarmState != AlarmState.Investigating)
            {
                alarmState = AlarmState.Unsuspecting;
            }
        } else
        {
            updateChest = false;

            if (GameSingleton.instance.allEnemies.Contains(this))
                GameSingleton.instance.allEnemies.Remove(this);
       
            Destroy(this.gameObject, 10f);
        }
    }

    private void LateUpdate()
    {
        if (updateChest && notLying && alarmState == AlarmState.Detecting)
            chest.LookAt(player.transform);
    }

    protected void CheckInvestigationComplete ()
    {
        if (Time.time > investigationStartTime + investigationDuration)
            alarmState = AlarmState.Unsuspecting;
    }

    protected float DistanceFromPlayer ()
    {
        return Vector3.Distance(player.transform.position, transform.position);
    }

    protected float HorizontalDistanceFromPlayer(Vector3 pos) =>
        Mathf.Sqrt(Mathf.Pow(player.transform.position.x - pos.x, 2) + 
            Mathf.Pow(player.transform.position.z - pos.z, 2));

    protected void LookAtTarget (Vector3 target)
    {
        Vector3 direction = new Vector3((target - transform.position).x, 0, (target - transform.position).z);
        transform.rotation = Quaternion.LookRotation(direction);
    }

    public void InitializeFields (Transform chest, float investigationDuration)
    {
        this.chest = chest;
        this.investigationDuration = investigationDuration;
    }

    public void Investigate(Vector3 source)
    {
        if (alarmState == AlarmState.Unsuspecting && !PlayerDead())
        {
            LookAtTarget(source);
            alarmState = AlarmState.Investigating;
            investigationStartTime = Time.time;
        }
    }


    public bool IsDead() => alarmState == AlarmState.Dead;

    public void OnHurt (float damage, Vector3 source)
    {
        if (!IsDead())
        {
            health.TakeDamage(damage);

            if (!IsDead() && alarmState == AlarmState.Unsuspecting)
                Investigate(source);
        }
    }

    public void OnShot(Bullet bullet, HitBodyDetector.PartOfBody partOfBody)
    {
        if (!IsDead())
        {
            DamageHandler shooter = bullet.firearm.equipper;

            float damageTaken = shooter.attackDamageRanged;
            float defenseModifier = GetComponent<DamageHandler>().defenseModifier;
            float criticalMultiplier = Random.Range(0f, 1f) > 1 - shooter.criticalChance / 100 ? Random.Range(2, 5) : 1;
            float damageTakenCalculated = (bullet.damage + damageTaken) * defenseModifier * criticalMultiplier;

            if (partOfBody == HitBodyDetector.PartOfBody.Head)
                health.TakeDamage(damageTakenCalculated * 5);
            else if (partOfBody == HitBodyDetector.PartOfBody.Torso)
                health.TakeDamage(damageTakenCalculated);
            else if (partOfBody == HitBodyDetector.PartOfBody.Arms)
                health.TakeDamage(damageTakenCalculated * .5f);
            else if (partOfBody == HitBodyDetector.PartOfBody.Legs)
                health.TakeDamage(damageTakenCalculated * .333f);

            if (!IsDead() && alarmState == AlarmState.Unsuspecting)
            {
                Investigate(bullet.firearm.equipper.transform.position);
            }
        }
    }

    private bool PlayerDead() => player.state == PlayerMovement.State.Dead;
}
