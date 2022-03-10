using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class RigidBodyFPSController : MonoBehaviour
{
    WeaponClassScriptableObject weaponClassScriptableObject;
    WeaponType _currentWeaponType;

    public Camera cam;
    public Camera playerBodyCam;

    //public Transform _playerHead;
    public TempCamLook camLook = new TempCamLook();

    private Rigidbody rBody;
    private Animator anim;
    private CapsuleCollider capsule;
    private Vector3 playerDestination;
    private bool isGrounded = false;

    [Header("--- Movement Variables ---")]
    [SerializeField] private float currentSpeed = 1.5f;
    [SerializeField] private float jumpSpeed = 1.5f;
    [SerializeField] private float peekSpeed;
    [SerializeField] private float runSmoothTime;
    [SerializeField] private LayerMask allButPlayer;

    private const float MaxRunSpeed = 3.0f;
    float t = 10f;
    //private const float MaxWalkSpeed = 1.5f;

    //Testing
    //private PlayerInput playerInput;
    //private PlayerInputActions playerInputActions;

    //private InputActionMap UIActionMap; /*new PlayerInputActions.UIActions()*/
    //private InputActionMap PlayerActionMap;    

    Vector2 move  = Vector2.zero;
    Vector2 lookValue = Vector2.zero;
    //Vector3 isGroundedNormal;
    private bool sprint   = false;
    private bool crouch   = false;
    private bool jump     = false;    
    private bool isAiming = false;

    public bool fireTrigger;
    public bool fireBool = false;
    public bool isFiring = false;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        anim.runtimeAnimatorController = animators[0] as RuntimeAnimatorController;
        //_currentWeaponType = weaponClassScriptableObject.Current_State.WeaponTypeUpdater(weaponClassScriptableObject.WeaponType);
        //_currentWeaponType = weaponClassScriptableObject.WeaponType;
        //current_Controller = GetComponentInChildren<RuntimeAnimatorController>();
        rBody = GetComponent<Rigidbody>();
        capsule   = GetComponent<CapsuleCollider>();
        //cam = GetComponent<Camera>();
        camLook.InitSettings(transform, cam.transform);

        //testing
        //playerInput = FindObjectOfType<PlayerInput>();
        //playerInputActions = new PlayerInputActions();

        //UIActionMap     = new InputActionAsset().FindActionMap("UI");
        //PlayerActionMap = new InputActionAsset().FindActionMap("Player");
    }

    //private void OnEnable() {
    //    playerInputActions.UI.Enable();
    //    playerInput.actions["Pause"].performed += SwitchActionMap;
    //}

    //private void OnDisable() {
    //    playerInputActions.UI.Disable(); 
    //    playerInput.actions["Pause"].performed -= SwitchActionMap;
    //}

    //private void SwitchActionMap(InputAction.CallbackContext context)
    //{
    //    UIActionMap.Enable();
    //    PlayerActionMap.Disable();
    //}

    public void OnMove(InputAction.CallbackContext context)
    {
        move =  context.ReadValue<Vector2>();
    }
    public void OnSprint(InputAction.CallbackContext context)
    {
        sprint = context.performed;        
    }
    public void OnCrouch(InputAction.CallbackContext context)
    {
        crouch = context.performed;
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        jump = context.performed;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        //if (Mathf.Abs(Time.timeScale) < float.Epsilon) return;
        //if (Mathf.Abs(Time.timeScale) < float.Epsilon) return;
        lookValue = context.ReadValue<Vector2>();
        //camLook.CameraLookRotation(lookValue, transform, /*_playerHead.transform,*/ cam.transform);

        //float oldYRotation = transform.eulerAngles.y;
        //camLook.CameraLookRotation(value, transform, /*_playerHead.transform,*/ cam.transform);
        //Quaternion velRotation = Quaternion.AngleAxis(transform.eulerAngles.y - oldYRotation, Vector3.up);
        //rBody.velocity = velRotation * rBody.velocity;
        //RotPlayerView();
    }

    public void OnFire(InputAction.CallbackContext context)
    {       
        fireTrigger = context.performed;

        fireBool = context.performed;
        anim.SetBool("FireBool", fireBool);
        fireBool = false;
        //Debug.Log(gameObject.GetComponent<WeaponManager>().currDMG);
    }
    public void OnFireHold(InputAction.CallbackContext context)
    {
        isFiring = context.performed;
        anim.SetBool("isFiring", isFiring);
        isFiring = false;
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        isAiming = context.performed;
        anim.SetBool("isAiming", isAiming);
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
            if (sprint)
            {
                speedMul *= 2.0f;                
            }

            if (crouch)
            {
                speedMul *= 0.75f;
            }          

            playerDestination.x *= speedMul;
            playerDestination.z *= speedMul;
            playerDestination.y *= 0f;

            if (jump)
            {               
                float JumpForce = Mathf.Sqrt(jumpSpeed * -2f * Physics.gravity.y);
                rBody.AddForce(Vector3.up * JumpForce, ForceMode.VelocityChange);

                anim.SetTrigger("Jump");
                jump = false;
            }

        }       

        Vector3 rbDestination = rBody.position + playerDestination * Time.fixedDeltaTime;
        rBody.MovePosition(rbDestination);
    }
        
    // Update is called once per frame
    void Update()
    {
        //_currentWeaponType = weaponClassScriptableObject.Current_State.WeaponTypeUpdater(weaponClassScriptableObject.WeaponType);
        RotPlayerView();        

        if (Input.GetKeyUp("1"))
        {
            //WeaponCheck();
            currentPlayerState = Normal_State.GetInstance();
            //weaponClassScriptableObject.WeaponType = WeaponType.UnArmed;
            anim.runtimeAnimatorController = animators[0] as RuntimeAnimatorController;
            isAiming = false;
            //weaponManager.Weapons[0].SetActive(false);
            //weaponManager.Weapons[1].SetActive(false);
        }
        if (Input.GetKeyUp("2"))
        {
            //WeaponCheck();
            currentPlayerState = AssaultRifle_State.GetInstance();
            //weaponClassScriptableObject.WeaponType = WeaponType.AssaultRifle;
            anim.runtimeAnimatorController = animators[1] as RuntimeAnimatorController;
            isAiming = false;
            //weaponManager.Weapons[0].SetActive(true);
            //weaponManager.Weapons[1].SetActive(false);
        }
        if (Input.GetKeyUp("3"))
        {
            //WeaponCheck();
            currentPlayerState = Pistol_State.GetInstance();
            //weaponClassScriptableObject.WeaponType = WeaponType.Pistol;
            anim.runtimeAnimatorController = animators[2] as RuntimeAnimatorController;
            isAiming = false;
            //weaponManager.Weapons[0].SetActive(false);
            //weaponManager.Weapons[1].SetActive(true);
        }

        if (isAiming)
        {
            //t += Time.smoothDeltaTime * 2.25f;
            //t = Mathf.Clamp(t, 0.0f, 1.0f);
            cam.GetComponent<Camera>().fieldOfView = Mathf.Lerp(cam.fieldOfView, 45f, t * Time.deltaTime);
            playerBodyCam.GetComponent<Camera>().fieldOfView = Mathf.Lerp(playerBodyCam.fieldOfView, 45f, t * Time.deltaTime);
            //cam.GetComponent<Camera>().fieldOfView = Mathf.SmoothStep(60f, 30f, Time.deltaTime * 100f;
        }
        if (!isAiming)
        {
            //if (t != 1f)
            //{
                //t = 0.0f;
                //t += Time.deltaTime * 5f;
                //t = Mathf.Clamp(t, 0.0f, 1.0f);
            cam.GetComponent<Camera>().fieldOfView = Mathf.Lerp(cam.fieldOfView, 60f, t * Time.deltaTime);
            playerBodyCam.GetComponent<Camera>().fieldOfView = Mathf.Lerp(playerBodyCam.fieldOfView, 60f, t * Time.deltaTime);
            isAiming = false;
            //cam.GetComponent<Camera>().fieldOfView = Mathf.SmoothStep(60f, 30f, Time.deltaTime * 100f);

            //}            
        }

        if (fireTrigger)
        {
            anim.SetTrigger("FireTrigger");
            fireTrigger = false;
        }
        //if (fireBool)
        //{
        //    anim.SetBool("FireBool", fireBool);
        //    fireBool = false;
        //}

        anim.SetBool("Sprint", sprint);
        //anim.SetBool("Walk", walk);
        anim.SetBool("isGrounded", CheckGrounded());
        anim.SetFloat("PlayerVelocity", playerDestination.magnitude, runSmoothTime, Time.deltaTime);
        anim.SetFloat("DirectionH", move.x);
        anim.SetFloat("DirectionV", move.y);

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

    void RotPlayerView()
    {
        if (Mathf.Abs(Time.timeScale) < float.Epsilon) return;

        //if (_playerHead.transform.localRotation.y == 50f || _playerHead.transform.localRotation.y == -50f)
        //{
        camLook.CameraLookRotation(lookValue, transform, /*_playerHead.transform,*/ cam.transform);
        //}
    }
}
