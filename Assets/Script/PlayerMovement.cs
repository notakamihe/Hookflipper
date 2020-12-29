using System.Collections;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerMovement : MonoBehaviour
{
    public AudioSource footstep; 
    public AudioSource slideSound;
    public AudioSource neckSnap;
    public AudioSource sip;
    public AudioSource hurt;
    
    [HideInInspector] public AimState aimState;
    [HideInInspector] public Hookshot hookshot;
    [HideInInspector] public Stamina stamina;

    [Space(25)]

    public new Camera camera;
    public CharacterController controller;
    public Health health;
    public LayerMask equipMask;
    public CharacterPhysics physics;
    public GameObject speedWind;
    public State state;
    public Transform cursor;
    public Vector3 lookingAt;
    public float newFov;
    public float equipRange = 10f;
    public float speedScale { get; private set; }
    public float hookshotSpeed = 5f;
    public float hookshotSpeedMultiplier = 2f;
    public float cameraFOVNormal;
    public float slideHeightScale = 0.5f;
    public float slideSpeed = 100f;

    private new Animation animation;
    private DamageHandler damageHandler;
    private LevelManager gameController;
    [SerializeField] private LayerMask stuckMask;
    private MouseLook mouse;
    [SerializeField] private Transform stuckCheck;
    [SerializeField] private Transform deathParent;
    [SerializeField] private Transform hatPoint;


    private bool somethingRightAbove;
    private bool isDoneAddForce = true;
    private float speedOriginal;

    public LayerMask enemMask;

    public enum State
    {
        Normal,
        HookshotThrow,
        Hookshot,
        Sliding,
        Dead
    }

    public enum AimState
    {
        Normal,
        Aiming
    }

    // Start is called before the first frame update
    private void Awake()
    {
        gameController = FindObjectOfType<LevelManager>();
        camera = GetComponentInChildren<Camera>();
        mouse = camera.gameObject.GetComponent<MouseLook>();
        controller = GetComponent<CharacterController>();
        physics = GetComponent<CharacterPhysics>();
        health = GetComponent<Health>();
        damageHandler = GetComponent<DamageHandler>();
        animation = GetComponent<Animation>();
        hookshot = GetComponent<Hookshot>();
        stamina = GetComponent<Stamina>();
        state = State.Normal;

        speedOriginal = physics.speed;
    }

    // Update is called once per frame
    void Update()
    {
        somethingRightAbove = Physics.Raycast(transform.position, Vector3.up, out RaycastHit hit, 5f);

        if (health.Depleted() && state != State.Dead)
        {
            state = State.Dead;
        }
        else
        {
            switch (state)
            {
                default:
                case State.Normal:
                    speedWind.SetActive(false);
                    physics.gravity = -9.81f;
                    break;
                case State.Hookshot:
                    hookshot.HookshotMove();
                    physics.gravity = 0f;
                    cursor.localScale = new Vector3(.7f, .7f, .7f);

                    if (!Input.GetKey(Keybindings.Jump))
                        physics.velocity.y = -2f;

                    if (aimState != AimState.Aiming)
                        camera.fieldOfView = Mathf.Clamp(camera.fieldOfView - 200 * Time.deltaTime,
                            GamePreferences.PlayerFOV * (2f / 3f), 90);

                    speedWind.SetActive(true);
                    break;
                case State.Sliding:
                    CheckSomethingRightAbove();
                    break;
                case State.Dead:
                    camera.gameObject.GetComponent<MouseLook>().enabled = false;
                    deathParent.parent = transform.parent;
                    transform.parent = deathParent;
                    animation.Play();
                    this.enabled = false;
                    Cursor.lockState = CursorLockMode.None;

                    if (gameController != null)
                    {
                        gameController.gameOver = true;
                    }

                    Weapon equippedWeapon = GetComponentInChildren<Weapon>();
                    Consumable equippedConsumable = GetComponentInChildren<Consumable>();

                    if (equippedWeapon)
                    {
                        DropWeapon(equippedWeapon);
                    }

                    if (equippedConsumable)
                    {
                        DropConsumable(equippedConsumable);
                    }



                    break;
            }

            switch (aimState)
            {
                case AimState.Normal:
                    if (state != State.Hookshot)
                    {
                        camera.fieldOfView = Mathf.Clamp(camera.fieldOfView + 600 * Time.deltaTime, 30,
                            GamePreferences.PlayerFOV);
                        cursor.localScale = new Vector3(1f, 1f, 1f);
                    }

                    mouse.mouseSensitivity = GamePreferences.PlayerLookSensitivity;
                    break;
                case AimState.Aiming:
                    camera.fieldOfView = Mathf.Clamp(camera.fieldOfView - 600 * Time.deltaTime,
                        newFov, 90);
                    cursor.localScale = new Vector3(.7f, .7f, .7f);
                    mouse.mouseSensitivity = GamePreferences.PlayerAimSensitivity;
                    break;
            }

            if ((Input.GetKey(Keybindings.Sprint) || state == State.Hookshot) && !stamina.IsDepleted())
            {
                stamina.DecreaseStamina(stamina.drainSpeed);
            } else
            {
                stamina.IncreaseStamina(stamina.rechargeSpeed);
            }

            Weapon nearestWeapon = Physics.OverlapSphere(transform.position, equipRange).Select(
                x => (Weapon)x.GetComponent(typeof(Weapon))).Where(w => w != null &&
                !w.TryGetComponent(out Fist fist) && w.equipper != damageHandler && w.equippable).OrderBy(
                p => Vector3.Distance(transform.position, p.transform.position)).FirstOrDefault();

            Consumable nearestConsumable = Physics.OverlapSphere(transform.position, equipRange).Select(
                x => (Consumable)x.GetComponent(typeof(Consumable))).Where(c => c != null &&
                c.useState != Consumable.UseState.Used && c.equipper != this.gameObject).OrderBy(
                y => Vector3.Distance(transform.position, y.transform.position)).FirstOrDefault();


            if (Input.GetKeyDown(Keybindings.Equip))
            {
                if (nearestWeapon != null)
                    EquipWeapon(nearestWeapon);
                if (nearestConsumable)
                    EquipConsumable(nearestConsumable);
            }

            Move();

            if (Input.GetKeyDown(Keybindings.Slide) && physics.grounded && state != State.Sliding)
            {
                Slide(slideSpeed);
                state = State.Sliding;
                slideSound.Play();
            }

            if (JumpKeyDown() && physics.grounded)
            {
                Jump();
            }

            if (Input.GetKeyDown(Keybindings.Equip))
            {
                var nearestWearable = Physics.OverlapSphere(transform.position, 5, ~13).Where(
                    s => s.GetComponent<Wearable>() != null).FirstOrDefault();

                Wearable wearable = nearestWearable == null ? null : nearestWearable.GetComponent<Wearable>();

                if (wearable != null && !wearable.worn)
                {
                    print(wearable.gameObject);
                    switch (wearable.itemType)
                    {
                        case Wearable.WearItem.Hat:
                            print("WORN!");
                            wearable.transform.rotation = Quaternion.Euler(0, 0, 0);
                            wearable.GetComponent<Rigidbody>().isKinematic = true;
                            wearable.transform.parent = transform;
                            wearable.transform.localPosition = hatPoint.localPosition;

                            if (wearable.wearSound)
                                wearable.wearSound.Play();
                            break;
                    }
                }
            }

            physics.speed = GetComponentInChildren<Katana>() != null ? speedOriginal * 3 : speedOriginal;

            HandleStamina();
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.TryGetComponent(out Bullet bullet))
        {
            if (!health.Depleted())
            {
                hurt.Play();
                float damageTaken = bullet.firearm.equipper.attackDamageRanged;
                float defenseModifier = GetComponent<DamageHandler>().defenseModifier;
                health.TakeDamage((bullet.damage + damageTaken) * defenseModifier);
            }
        }

        if (col.gameObject.TryGetComponent(out Melee melee) && melee.meleeState == Melee.MeleeState.Attacking)
        {
            hurt.Play();
            health.TakeDamage((melee.damage + melee.equipper.attackDamageMelee) *
                GetComponent<DamageHandler>().defenseModifier);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (Physics.SphereCast(physics.groundCheck.position, 0.15f, Vector3.down, out RaycastHit raycastHit, 1.5f))
        {
            Enemy enemy = (Enemy)raycastHit.collider.GetComponentInParent(typeof(Enemy));

            if (enemy && !enemy.IsDead())
            {
                if (!enemy.GetComponentInParent<Samurai>())
                {
                    if (!neckSnap.isPlaying)
                        neckSnap.Play();

                    enemy.health.Kill();
                }

                Jump(3f);
            }
        }

        if (hit.collider.gameObject.layer == 4)
        {
            controller.Move(Vector3.left * 1 * Time.deltaTime);
        }
    }

    public IEnumerator AddForce(float movement, Vector3 direction, int force = 20, float interval = .01f)
    {
        isDoneAddForce = false;

        for (int i = 0; i < force; i++)
        {
            yield return new WaitForSeconds(interval);
            controller.Move(direction * movement * Time.deltaTime);
            controller.Move(Vector3.down * 100f * Time.deltaTime);
        }

        isDoneAddForce = true;
    }

    public IEnumerator BumpUpDefense (float amount, float delay)
    {
        float originalDefenseModifier = damageHandler.defenseModifier;
        damageHandler.defenseModifier *= amount;
        yield return new WaitForSeconds(delay);
        damageHandler.defenseModifier = originalDefenseModifier;
    }

    public float CalculateDamage(float dmg, float dmgrAttack, float defenseMod, float crt = 0)
    {
        crt = Random.Range(0f, 1f) > 1 - (crt / 100) ? Random.Range(2, 5) : 1;
        return (dmg + dmgrAttack) * defenseMod * crt;
    }

    void CheckSomethingRightAbove ()
    {
        if (isDoneAddForce)
        {
            if (somethingRightAbove)
            {
                StartCoroutine(AddForce(slideSpeed, transform.forward));
            }
            else
            {
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y / slideHeightScale, transform.localScale.z);
                state = State.Normal;
            }
        }
    }

    public void DropConsumable (Consumable consumable)
    {
        consumable.equipper = null;
        consumable.transform.parent = null;
    }

    public void DropWeapon (Weapon weapon)
    {
        weapon.transform.parent = null;
    }

    public void EquipConsumable(Consumable consumable)
    {
        Consumable equippedConsumable = (Consumable)GetComponentInChildren(typeof(Consumable));

        if (equippedConsumable)
            DropConsumable(equippedConsumable);

        consumable.equipper = this.gameObject;
        consumable.transform.parent = camera.transform;
    }

    public void EquipWeapon (Weapon weapon)
    {
        var equippedWeapon = GetComponentInChildren(typeof(Weapon));

        if (equippedWeapon != null)
            DropWeapon((Weapon)equippedWeapon);

        weapon.transform.parent = camera.transform;
        weapon.transform.localPosition = weapon.playerEquipPosition;
        weapon.transform.localRotation = weapon.playerEquipRotation;
        weapon.transform.localScale = Weapon.playerEquipScale;
    }

    void HandleStamina ()
    {
        if (Input.GetKey(Keybindings.Sprint) && !stamina.IsDepleted())
        {
            speedScale = 2f;
            stamina.drainSpeed = 10f;
        }
        else
        {
            speedScale = 1f;
        }
    }

    public bool IsDead() => state == State.Dead;

    public void Jump (float jumpScale = 1f)
    {
        physics.velocity.y = Mathf.Sqrt(physics.jumpForce * jumpScale * -2f * physics.gravity);
    }

    public bool JumpKeyDown ()
    {
        return Input.GetKeyDown(Keybindings.Jump);
    }

    void Move ()
    {
        float forwardMovement = Input.GetAxis("Vertical");
        float lateralMovement = Input.GetAxis("Horizontal");
        Vector3 movement = transform.right * lateralMovement + transform.forward * forwardMovement * speedScale;
        controller.Move(movement * physics.speed * Time.deltaTime);

        if (forwardMovement == 1 && physics.grounded && controller.velocity.magnitude > 3)
        {
            if (!footstep.isPlaying && state != State.Sliding)
            {
                footstep.pitch = (physics.speed / speedOriginal) * speedScale;
                footstep.Play();
            }
        }
    }

    void Slide (float slideForce)
    {
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * slideHeightScale, transform.localScale.z);
        StartCoroutine(AddForce(slideForce, transform.forward));
    }
}
