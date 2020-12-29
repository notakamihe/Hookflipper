using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    public PlayerMovement player;
    private GameObject ammoInfo;
    private Text gunName;
    private Text shotsRemaining;
    private Text magazineCapacity;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().GetComponent<PlayerMovement>();
        gunName = transform.GetChild(0).gameObject.GetComponent<Text>();
        ammoInfo = transform.GetChild(1).gameObject;
        shotsRemaining = ammoInfo.transform.GetChild(1).gameObject.GetComponent<Text>();
        magazineCapacity = ammoInfo.transform.GetChild(2).gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.gun != null)
        {
            gunName.text = player.gun.name.ToUpper();
            ammoInfo.SetActive(true);
            shotsRemaining.text = player.gun.shotsRemaining.ToString();
            magazineCapacity.text = player.gun.shotsPerRound.ToString();
        } else
        {
            gunName.text = "NONE";
            ammoInfo.SetActive(false);
        }
    }
}
