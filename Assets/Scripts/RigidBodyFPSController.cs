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
    [SerializeField] private float currentSpeed = 2.0f;    
    [SerializeField] private float jumpSpeed = 1.5f;
    [SerializeField] private float peekSpeed;
    [SerializeField] private LayerMask allButPlayer;

    //Testing
    //private InputActionMap UIActionMap; /*new PlayerInputActions.UIActions()*/
    //private InputActionMap PlayerActionMap;    

    Vector2 move  = Vector2.zero;
    Vector2 lookValue = Vector2.zero;
    //Vector3 isGroundedNormal;
    private bool sprint = false;
    private bool walk   = false;
    private bool jump   = false;  
    
    void Start()
    {
        anim = GetComponent<Animator>();
        rBody = GetComponent<Rigidbody>();
        capsule   = GetComponent<CapsuleCollider>();
        camLook.InitSettings(transform, cam.transform);

        //UIActionMap     = new InputActionAsset().FindActionMap("UI");
        //PlayerActionMap = new InputActionAsset().FindActionMap("Player");
    }

    //Est-ce que c'est mieux de faire une fonction a part pour le mouvement du player
    //                  ou
    //Est-ce que c'est correct si on le met dans le void OnMove()
    public void OnMove(InputAction.CallbackContext context)
    {
        move =  context.ReadValue<Vector2>();
        

        //Debug
    }
    public void OnSprint(InputAction.CallbackContext context)
    {
        sprint = context.performed;        
    }
    public void OnWalk(InputAction.CallbackContext context)
    {
        walk = context.performed;       
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        jump = context.performed;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        //if (Mathf.Abs(Time.timeScale) < float.Epsilon) return;
        lookValue = context.ReadValue<Vector2>();
        //float oldYRotation = transform.eulerAngles.y;
        //camLook.CameraLookRotation(value, transform, /*_playerHead.transform,*/ cam.transform);
        //Quaternion velRotation = Quaternion.AngleAxis(transform.eulerAngles.y - oldYRotation, Vector3.up);
        //rBody.velocity = velRotation * rBody.velocity;
        //RotPlayerView();
    }

    bool CheckGrounded() 
    {
        //Vector3 SphereOffset = new Vector3(0f, 1f, 0f);
        //RaycastHit hitInfo;
        return Physics.CheckSphere(transform.position, 
                                   0.25f, 
                                   allButPlayer, 
                                   QueryTriggerInteraction.Ignore);
        //{
        //    isGrounded = true;
        //    isGroundedNormal = hitInfo.normal;
        //}
        //else        
        //    isGrounded = false;
        //    isGroundedNormal = Vector3.up;                   
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

            playerDestination = cam.transform.forward * move.y + cam.transform.right * move.x;
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
                speedMul *= 1.5f;
            }
            if (walk)
            {
                speedMul *= 0.5f;
            }

            playerDestination.x *= speedMul;
            playerDestination.z *= speedMul;
            playerDestination.y *= 0f;

            if (jump)
            {
                //rBody.drag = 0.0f;
                float JumpForce = Mathf.Sqrt(jumpSpeed * -2f * Physics.gravity.y);
                rBody.AddForce(Vector3.up * JumpForce, ForceMode.VelocityChange);
                jump = false;
            }

            Vector3 rbDestination = rBody.position + playerDestination * Time.fixedDeltaTime;
            rBody.MovePosition(rbDestination);
            Debug.Log("Move X value : " + move.x + ", Move Y value : " + move.y + ", Current Speed : " + playerDestination.magnitude);
        }
        anim.SetBool("isGrounded", isGrounded);
    }

    // Update is called once per frame
    void Update()
    {
        RotPlayerView();

        //if (jump /*&& isGrounded*/)
        //{
        //    jump = true;
        //}
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
