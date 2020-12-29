using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Transform player;
    public float mouseSensitivity;
    public float lookLimitUp = -180f;
    public float lookLimitDown = 180f;
    public bool canLookAround = true;

    private float mouseX;
    private float mouseY;
    private float xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        mouseSensitivity = GamePreferences.PlayerLookSensitivity;
    }

    // Update is called once per frame
    void Update()
    {
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, lookLimitUp, lookLimitDown);

        if (mouseX != 0)
            transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        player.Rotate(Vector3.up * mouseX);
    }
}
