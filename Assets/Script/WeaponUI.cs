using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    public PlayerMovement player;
    
    [SerializeField] private Text nameOfWeapon;
    [SerializeField] private Text shotsRemaining;
    [SerializeField] private Text magazineCapacity;

    // Update is called once per frame
    void Update()
    {
        var weapon = player.gameObject.GetComponentInChildren(typeof(Weapon));

        if (weapon != null)
        {
            nameOfWeapon.text = ((Weapon)weapon).weaponName.ToUpper();
            
            if (weapon.gameObject.TryGetComponent(out Firearm arm))
            {
                shotsRemaining.text = arm.shotsRemaining.ToString();
                magazineCapacity.text = arm.magazineCapacity.ToString();
            } else
            {
                shotsRemaining.text = string.Empty;
                magazineCapacity.text = string.Empty;
            }
        } else
        {
            nameOfWeapon.text = "NONE";
            shotsRemaining.text = string.Empty;
            magazineCapacity.text = string.Empty;
        }
    }
}
