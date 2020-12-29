using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistCollider : MonoBehaviour
{
    public AudioSource audioSource;
    public float damage;

    [SerializeField] private DamageHandler damageHandler;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        damage = damageHandler.attackDamageMelee;
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
