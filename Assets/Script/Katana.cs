using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;


public class Katana : Melee
{
    public float attackRate;
    public float attackRange;
    public float aimAssist;

    private new Animation animation;
    private Rigidbody rb;
    private string[] animations;
    private float lastAttack;

    [Space(25)]

    public AudioSource sliceSound;
    public AudioSource hitSound;

    private new void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
        animation = GetComponent<Animation>();
        animations = animation.Cast<AnimationState>().Select(x => x.name).ToArray();
    }

    private new void Update()
    {
        base.Update();

        if (equipper != null)
        {
            rb.isKinematic = true;

            if (equipper.TryGetComponent(out PlayerMovement player))
            {
                KatanaMechanics(player);
            }
        }
        else
            rb.isKinematic = false;
    }

    void KatanaMechanics(PlayerMovement player)
    {
        Ray ray = player.camera.ScreenPointToRay(player.cursor.position);
        
        if (Input.GetButton(GameSingleton.instance.leftClickInputName) && Time.time > lastAttack + attackRate)
        {
            bool hasHitAnEnemy = false;
            lastAttack = Time.time;
            animation.Play(animations[Random.Range(0, animations.Length)]);

            if (Physics.SphereCast(ray, 0.5f, out RaycastHit rhit, attackRange, ~LayerMask.GetMask("Boundary")))
            {
                Enemy enemy = (Enemy)rhit.collider.GetComponentInParent(typeof(Enemy));

                if (enemy)
                {
                    enemy.OnHurt(player.CalculateDamage(damage, equipper.attackDamageMelee,
                        enemy.GetComponent<DamageHandler>().defenseModifier, equipper.criticalChance),
                        transform.position);
                    hasHitAnEnemy = true;
                }
            }

            if (hasHitAnEnemy)
                hitSound.Play();
            else
                sliceSound.Play();
        }

        if (Input.GetKeyDown(Keybindings.Drop))
        {
            player.DropWeapon(this);
        }
    }
}