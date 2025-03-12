using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Playables;

public class PlayerController : MonoBehaviour
{
    public Vector2 MovementInput { get; private set; }

    #region components
    private PlayerInput playerInput;
    public Animator animator;
    public Rigidbody rb;

    private AudioSource audioSource;
    public AudioClip swingClip;

    public GameObject currentWeapon;
    public GameObject weaponHolder;
    #endregion

    #region states
    public PlayerState CurrentState { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerRunState RunState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerPrimaryAttackState PrimaryAttackState { get; private set; }
    public BasicSkillState BasicSkillState { get; private set; }
    public UltimateSkillState UltimateSkillState { get; private set; }
    #endregion

    #region public variables
    public bool isAttacking = false;
    public bool isUsingSkill = false;
    public bool isDamaging = false;
    public int weaponIdx = 0;
    #endregion

    /*#region slash effects
    public GameObject slashEffect1;
    public GameObject slashEffect2;
    public GameObject slashEffect3;
    #endregion*/

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        playerInput = new PlayerInput();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        IdleState = new PlayerIdleState(this, "Idle");
        RunState = new PlayerRunState(this, "isRunning");
        DashState = new PlayerDashState(this, "isDashing");
        PrimaryAttackState = new PlayerPrimaryAttackState(this, "Attack1");
        BasicSkillState = new BasicSkillState(this, "BasicSkill");
        UltimateSkillState = new UltimateSkillState(this, "UltimateSkill");
    }

    private void Start()
    {
        CurrentState = IdleState;
        CurrentState.EnterState();
    }

    private void FixedUpdate()
    {
        CurrentState.UpdateState();
        if (Input.GetKeyDown(KeyCode.K))
        {
            SkillManager.instance.basicSkill.currentBasic++;
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            SkillManager.instance.ultimateSkill.currentUltimate++;
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            SkillManager.instance.dash.currentDash++;
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            PlayerManager.instance.stat.CurrentHealth += 40;
        }
    }

    public void TransitionToState(PlayerState newState)
    {
        CurrentState.ExitState();
        CurrentState = newState;
        CurrentState.EnterState();
    }

    public void PlayerDeath()
    {
        animator.SetTrigger("Dead");
        playerInput.Disable();  // Disable the input system
        rb.velocity = Vector3.zero;
    }

    public void EnableDamage()
    {
        PlaySoundEffect(swingClip, 0.6f);
        isDamaging = true;
        rb.velocity = Vector3.zero;
    }

    public void DisableDamage()
    {
        isDamaging = false;
        Weapon weaponScript = currentWeapon.GetComponent<Weapon>();
        weaponScript.hitEnemies.Clear();
    }

    public void OnAttackEnd()
    {
        animator.SetBool("Attack1", false);
        StartCoroutine(Wait());
        
        /*Weapon weaponScript = currentWeapon.GetComponent<Weapon>();
        weaponScript.trail.SetActive(false);*/
    }

    public void PlaySoundEffect(AudioClip clip, float _volume)
    {
        if (audioSource != null && clip != null)
        {
            // Set random pitch between 0.9 and 1.1
            audioSource.volume = _volume;
            audioSource.pitch = Random.Range(0.85f, 1.15f);
            audioSource.PlayOneShot(clip);
        }
    }

    public void WeaponRefresh(int weaponIndex)
    {
        if(currentWeapon != null)
        {
            currentWeapon.SetActive(false);
        }
        currentWeapon = weaponHolder.transform.GetChild(weaponIndex).gameObject;
        currentWeapon.SetActive(true);
        weaponIdx = weaponIndex;
    }

    private void OnEnable()
    {
        playerInput.Player.Movement.performed += OnMovementPerformed;
        playerInput.Player.Movement.canceled += OnMovementCanceled;
        playerInput.Player.Dash.performed += OnDashPerformed;
        playerInput.Player.Attack.performed += OnAttackPerformed;
        playerInput.Player.BasicSkill.performed += OnBasicSkillPerformed;
        playerInput.Player.UltimateSkill.performed += OnUltimateSkillPerformed;
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Player.Movement.performed -= OnMovementPerformed;
        playerInput.Player.Movement.canceled -= OnMovementCanceled;
        playerInput.Player.Dash.performed -= OnDashPerformed;
        playerInput.Player.Attack.performed -= OnAttackPerformed;
        playerInput.Player.BasicSkill.performed -= OnBasicSkillPerformed;
        playerInput.Player.UltimateSkill.performed -= OnUltimateSkillPerformed;
        playerInput.Disable();
    }

    private void OnMovementPerformed(InputAction.CallbackContext context)
    {
        MovementInput = context.ReadValue<Vector2>();
    }

    private void OnMovementCanceled(InputAction.CallbackContext context)
    {
        MovementInput = Vector2.zero;
    }

    private void OnDashPerformed(InputAction.CallbackContext context)
    {
        if (SkillManager.instance.dash.CanUseSkill() && !isUsingSkill && !isAttacking)
        {
            isUsingSkill = true;
            TransitionToState(DashState);
        }
    }

    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        if (!isAttacking && !isUsingSkill)
        {
            isAttacking = true;
            TransitionToState(PrimaryAttackState);
        }
    }

    private void OnBasicSkillPerformed(InputAction.CallbackContext context)
    {
        if (SkillManager.instance.basicSkill.CanUseSkill() && !isUsingSkill && !isAttacking)
        {
            TransitionToState(BasicSkillState);
            isUsingSkill = true;
        }
    }

    private void OnUltimateSkillPerformed(InputAction.CallbackContext context)
    {
        if (SkillManager.instance.ultimateSkill.CanUseSkill() && !isUsingSkill && !isAttacking)
        {
            TransitionToState(UltimateSkillState);
            isUsingSkill = true;
        }
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.11f);
        isAttacking = false;
        TransitionToState(IdleState);
    }
}



/*public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;
    private Vector2 movementInput;
    private Animator animator;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 0.15f;

    private void Awake()
    {
        playerInput = new PlayerInput();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (movementInput != Vector2.zero)
        {
            Vector3 movement = new Vector3(movementInput.x, 0, movementInput.y);
            movement.Normalize();

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), rotationSpeed);

            transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);

        }
    }

    private void OnEnable()
    {
        playerInput.Player.Movement.performed += OnMovementPerformed;
        playerInput.Player.Movement.canceled += OnMovementCanceled;
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Player.Movement.performed -= OnMovementPerformed;
        playerInput.Player.Movement.canceled -= OnMovementCanceled;
        playerInput.Disable();
    }

    private void OnMovementPerformed(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
        animator.SetBool("isRunning", true);
    }

    private void OnMovementCanceled(InputAction.CallbackContext context)
    {
        movementInput = Vector2.zero;
        animator.SetBool("isRunning", false);
    }
}*/
