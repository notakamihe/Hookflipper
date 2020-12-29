using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Animation animations;
    private AnimationState[] arrayOfAnims;
    public AnimationState fireAnim;
    public AnimationState reloadAnim;
    public AudioSource fireSound;
    public new Camera camera;
    public GameObject cartridge;
    public GameObject muzzleFlash;
    public GameObject crosshair;
    public GameObject equippedBy;
    public PlayerMovement player;
    public Quaternion startRot;
    public Rigidbody rb;
    public Transform firePoint;
    public Transform flashPoint;
    public Vector3 equipPos;
    public Vector3 equipRot;
    public Vector3 equipScale;
    public Vector3 startPos;
    public float fireRate;
    public float accuracyRange = 50f;
    public float reloadTime = 2.5f;
    public float bulletSpeed = 100f;
    public float aimZoom = 15f;
    public float fireRange = 1f;
    public int magazineCapacity = 2;
    public int shotsRemaining;
    public new string name;
    public bool automatic = false;
    public bool equipped = false;
    public bool reloading = false;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        rb = GetComponent<Rigidbody>();
        fireSound = GetComponents<AudioSource>()[0];
        animations = GetComponent<Animation>();
        arrayOfAnims = FillAnimsArray();
        fireAnim = arrayOfAnims[0];
        reloadAnim = arrayOfAnims[1];
        shotsRemaining = magazineCapacity;
    }

    // Update is called once per frame
    void Update()
    {
        if (equipped)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
            
            if (shotsRemaining <= 0)
            {
                if (!reloading)
                {
                    reloading = true;
                    animations.Play(reloadAnim.name);
                    StartCoroutine(Reload(reloadTime));
                }
            }
        } else
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }
    }

    void LateUpdate ()
    {
        if (equipped && equippedBy != player.gameObject)
        {
            transform.localPosition = startPos;
            transform.localRotation = startRot;
        }
    }

    AnimationState[] FillAnimsArray ()
    {
        List<AnimationState> animStates = new List<AnimationState>();
     
        foreach (AnimationState animState in animations)
        {
            animStates.Add(animState);
        }

        return animStates.ToArray();
    }

    void FireBullet ()
    {
        if (camera != null && crosshair != null)
        {
            Bullet bullet = Instantiate(cartridge, firePoint.position, transform.parent.transform.rotation).GetComponent<Bullet>();
            Instantiate(muzzleFlash, flashPoint.position, Quaternion.identity);
            bullet.lifetime = fireRange;
            Ray ray = Camera.main.ScreenPointToRay(crosshair.transform.position);
            RaycastHit hit;

            if (Physics.Raycast(ray.origin, ray.direction, out hit, accuracyRange))
            {
                ApplyBulletVelocity(bullet, hit.point);
            } else
            {
                ApplyBulletVelocity(bullet, ray.GetPoint(accuracyRange));
            }
        } else if (equippedBy.GetComponent<Enemy>() != null)
        {
            Bullet bullet = Instantiate(cartridge, firePoint.position, transform.parent.transform.rotation).GetComponent<Bullet>();
            Instantiate(muzzleFlash, flashPoint.position, Quaternion.identity);
            bullet.lifetime = fireRange;

            Vector3 targetPos = new Vector3(
                player.transform.position.x * Random.Range(0f, 2f), 
                player.transform.position.y * Random.Range(0f, 2f), 
                player.transform.position.z
            );
             
            RaycastHit hit;

            if (Physics.Raycast(transform.position, targetPos - transform.position, out hit, accuracyRange))
            {
                print(hit);
                ApplyBulletVelocity(bullet, hit.point);
                print("Shooting at player");
            } else
            {
                ApplyBulletVelocity(bullet, transform.forward * accuracyRange);
            }
        }
    }

    void ApplyBulletVelocity (Bullet shot, Vector3 point)
    {
        Vector3 direction = (point - shot.transform.position).normalized;
        shot.rb.AddForce(direction * bulletSpeed, ForceMode.Impulse);
    }

    IEnumerator Reload (float delay)
    {
        yield return new WaitForSeconds(delay);
        shotsRemaining = magazineCapacity;
        reloading = false;
    }

    public void Shoot ()
    {
        if (equipped && equippedBy != player.gameObject)
        {
            startPos = transform.localPosition;
            startRot = transform.localRotation;
        }
        
        fireSound.Play();
        animations.Play(fireAnim.name);
        FireBullet();
        shotsRemaining--;
        print(shotsRemaining);
    } 
}
