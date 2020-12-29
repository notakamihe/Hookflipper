using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class CharacterPhysics : MonoBehaviour
{
    public AudioSource slopeSlideSound;

    [Space(25)]

    public Transform groundCheck;
    public LayerMask groundMask;
    public Vector3 velocity;
    public float speed = 6f;
    public float turnSpeed = 60f;
    public float jumpForce = 3f;
    public float gravity = -9.81f;
    public float detectGroundDist = .4f;
    public bool grounded;
    public bool isSlopeTooSteep;

    private CharacterController controller;
    private Vector3 hitNormal;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        PullDown();
        grounded = Physics.CheckSphere(groundCheck.position, detectGroundDist, groundMask);

        if (grounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (!grounded)
            isSlopeTooSteep = false;
        
        if (isSlopeTooSteep)
        {
            controller.Move(new Vector3(hitNormal.x, 0, hitNormal.z) * 6f * Time.deltaTime);

            if (controller.GetComponent<PlayerMovement>())
            {
                if (slopeSlideSound.isActiveAndEnabled && !slopeSlideSound.isPlaying)
                    slopeSlideSound.Play();
            }
        } else
        {
            if (slopeSlideSound.isPlaying)
                slopeSlideSound.Stop();
        }
    }

    public void PullDown ()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        hitNormal = hit.normal;

        if (Physics.SphereCast(groundCheck.position, 0.2f, Vector3.down, out RaycastHit slopeHit, 1f))
        {
            isSlopeTooSteep = grounded && Vector3.Angle(Vector3.up, slopeHit.normal) > 40;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(groundCheck.position, detectGroundDist);
    }
}
