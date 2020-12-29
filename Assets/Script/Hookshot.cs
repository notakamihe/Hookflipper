using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hookshot : MonoBehaviour
{
    [HideInInspector] public GameObject objectToHookshot { get; private set; }
    public float hookshotStopDistance = -5f;
    public float hookSpeedMin = 20f;
    public float hookSpeedMax = 50f;
    public float hookshotRange = 100f;

    private PlayerMovement player;
    [SerializeField] private Transform hookPoint;
    private float speedOriginal;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerMovement>();
        speedOriginal = hookSpeedMin;
    }

    // Update is called once per frame
    void Update()
    {
        CheckHookshot();
    }

    void CheckHookshot ()
    {
        if (HookGetKeyDown() && player.state != PlayerMovement.State.Sliding)
        {
            if (Physics.Raycast(player.camera.transform.position, player.camera.transform.forward,
                out RaycastHit raycastHit, hookshotRange, 
                ~(1 << 12 | 1 << 4), QueryTriggerInteraction.Ignore))
            {
                if (raycastHit.collider.GetComponentInParent(typeof(Enemy)) != null)
                {
                    raycastHit.point += new Vector3(0, 5, 0);
                    hookshotStopDistance = 0f;
                }
                else
                {
                    hookshotStopDistance = -5f;
                }

                objectToHookshot = raycastHit.collider.gameObject;
                hookPoint.position = raycastHit.point;
                hookPoint.rotation = player.camera.transform.rotation;
                player.state = PlayerMovement.State.Hookshot;
            }
        }
    }

    bool HookGetKeyDown ()
    {
        return Input.GetKeyDown(Keybindings.Hookshot);
    }

    public void HookshotMove ()
    {
        if (player.JumpKeyDown())
        {
            player.state = PlayerMovement.State.Normal;
            player.physics.velocity.y = 15f;
            return;
        }

        player.stamina.drainSpeed = 2.5f;

        if (player.stamina.IsDepleted())
        {
            player.state = PlayerMovement.State.Normal;
            return;
        }

        float hookMomentumSpeed = Mathf.Clamp(Vector3.Distance(hookPoint.position, transform.position) * 2f, hookSpeedMin, hookSpeedMax);
        Vector3 hookshotDir = (hookPoint.position - transform.position).normalized;
        player.controller.Move(hookshotDir * hookMomentumSpeed * Time.deltaTime);

        if (hookPoint.InverseTransformPoint(transform.position).z >= hookshotStopDistance || player.controller.velocity.magnitude < .5f || Input.GetKeyDown(KeyCode.R))
        {
            player.state = PlayerMovement.State.Normal;
        }
    }

    public IEnumerator IncreaseHookshotSpeed(float speed, float delay)
    {
        float hookShotSpeedOriginal = hookSpeedMin;
        hookSpeedMin = speed;
        yield return new WaitForSeconds(delay);
        hookSpeedMin = hookShotSpeedOriginal;
    }
}

