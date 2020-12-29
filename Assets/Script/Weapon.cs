using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]

public class Weapon : MonoBehaviour
{
    public DamageHandler equipper;
    public Quaternion playerEquipRotation;
    public Vector3 playerEquipPosition;
    public static Vector3 playerEquipScale = new Vector3(0.51381f, 0.66599f, 0.64422f);
    public string weaponName;
    public bool vertical = true;
    public bool equippable = true;

    private PlayerMovement plyr;
    private SphereCollider colliderSphere;
    private bool canBePickedUp = true;

    protected void Start()
    {
        colliderSphere = GetComponent<SphereCollider>() ? GetComponent<SphereCollider>() : 
            this.gameObject.AddComponent<SphereCollider>();
        colliderSphere.isTrigger = true;
        colliderSphere.radius = 1.5f;
    }

    protected void Update()
    {
        equipper = GetComponentInParent<DamageHandler>();

        if (plyr != null)
        {
            if (equipper && equipper.gameObject == plyr.gameObject)
            {
                canBePickedUp = false;
            } 
            else if (Vector3.Distance(plyr.transform.position, transform.position) > 10 && !canBePickedUp)
            {
                canBePickedUp = true;
            }
        }

        if (equipper != null && equipper.gameObject.TryGetComponent(out PlayerMovement player))
        {
            if (vertical)
                transform.localScale = player.state == PlayerMovement.State.Sliding ? 
                    new Vector3(playerEquipScale.x, playerEquipScale.y * 2, playerEquipScale.z) : playerEquipScale;
            else
                transform.localScale = player.state == PlayerMovement.State.Sliding ?
                    new Vector3(playerEquipScale.x * 2, playerEquipScale.y, playerEquipScale.z) : playerEquipScale;
        }
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovement player))
        {
            plyr = player;

            if (GamePreferences.AutoEquip && canBePickedUp && !player.GetComponentInChildren(typeof(Weapon)) &&
                !player.IsDead())
                plyr.EquipWeapon(this);
        }
    }
}

