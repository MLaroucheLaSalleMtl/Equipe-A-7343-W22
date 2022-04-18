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

    //WeaponShoot wpShoot;

    //Move to GameManager after Demo
    //public RigidBodyFPSController instance = null;

    Coroutine fireCoroutine;

    private void Awake()
    {
        //controls = new PlayerInputActions();
        //playerMovement = controls.Player;


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

    WeaponManager _weaponManager = null;    

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
    }
    
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
                    //WeaponShoot.testOnFireAutoDelegate?.Invoke(context);
                    StartFiring();
                }
                if (context.canceled)
                {
                    StopFiring();
                }
                //context.started

                //playerMovement.Fire.started += context => StartFiring();
                //playerMovement.Fire.canceled += context => StopFiring();

                //if (context.performed)
                //{
                //    //_weaponManager.weaponShoot.IsFiring = true;
                //    
                //}

                //switch (context.interaction)
                //{
                //    case TapInteraction:
                //        if (context.phase == InputActionPhase.Performed)
                //        {
                //            WeaponShoot.onFireSemiDelegate?.Invoke();
                //            _weaponManager.weaponShoot.IsFiring = false;
                //        }
                //        if (context.phase == InputActionPhase.Canceled)
                //        {
                //            _weaponManager.weaponShoot.IsFiring = false;
                //        }
                //        _weaponManager.weaponShoot.IsFiring = false;
                //        break;

                //    case PressInteraction:
                //        if (context.phase == InputActionPhase.Performed)
                //        {
                //            _weaponManager.weaponShoot.IsFiring = true;
                //            WeaponShoot.onFireAutoDelegate?.Invoke();
                //        }
                //        if (context.phase == InputActionPhase.Started)
                //        {
                //            return;
                //        }
                //        if (context.phase == InputActionPhase.Canceled)
                //        {
                //            _weaponManager.weaponShoot.IsFiring = false;
                //        }
                //        break;
                //}                

                //switch (context.interaction)    
                //{
                //    case HoldInteraction:
                //        break;

                //    case InputActionPhase.Performed:
                //        if (context.interaction is TapInteraction)
                //        {
                //        }
                //        break;

                //    case InputActionPhase.Started:
                //        if (context.interaction is HoldInteraction)
                //        {
                //            _weaponManager.weaponShoot.IsFiring = true;
                //            WeaponShoot.onFireAutoDelegate?.Invoke();
                //        }
                //        if (context.interaction is TapInteraction)
                //        {

                //        }
                //        break;

                //    case InputActionPhase.Canceled:
                //        _weaponManager.weaponShoot.IsFiring = false;
                //        break;
                //}
            }
        }
        
        //if (true)
        //{

        //}
        //if (context.performed)
        //{
        //    _weaponManager.weaponShoot.IsFiring = true;
        //    WeaponShoot.onFireSemiDelegate?.Invoke();
        //}
    }

    //bool DontFire()
    //{
    //    return false;
    //}

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

    //public void OnFire(InputAction.CallbackContext context)
    //{
    //    //fireTrigger = context.performed;

    //    //fireBool = context.performed;
    //    ////anim.SetBool("FireBool", fireBool);
    //    //fireBool = false;
    //    ////Debug.Log(gameObject.GetComponent<WeaponManager>().currDMG);               

    //    if (_canFire)
    //    {
    //        IsFiring = context.performed;


    //        if (_weaponManager.CurrentWeaponType != WeaponType.Unarmed)
    //        {
    //            //if(!IsAiming && IsFiring)
    //            _weaponManager.ArmsAnim.SetBool("isFiring", IsFiring && !IsAiming/*_weaponManager._currentWeapon.ArmsFireAnim.name*/);
    //            //else if (IsAiming/* && IsFiring*/)
    //            _weaponManager.ArmsAnim.SetBool("isAiming&Firing", IsFiring && IsAiming);
    //            //else
    //            //_weaponManager.ArmsAnim.SetBool("isFiring", IsFiring /*&& !IsAiming ||*/ /*_weaponManager._currentWeapon.ArmsFireAnim.name*/);
    //            //_weaponScript.WeaponAnim.SetBool("IsFiring", _isFiring);
    //        }
    //    }

    //    IsFiring = false;
    //}


    public void OnAim(InputAction.CallbackContext context)
    {
        IsAiming = context.performed;       

        if (_weaponManager.CurrentWeaponType != WeaponType.Unarmed)
        {
            _weaponManager.ArmsAnim.SetBool("isAiming", IsAiming);
        }
    }

    //public void OnReload(InputAction.CallbackContext context)
    //{
    //    _isReloading = context.performed;
    //}

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

                //anim.SetTrigger("Jump");
                jump = false;
            }

        }       

        Vector3 rbDestination = rBody.position + playerDestination * Time.fixedDeltaTime;
        rBody.MovePosition(rbDestination);
    }
        
    // Update is called once per frame
    void Update()
    {
        //if (IsSprinting)
        //{
        //    PlayerUIManager.playerStaminaUpdate?.Invoke();
        //}
        //_currentWeaponType = weaponClassScriptableObject.Current_State.WeaponTypeUpdater(weaponClassScriptableObject.WeaponType);
        RotPlayerView();

        //ReloadCheck();
        //        

        AimCheck(/*IsAiming*/);

        //if (IsFiring && IsAiming)

        //if (IsAiming)
        //{
        //    //t += Time.smoothDeltaTime * 2.25f;
        //    //t = Mathf.Clamp(t, 0.0f, 1.0f);
        //    cam.GetComponent<Camera>().fieldOfView = Mathf.Lerp(cam.fieldOfView, 30f, t * Time.deltaTime);
        //    playerBodyCam.GetComponent<Camera>().fieldOfView = Mathf.Lerp(playerBodyCam.fieldOfView, 30f, t * Time.deltaTime);

        //    _isSprinting = !IsAiming;
        //    //_weaponManager.ArmsAnim.SetBool("isAiming", isAiming);           

        //    //cam.GetComponent<Camera>().fieldOfView = Mathf.SmoothStep(60f, 30f, Time.deltaTime * 100f;
        //}
        //if (!IsAiming)
        //{
        //    //if (t != 1f)
        //    //{
        //    //t = 0.0f;
        //    //t += Time.deltaTime * 5f;
        //    //t = Mathf.Clamp(t, 0.0f, 1.0f);
        //    cam.GetComponent<Camera>().fieldOfView = Mathf.Lerp(cam.fieldOfView, 60f, t * Time.deltaTime);
        //    playerBodyCam.GetComponent<Camera>().fieldOfView = Mathf.Lerp(playerBodyCam.fieldOfView, 60f, t * Time.deltaTime);
        //    IsAiming = false;
        //    //cam.GetComponent<Camera>().fieldOfView = Mathf.SmoothStep(60f, 30f, Time.deltaTime * 100f);

        //    //}            
        //}

        //for (int i = 0; i < keys.Length; i++)
        //{
        //    int previousWeaponSelect = weaponSelect;

        //    if (Input.GetKeyDown(keys[i]))
        //    {
        //        _isAiming = false;               
        //        _weaponManager.ArmsAnim.Play(_weaponManager._currentWeapon.ArmsLowerAnim.name);
        //        weaponSelect = i;
        //    }

        //    if (previousWeaponSelect != weaponSelect) 
        //        _weaponManager.WeaponSpawnByClass(_weaponManager.CurrentWeaponType = (WeaponType)weaponSelect);
        //    //if (previousWeaponSelect == weaponSelect) _weaponManager.WeaponSpawnByClass(WeaponType.Unarmed);
        //}

        //if (PlayerArms.activeSelf/* == true*/)
        //{

        //if (Input.GetKeyUp("1"))
        //{

        //    //WeaponCheck();
        //    //currentPlayerState = Normal_State.GetInstance();
        //    //weaponClassScriptableObject.WeaponType = WeaponType.UnArmed;
        //    //anim.runtimeAnimatorController = animators[0] as RuntimeAnimatorController;
        //    isAiming = false;
        //    //weaponManager.Weapons[0].SetActive(false);
        //    //weaponManager.Weapons[1].SetActive(false);
        //}
        //if (Input.GetKeyUp("2"))
        //{
        //    //WeaponCheck();
        //    //currentPlayerState = AssaultRifle_State.GetInstance();
        //    //weaponClassScriptableObject.WeaponType = WeaponType.AssaultRifle;
        //    //anim.runtimeAnimatorController = animators[1] as RuntimeAnimatorController;
        //    isAiming = false;
        //    //weaponManager.Weapons[0].SetActive(true);
        //    //weaponManager.Weapons[1].SetActive(false);
        //}
        //if (Input.GetKeyUp(KeyCode.Alpha3)/* && _weaponManager.PistolsPrefab[0]*//* != null*/)
        //{
        //    //PlayerArms.SetActive(true);
        //    //_weaponManager.WeaponPrefabs[0].SetActive(true);
        //    //_weaponManager.WeaponSpawnByClass(_weaponManager._currentWeapon.WeaponType);
        //    //if (PlayerArms.activeSelf)
        //    //{
        //        //anim.SetBool("Pistol", true);
        //        //anim.SetBool("AssaultRifle", false);
        //    //}                
        //    //WeaponCheck();
        //    //currentPlayerState = Pistol_State.GetInstance();
        //    //weaponClassScriptableObject.WeaponType = WeaponType.Pistol;
        //    //anim.runtimeAnimatorController = animators[2] as RuntimeAnimatorController;
        //    isAiming = false;
        //    //weaponManager.Weapons[0].SetActive(false);
        //    //weaponManager.Weapons[1].SetActive(true);
        //}            

        //if (IsFiring)
        //{
        //    _weaponManager.ArmsAnim.SetBool("isFiring", IsFiring /*&& !IsAiming ||*/ /*_weaponManager._currentWeapon.ArmsFireAnim.name*/);
        //    //anim.SetTrigger("FireTrigger");
        //    //fireTrigger = false;
        //}

        if (_weaponManager.ArmsAnim && _weaponManager.CurrentWeaponType != WeaponType.Unarmed)
        {
            _weaponManager.ArmsAnim.SetFloat("PlayerVelocity", playerDestination.magnitude, runSmoothTime, Time.deltaTime);
        }

        //if (fireBool)
        //{
        //    anim.SetBool("FireBool", fireBool);
        //    fireBool = false;
        //}

        //anim.SetBool("Sprint", sprint);
        ////anim.SetBool("Walk", walk);
        //anim.SetBool("isGrounded", CheckGrounded());
        //}

        //anim.SetFloat("DirectionH", move.x);
        //anim.SetFloat("DirectionV", move.y);

        //Debug.Log(currentPlayerState.ToString());

        //Debug.Log(weaponClassScriptableObject.WeaponType.ToString()/*, anim.runtimeAnimatorController*/);
        //For Debug
        //Debug.Log("Move X value : " + move.x +
        //          ", Move Y value : " + move.y +
        //          ", Current Speed : " + playerDestination.magnitude +
        //          ", FOV : " + cam.fieldOfView +
        //          ", isAiming : " + isAiming + ", T : " + t /*+
        //          ", Current Action Map : " + playerInput.currentActionMap*/);
    }

    //private bool ReloadCheck()
    //{
    //    if (_isReloading)
    //    {
    //        Debug.Log("- Reloading Weapon -");
    //        _weaponScript.CanFire = !_isReloading;
    //        IsAiming = !_isReloading;
    //    }

    //    return _isReloading;
    //}

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

        //return IsAiming;
    }

    void RotPlayerView()
    {
        if (Mathf.Abs(Time.timeScale) < float.Epsilon) return;
       
        camLook.CameraLookRotation(lookValue, transform, cam.transform);      
    }
}
