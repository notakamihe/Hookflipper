using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Firearm : Weapon
{
    public FireMode mode;
    public GameObject bulletType;
    public float fireRange;
    public float fireRate;
    public float bulletSpeed;
    public float reloadDuration;
    public float zoomFOV;
    public int magazineCapacity;
    [HideInInspector] public int shotsRemaining;
    public string shootAnim;
    public string reloadAnim;
    public string fireModeName;

    Vector3 lookingAt;
    private new Animation animation;
    private Enemy targetEnemy;
    [SerializeField] private GameObject muzzleFlash;
    private Rigidbody rb;
    [SerializeField] private Transform fireTransformNormal;
    [SerializeField] private Transform flashPosition;
    private float accuracyRange;
    private float lastShot;
    private bool isReloading = false;

    [Space(25)]

    public AudioSource shotSound;
    public AudioSource reloadSound;

    public enum FireMode
    {
        SemiSingle,
        Automatic
    }

    private new void Start()
    {
        base.Start();

        animation = GetComponent<Animation>();
        shotSound = GetComponent<AudioSource>();
        shotsRemaining = magazineCapacity;
        rb = GetComponent<Rigidbody>();
    }

    private new void Update()
    {
        base.Update();

        accuracyRange = fireRange * 20;

        if (equipper != null)
            rb.isKinematic = true;
        else
            rb.isKinematic = false;

        if (equipper != null)
        {
            if (equipper.gameObject.TryGetComponent(out PlayerMovement player) && !player.IsDead())
            {
                WeaponMechanics(player);
            }

            if (equipper.GetComponent(typeof(Enemy)))
            {
                foreach (MeshCollider meshCollider in GetComponentsInChildren<MeshCollider>().Skip(1))
                {
                    meshCollider.isTrigger = true;
                }
            } else
            {
                foreach (MeshCollider meshCollider in GetComponentsInChildren<MeshCollider>().Skip(1))
                {
                    meshCollider.isTrigger = false;
                }
            }
        }
    }

    public void Shoot (Vector3 at)
    {
        if (shotsRemaining > 0 && Time.time > lastShot + fireRate)
        {
            lastShot = Time.time;
            shotSound.Play();
            animation.Play(shootAnim);

            Enemy[] enemiesHearing = Physics.OverlapSphere(transform.position, shotSound.maxDistance).Where(
                c => c.GetComponent(typeof(Enemy)) != null).Select(x => (Enemy)x.GetComponent(typeof(Enemy))).Where(
                e => equipper != e.GetComponent<DamageHandler>()).ToArray();

            Array.ForEach(enemiesHearing, e => e.Investigate(transform.position));

            Bullet bullet = Instantiate(bulletType, fireTransformNormal.position, fireTransformNormal.rotation).GetComponent<Bullet>();
            Instantiate(muzzleFlash, flashPosition.position, Quaternion.identity);
            bullet.lifetime = fireRange;
            bullet.firearm = this;

            bullet.rb.AddForce((at - bullet.transform.position).normalized * Mathf.Pow(bulletSpeed, 2) * Time.deltaTime);
            shotsRemaining--;

            if (shotsRemaining <= 0)
                if (!equipper.TryGetComponent(out PlayerMovement player) || GamePreferences.AutoReload)
                StartCoroutine(Reload(reloadDuration));
        }
    }

    IEnumerator Reload (float duration)
    {
        animation.Play(reloadAnim);
        isReloading = true;

        if (reloadSound)
            reloadSound.Play();

        yield return new WaitForSeconds(duration);
        shotsRemaining = magazineCapacity;
        isReloading = false;
    }

    private void WeaponMechanics(PlayerMovement player)
    {
        player.newFov = GamePreferences.PlayerFOV * zoomFOV;
        player.aimState = Input.GetMouseButton(1) ? PlayerMovement.AimState.Aiming : PlayerMovement.AimState.Normal;

        MouseLook camera = player.camera.GetComponent<MouseLook>();
        Ray ray = player.camera.ScreenPointToRay(player.cursor.position);
        RaycastHit hit;

        if (GamePreferences.AimAssist)
        {
            foreach (Enemy enemy in GameSingleton.instance.allEnemies)
            {
                if (enemy)
                {
                    if (Vector3.Distance(player.transform.position, enemy.transform.position) <= accuracyRange)
                    {
                        if (Vector3.Angle(camera.transform.forward, enemy.transform.position - player.transform.position) <= 25)
                        {
                            Vector3 enemyCenter = enemy.transform.position + enemy.transform.up * 3.15f;
                            Vector3 aimAssistPoint = ray.GetPoint(Vector3.Distance(enemyCenter, camera.transform.position));

                            if (Vector3.Distance(aimAssistPoint, enemyCenter) <= 1.5f)
                            {
                                if (targetEnemy == null)
                                {
                                    StartCoroutine(ToggleMouseLook(1f, camera, enemy));
                                    camera.transform.LookAt(enemyCenter);
                                }
                            }
                        }
                        else
                        {
                            if (enemy == targetEnemy || (targetEnemy != null && targetEnemy.IsDead()))
                            {
                                targetEnemy = null;
                            }
                        }
                    }
                }
            }
        }

        if (Physics.Raycast(ray, out hit, accuracyRange, ~LayerMask.GetMask("Boundary"),
                  QueryTriggerInteraction.Ignore))
        {
            lookingAt = hit.point;
        }
        else
        {
            lookingAt = ray.GetPoint(accuracyRange);
        }
        

        switch (mode)
        {
            case FireMode.SemiSingle:
                if (Input.GetButtonDown(GameSingleton.instance.leftClickInputName))
                {
                    Shoot(lookingAt);
                }

                break;
            case FireMode.Automatic:
                if (Input.GetButton(GameSingleton.instance.leftClickInputName))
                {
                    Shoot(lookingAt);
                }

                break;
        }

        if (Input.GetKeyDown(Keybindings.Drop))
        {
            player.aimState = PlayerMovement.AimState.Normal;
            player.DropWeapon(this);
        }

        if (Input.GetKeyDown(Keybindings.Reload) && !isReloading && shotsRemaining < magazineCapacity)
        {
            shotsRemaining = 0;
            StartCoroutine(Reload(reloadDuration));
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(lookingAt, 1f);
    }

    IEnumerator ToggleMouseLook (float delay, MouseLook mouseLook, Enemy enemy)
    {
        mouseLook.enabled = false;
        yield return new WaitForSeconds(delay);
        mouseLook.enabled = true;
        targetEnemy = enemy;
    }
}