using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Animator animator;
    public CharacterController controller;
    public NavMeshAgent agent;
    public ObjPhysics physics;
    public Transform target;
    public float detectRadius = 10f;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        physics = GetComponent<ObjPhysics>();
        controller = GetComponent<CharacterController>();
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnDrawGizmos ()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }
}
