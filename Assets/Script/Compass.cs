using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
    private static readonly Vector3 north = new Vector3(0, 0, 1);
    private Image compassImage;
    [SerializeField] private Sprite compassNormal;
    [SerializeField] private Sprite compassNorth;
    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
        compassImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        float direction = Vector3.SignedAngle(north, player.forward, Vector3.up);
        compassImage.sprite = Mathf.Abs(direction) <= 5 ? compassNorth : compassNormal;
        transform.rotation = Quaternion.Euler(0, 0, direction);
    }
}
