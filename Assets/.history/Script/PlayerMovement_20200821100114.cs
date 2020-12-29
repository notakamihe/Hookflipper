using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private new Camera camera;
    private CharacterController controller;
    public LayerMask equipMask;
    public Gun gun;
    private ObjPhysics physics;
    private float cameraFOVOriginal;
    private float sensitivityOriginal;
    private float forwardMovement;
    private float lateralMovement;
    private float lastShot;
    public float equipRange = 2f;
    public bool isArmedGun;
    public bool reloading = false;
    public bool aiming = false;

    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponentInChildren<Camera>();
        cameraFOVOriginal = camera.fieldOfView;
        sensitivityOriginal = camera.GetComponent<MouseLook>().mouseSensitivity;
        controller = GetComponent<CharacterController>();
        physics = GetComponent<ObjPhysics>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();

        try 
        {
            if (gun != null && isArmedGun)
            {
                gun.equipped = true;

                if (Input.GetKeyDown(KeyCode.Z))
                {
                    UnequipGun();
                }

                if (Input.GetMouseButton(1) && isArmedGun)
                {
                    if (!aiming)
                    {
                        Aim();
                    }
                } else 
                {
                    aiming = false;
                    camera.fieldOfView = cameraFOVOriginal;
                    camera.GetComponent<MouseLook>().mouseSensitivity = sensitivityOriginal;
                }

                if (gun.automatic)
                {
                    if (Input.GetMouseButton(0) && isArmedGun)
                    {
                        if (Time.time > gun.fireRate + lastShot && gun.shotsRemaining > 0)
                        {
                            gun.Shoot();
                            lastShot = Time.time;
                        }
                    }
                } else
                {
                    if (Input.GetMouseButtonDown(0) && isArmedGun)
                    {
                        if (Time.time > gun.fireRate + lastShot && gun.shotsRemaining > 0)
                        {
                            gun.Shoot();
                            lastShot = Time.time;
                        }
                    }
                }
            }
        } 
        catch (NullReferenceException nullError)
        {
            print("Error during transition into (un)equipping weapon: " + nullError.Source);
        }

        CheckPickUpGuns();
    }

    void OnCollisionEnter (Collision col)
    {
        if (col.gameObject.layer != 8 && col.gameObject.layer != 9)
        {
            print("Got hit by: " + col.gameObject.ToString());
        }
    }

    void Aim ()
    {
        aiming = true;
        camera.fieldOfView -= gun.aimZoom;
        camera.GetComponent<MouseLook>().mouseSensitivity = 100;
    }

    void EquipGun (GameObject obj)
    {
        
        gun = obj.GetComponent<Gun>();
        gun.gameObject.layer = 10;
        gun.equippedBy = this.gameObject;
        gun.transform.parent = camera.transform;
        gun.transform.localPosition = gun.equipPos;
        gun.transform.localRotation = Quaternion.Euler(gun.equipRot);
        gun.transform.localScale = gun.equipScale;
        gun.camera = camera;
        gun.crosshair = camera.GetComponentInChildren<Canvas>().transform.GetChild(0).gameObject;
        print(gun.camera);

        if (gun.shotsRemaining <= 0)
        {
            reloading = false;
        }
    }

    void Jump ()
    {
        if (Input.GetButtonDown("Jump") && physics.grounded)
        {
            physics.velocity.y = Mathf.Sqrt(physics.jumpForce * -2f * physics.gravity);
        }
    }

    void Move ()
    {
        forwardMovement = Input.GetAxis("Vertical");
        lateralMovement = Input.GetAxis("Horizontal");
        Vector3 movement = transform.right * lateralMovement + transform.forward * forwardMovement;
        controller.Move(movement * physics.speed * Time.deltaTime);
    }

    void CheckPickUpGuns ()
    {
        Collider[] nearbyGuns = Array.FindAll(
            Physics.OverlapSphere(transform.position, equipRange, equipMask), 
            gun => gun.gameObject.GetComponent<Gun>() != null && transform.InverseTransformPoint(gun.transform.position).z > 0
        );

        float nearestSurroundingDist = equipRange;
        GameObject nearestSurrounding = null;

        foreach (Collider gun in nearbyGuns)
        {
            float distanceFromPlayer = Vector3.Distance(transform.position, gun.transform.position);

            if (distanceFromPlayer < nearestSurroundingDist)
            {
                nearestSurroundingDist = distanceFromPlayer;
                nearestSurrounding = gun.gameObject;
            }
        }

        if (nearestSurrounding != null)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                if (gun != null)
                {
                    UnequipGun();
                    EquipGun(nearestSurrounding);
                } else
                {
                    EquipGun(nearestSurrounding);
                }
            }
        }
    }

    void UnequipGun ()
    {
        gun.equipped = false;
        gun.equippedBy = null;
        gun.camera = null;
        gun.crosshair = null;
        gun.transform.parent = null;
        gun.gameObject.layer = 0;
        gun = null;
    }

    void OnDrawGizmos ()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, equipRange);
    }
}
