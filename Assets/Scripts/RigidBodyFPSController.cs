using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class RigidBodyFPSController : MonoBehaviour
{
    PlayerInputActions controls;
    PlayerInputActions.PlayerActions playerMovement;

    public PlayerStatsSO playerStatsSO;
    public float playerHealth;
    public float playerArmor;
    public float playerStamina;

    //WeaponShoot wpShoot;

    //Move to GameManager after Demo
    //public RigidBodyFPSController instance = null;

    Coroutine fireCoroutine;

    private void Awake()
    {
        controls = new PlayerInputActions();
        playerMovement = controls.Player;


        //playerMovement.Fire.started += context => 
        //playerMovement.Fire.canceled += context => StopFiring();

    }

    private void OnEnable()
    {
        //controls.Enable();
    }

    void StartFiring()
    {
        fireCoroutine = StartCoroutine(_weaponManager.weaponShoot.Firing());
    }

    void StopFiring()
    {
        if (fireCoroutine != null)
        {
            StopCoroutine(fireCoroutine);
        }
    }

    public WeaponManager _weaponManager = null;    

    public Camera cam;
    public Camera playerBodyCam;

    public IState currentPlayerState;    

    public TempCamLook camLook = new TempCamLook();

    private Rigidbody rBody;
    private Animator anim;
    private CapsuleCollider capsule;
    private Vector3 playerDestination;
    private bool isGrounded = false;

    [SerializeField] private GameObject PlayerArms;

    [Header("--- Movement Variables ---")]
    [SerializeField] private float currentSpeed = 1.5f;
    [SerializeField] private float jumpSpeed = 1.5f;
    [SerializeField] private float peekSpeed;
    [SerializeField] private float runSmoothTime;
    [SerializeField] private LayerMask allButPlayer;
        
    float t = 10f;    

    //Testing
    //private PlayerInput playerInput;
    //private PlayerInputActions playerInputActions;

    //private InputActionMap UIActionMap; /*new PlayerInputActions.UIActions()*/
    //private InputActionMap PlayerActionMap;    

    Vector2 move = Vector2.zero;
    Vector2 lookValue = Vector2.zero;
    //Vector3 isGroundedNormal;
    //private bool crouch = false;
    private bool jump = false;

    public bool fireTrigger;
    public bool fireBool = false;

    //private bool _canFire   = true;
    private bool _canSprint = true;

    //private bool _isFiring = false;
    private bool _isAiming = false;
    private bool _isSprinting = false;
    //private bool _isReloading = false;

    //[SerializeField] private KeyCode[] keys;
    //[HideInInspector] public int weaponSelect;

    //public bool IsFiring { get => _isFiring; set => _isFiring = value; }
    public bool IsAiming { get => _isAiming; set => _isAiming = value; }
    public bool IsSprinting { get => _isSprinting; set => _isSprinting = value; }

    void Start()
    {
        _weaponManager = GetComponentInChildren<WeaponManager>();        
        currentPlayerState = Normal_State.GetInstance();        
        rBody = GetComponent<Rigidbody>();
        capsule   = GetComponent<CapsuleCollider>();        
        camLook.InitSettings(transform, cam.transform);

        playerHealth = playerStatsSO.PlayerHealth;
        playerArmor = playerStatsSO.PlayerArmor;
        playerStamina = playerStatsSO.PlayerStamina;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    switch (other.tag)
    //    {
    //        case "ArmorSO":
    //            AmmoCollect.onAmmoCollect?.Invoke();
    //            break;
    //        case "HealthPackSO":
    //            HealthCollect.onHealthCollect?.Invoke();
    //            break;
    //        case "AmmoSO":
    //            AmmoCollect.onAmmoCollect?.Invoke();
    //            break;
    //        case "WeaponItemSO":
    //            //WeaponCollect.onWeaponDelegate?.Invoke();
    //            break;
    //    }

    //    //if (other.CompareTag("ArmorSO"))
    //    //{

    //    //}
    //    //if (other.CompareTag("HealthPackSO"))
    //    //{

    //    //}
    //    //if (other.CompareTag("AmmoSO"))
    //    //{

    //    //}
    //    //if (other.CompareTag("KeyItem"))
    //    //{
    //    //    _weaponManager.keyItemPickUpList.Add(other.gameObject);
    //    //}
    //    //if (other.CompareTag("WeaponItemSO"))
    //    //{
    //    //    _weaponManager.weaponPickUpList.Add(other.gameObject);
    //    //}
    //}

    public void OnMove(InputAction.CallbackContext context)
    {
        move =  context.ReadValue<Vector2>();
    }
    public void OnSprint(InputAction.CallbackContext context)
    {
        if(_canSprint)
            IsSprinting = context.performed;          
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        jump = context.performed;
    }

    public void OnLook(InputAction.CallbackContext context)
    {       
        lookValue = context.ReadValue<Vector2>();        
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (_weaponManager.weaponShoot != null)
        {
            if (_weaponManager.weaponShoot.CanFire && !_weaponManager.weaponShoot.IsReloading)
            {
                if (context.started)
                {                    
                    StartFiring();
                }
                if (context.canceled)
                {
                    StopFiring();
                }                
            }
        }       
    }    

    public void OnReload(InputAction.CallbackContext context)
    {
        if (_weaponManager.weaponShoot != null)
        {
            if (_weaponManager.weaponShoot.CanReload)
            {
                if (context.performed)
                {
                    StopFiring();
                    StartCoroutine(_weaponManager.weaponShoot.Reloading());
                }
            }
        }
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        IsAiming = context.performed;       

        if (_weaponManager.CurrentWeaponType != WeaponType.Unarmed)
        {
            _weaponManager.ArmsAnim.SetBool("isAiming", IsAiming);
        }
    }        

    bool CheckGrounded() 
    {        
        return Physics.CheckSphere(transform.position, 
                                   0.25f, 
                                   allButPlayer, 
                                   QueryTriggerInteraction.Ignore);                     
    }       

    void FixedUpdate()
    {
        isGrounded = CheckGrounded();
        if (isGrounded)
        {
            playerDestination = transform.forward * move.y + transform.right * move.x;                        

            float speedMul = currentSpeed;
            if (move == Vector2.zero) return;            
            if (IsSprinting)
            {
                speedMul *= 2.0f;               
            }

            playerDestination.x *= speedMul;
            playerDestination.z *= speedMul;
            playerDestination.y *= 0f;

            if (jump)
            {               
                float JumpForce = Mathf.Sqrt(jumpSpeed * -2f * Physics.gravity.y);
                rBody.AddForce(Vector3.up * JumpForce, ForceMode.VelocityChange);
                
                jump = false;
            }

        }       

        Vector3 rbDestination = rBody.position + playerDestination * Time.fixedDeltaTime;
        rBody.MovePosition(rbDestination);
    }
        
    // Update is called once per frame
    void Update()
    {
        if (_weaponManager.weaponShoot != null)
        {
            if (Time.timeScale <= 0f)
                playerMovement.Fire.Disable();
            else
                playerMovement.Fire.Enable();
        }


        if (playerStamina <= 0)
        {
            StartCoroutine(RegainStamina());
        }

        if (IsSprinting && playerStamina > 0)
        {            
            playerStamina -= 0.1f;

            if (_weaponManager.CurrentWeaponType != WeaponType.Unarmed)
            {
                _weaponManager.weaponShoot.CantShoot();
            }
            PlayerUIManager.playerStaminaUpdate?.Invoke();
        }
        else if(!IsSprinting)
        {
            if (_weaponManager.CurrentWeaponType != WeaponType.Unarmed)
            {
                _weaponManager.weaponShoot.CanFire = true;
            }
        }

        RotPlayerView();            

        AimCheck();        

        if (_weaponManager.ArmsAnim && _weaponManager.CurrentWeaponType != WeaponType.Unarmed)
        {
            _weaponManager.ArmsAnim.SetFloat("PlayerVelocity", playerDestination.magnitude, runSmoothTime, Time.deltaTime);
        }        
    }

    IEnumerator RegainStamina()
    {
        yield return new WaitForSeconds(10f);
        playerStamina = playerStatsSO.PlayerStamina;
        PlayerUIManager.playerStaminaUpdate?.Invoke();
    }

    private void AimCheck()
    {
        if (IsAiming)
        {            
            cam.GetComponent<Camera>().fieldOfView = Mathf.Lerp(cam.fieldOfView, 30f, t * Time.deltaTime);
            playerBodyCam.GetComponent<Camera>().fieldOfView = Mathf.Lerp(playerBodyCam.fieldOfView, 30f, t * Time.deltaTime);

            IsSprinting = !IsAiming;                       
        }
        if (!IsAiming)
        {
            cam.GetComponent<Camera>().fieldOfView = Mathf.Lerp(cam.fieldOfView, 60f, t * Time.deltaTime);
            playerBodyCam.GetComponent<Camera>().fieldOfView = Mathf.Lerp(playerBodyCam.fieldOfView, 60f, t * Time.deltaTime);

            IsAiming = false;                       
        }        
    }

    void RotPlayerView()
    {
        if (Mathf.Abs(Time.timeScale) < float.Epsilon) return;
       
        camLook.CameraLookRotation(lookValue, transform, cam.transform);      
    }
}
