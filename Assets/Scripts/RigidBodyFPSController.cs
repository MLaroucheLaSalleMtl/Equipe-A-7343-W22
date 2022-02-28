using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class RigidBodyFPSController : MonoBehaviour
{        
    public Camera cam;
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
    //private const float MaxWalkSpeed = 1.5f;

    //Testing
    //private InputActionMap UIActionMap; /*new PlayerInputActions.UIActions()*/
    //private InputActionMap PlayerActionMap;    

    Vector2 move  = Vector2.zero;
    Vector2 lookValue = Vector2.zero;
    //Vector3 isGroundedNormal;
    private bool sprint   = false;
    private bool crouch   = false;
    private bool jump     = false;
    private bool fire     = false;
    private bool isAiming = false;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rBody = GetComponent<Rigidbody>();
        capsule   = GetComponent<CapsuleCollider>();
        camLook.InitSettings(transform, cam.transform);

        //UIActionMap     = new InputActionAsset().FindActionMap("UI");
        //PlayerActionMap = new InputActionAsset().FindActionMap("Player");
    }
    
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
        fire = context.performed;
        anim.SetBool("isFiring", fire);
        fire = false;
        //anim.SetTrigger("Fire");
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        isAiming = context.performed;        
        anim.SetBool("isAiming", isAiming);
        isAiming = false;
    }

    bool CheckGrounded() 
    {        
        return Physics.CheckSphere(transform.position, 
                                   0.25f, 
                                   allButPlayer, 
                                   QueryTriggerInteraction.Ignore);                       
    }    

    //private Vector2 PlayerInput() 
    //{
    //    move = new Vector2
    //    {
    //        x = Input.GetAxis("Horizontal"),
    //        y = Input.GetAxis("Vertical")
    //    };
    //    return move;
    //}

    void FixedUpdate()
    {
        isGrounded = CheckGrounded();
        if (/*(Mathf.Abs(move.x) > float.Epsilon || Mathf.Abs(move.y) > float.Epsilon) &&*/ isGrounded)
        {
            //rBody.drag = 5f;
            //if (move.y < 0)
            //{
            //    currentSpeed = moveSpeed;
            //}
            //if (move.y > 0)
            //{
            //    currentSpeed = moveSpeed;
            //}

            //if ((Mathf.Abs(move.x) > float.Epsilon || Mathf.Abs(move.y) > float.Epsilon)/* && isGrounded*/)
            //{
            //playerDestination = new Vector3(playerDestination.x/*cam.transform.right.x * move.x*/, 0f, playerDestination.y/*cam.transform.forward.x * move.y*/);

            playerDestination = transform.forward * move.y + transform.right * move.x;
            //playerDestination = Vector3.ProjectOnPlane(playerDestination, isGroundedNormal).normalized;

            //playerDestination.x = playerDestination.x * currentSpeed;
            //playerDestination.z = playerDestination.z * currentSpeed;
            //playerDestination.y = playerDestination.y * currentSpeed;

            //rBody.AddForce(playerDestination, ForceMode.Impulse);
            //}
                        

            float speedMul = currentSpeed;
            if (move == Vector2.zero) return;
            //if (move.x > 0 || move.x < 0)
            //{
            //    speedMul *= 0.45f;
            //}
            if (sprint)
            {
                speedMul *= 2.0f;
                //move.y = 2.0f;
            }

            if (crouch)
            {
                speedMul *= 0.75f;
            }

            //playerDestination.x *= Mathf.SmoothStep(speedMul, MaxSpeed, runSmoothTime);
            //playerDestination.z *= Mathf.SmoothStep(speedMul, MaxSpeed, runSmoothTime);

            playerDestination.x *= speedMul;
            playerDestination.z *= speedMul;
            playerDestination.y *= 0f;

            if (jump)
            {
                //rBody.drag = 0.0f;
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
        RotPlayerView();

        //if (jump /*&& isGrounded*/)
        //{
        //    jump = true;
        //}

        //if (isAiming)
        //{
        //    isAiming = false;
        //}

        //if (sprint)
        //{
        //    currentSpeed += 3.0f * Time.deltaTime;
        //    currentSpeed = Mathf.Clamp(currentSpeed, 1.25f, 3.0f);
        //}
        //if (walk)
        //{
        //    currentSpeed -= 1.0f * Time.deltaTime;
        //    currentSpeed = Mathf.Clamp(currentSpeed, 0.5f, 1.5f);
        //}

        //if (fire)
        //{            
        //    fire = false;
        //}

        anim.SetBool("Sprint", sprint);
        //anim.SetBool("Walk", walk);
        anim.SetBool("isGrounded", CheckGrounded());
        anim.SetFloat("PlayerVelocity", playerDestination.magnitude, runSmoothTime, Time.deltaTime);
        anim.SetFloat("DirectionH", move.x);
        anim.SetFloat("DirectionV", move.y);
        Debug.Log("Move X value : " + move.x + ", Move Y value : " + move.y + ", Current Speed : " + playerDestination.magnitude);
    }

    //Temporary Test
    //private void LateUpdate()
    //{
    //    RotPlayerView();
    //}

    void RotPlayerView()
    {
        if (Mathf.Abs(Time.timeScale) < float.Epsilon) return;

        //if (_playerHead.transform.localRotation.y == 50f || _playerHead.transform.localRotation.y == -50f)
        //{
        camLook.CameraLookRotation(lookValue, transform, /*_playerHead.transform,*/ cam.transform);
        //}
    }
}
