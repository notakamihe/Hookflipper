using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class AutoEquipConsumable : MonoBehaviour
{
    private Consumable consumable;
    private PlayerMovement player;
    private bool canBeAutoEquipped = true;

    private void Start()
    {
        consumable = GetComponent<Consumable>();
        player = GameSingleton.instance.player;
    }

    private void Update()
    {
        if (consumable.equipper == player.gameObject)
            canBeAutoEquipped = false;

        if (Vector3.Distance(player.transform.position, transform.position) > player.equipRange)
            canBeAutoEquipped = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovement player))
        {
            if (!player.GetComponentInChildren(typeof(Consumable)) && consumable.equipper == null &&
                consumable.useState != Consumable.UseState.Used && canBeAutoEquipped)
                player.EquipConsumable(consumable);
        }
    }
}