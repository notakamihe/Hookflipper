using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bandit : MonoBehaviour
{
    public Enemy enemy;
    public GameObject hand;
    public Gun gun;
    public Vector3 gunPosConstraint;
    public Vector3 gunAimRot;
    public Vector3 gunAimPos;
    public Vector3 gunPosOriginal;
    public Quaternion gunRotOriginal;
    public float shootRadius = 15f;
    private float lastShot;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();
        gunPosOriginal = hand.transform.localPosition;
        gunRotOriginal = hand.transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (hand.transform.childCount > 0)
        {
            gun = hand.transform.GetChild(0).gameObject.GetComponent<Gun>();
            gun.equipped = true;
            gun.equippedBy = this.gameObject;
        } else
        {
            gun = null;
        }

        if (gun == null)
        {
            Destroy(GetComponent<Bandit>());
            AddComponnent<Fighter>();
        }

        float distance = Vector3.Distance(enemy.target.position, transform.position);

        if (distance < enemy.detectRadius)
        {
            Vector3 direction = enemy.target.position - transform.position;
            direction.y = 0;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = lookRotation;

            if (distance > shootRadius)
            {
                print("MOVE");
                enemy.controller.Move(transform.forward * (enemy.physics.speed/2) * Time.deltaTime); 
            }

            if (distance <= shootRadius)
            {
                if (gun != null)
                {
                    Shoot();
                }
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
                
                if (gun != null)
                {
                    hand.transform.localPosition = gunPosOriginal;
                    hand.transform.localRotation = gunRotOriginal;
                }
            }
        }
    }

    void LateUpdate ()
    {

    }

    void Shoot ()
    {
        enemy.animator.SetInteger("MotionState", 3);
        hand.transform.localPosition = gunAimPos;
        hand.transform.localRotation = Quaternion.Euler(gunAimRot);

        if (enemy.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            if (Time.time >= lastShot + gun.fireRate && gun.shotsRemaining > 0)
            {
                gun.Shoot();
                lastShot = Time.time;
            }
        }
    }
}
