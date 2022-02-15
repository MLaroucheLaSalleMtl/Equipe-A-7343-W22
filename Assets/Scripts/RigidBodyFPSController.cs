using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class RigidBodyFPSController : MonoBehaviour
{    
    private CharacterController characterController;
    public Camera cam;
    public Transform _playerHead;
    public TempCamLook camLook = new TempCamLook();

    private Rigidbody rBody;
    private CapsuleCollider capsule;
    private Vector3 playerVel;
    private bool isGrounded = false;

    [Header("--- Movement Variables ---")]
    [HideInInspector] private float currentSpeed = 2.0f;
    [SerializeField] private float runSpeed  = 4.0f;
    [SerializeField] private float forwardMoveSpeed = 2.0f;
    [SerializeField] private float backwardMoveSpeed = 1.0f;
    [SerializeField] private float straffeSpeed = 1.0f;
    [SerializeField] private float slowWalkSpeed = 1.0f;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float peekSpeed;
    [SerializeField] private LayerMask allButPlayer;

    //private CapsuleCollider capsule;

    Vector2 move  = Vector2.zero;
    Vector2 value = Vector2.zero;
    Vector2 isGroundedNormal;
    private bool sprint = false;
    private bool walk   = false;
    private bool jump   = false;  
    
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        capsule   = GetComponent<CapsuleCollider>();
        camLook.InitSettings(transform, _playerHead.transform, cam.transform);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        move =  context.ReadValue<Vector2>();
        if (move == Vector2.zero) return;
        if (move.x > 0 || move.x < 0)
        {
            currentSpeed = straffeSpeed;
        }
        if (move.y < 0)
        {
            currentSpeed = backwardMoveSpeed;
        }
        if (move.y > 0)
        {
            currentSpeed = forwardMoveSpeed;
        }

        //Debug
        Debug.Log("Move X value : " + move.x + ", Move Y value : " + move.y);
    }
    public void OnSprint(InputAction.CallbackContext context)
    {
        sprint = context.performed;
        if (sprint)
        {
            currentSpeed = runSpeed;
        }
    }
    public void OnSlowWalk(InputAction.CallbackContext context)
    {
        walk = context.performed;
        if (walk)
        {
            currentSpeed = slowWalkSpeed;
        }
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        jump = context.performed;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (Mathf.Abs(Time.timeScale) < float.Epsilon) return;
        value = context.ReadValue<Vector2>();
        //float oldYRotation = transform.eulerAngles.y;
        camLook.CameraLookRotation(value, transform, _playerHead.transform, cam.transform);
        //Quaternion velRotation = Quaternion.AngleAxis(transform.eulerAngles.y - oldYRotation, Vector3.up);
        //rBody.velocity = velRotation * rBody.velocity;
        //RotPlayerView();
    }

    void CheckGrouded() 
    {
        //Vector3 SphereOffset = new Vector3(0f, 1f, 0f);
        RaycastHit hitInfo;
        if (Physics.SphereCast(transform.position /*+ SphereOffset*/, 0.25f, Vector3.down,
        out hitInfo, 0.75f, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            isGrounded = true;
            isGroundedNormal = hitInfo.normal;
        }
        else
            isGrounded = false;
            isGroundedNormal = Vector2.up;

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
        CheckGrouded();

        //rBody.drag = 5f;
        //move = PlayerInput();

        //playerVel = new Vector3(move.x, 0f, move.y).normalized;

        //float speed = currentSpeed;
        //if (move.y < 0)
        //{
        //    speed -= moveSpeed;
        //}
        //if (move.y > 0)
        //{
        //    speed = moveSpeed;
        //}

        //if (sprint && isGrounded)
        //{
        //    speed = sprint ? forwardMoveSpeed : runSpeed;
        //}
        //if (walk && isGrounded)
        //{
        //    speed = walk ? forwardMoveSpeed : walkSpeed;
        //}
        //playerVel *= speed;

        if ((Mathf.Abs(move.x) > float.Epsilon || Mathf.Abs(move.y) > float.Epsilon) /*&& isGrounded*/)
        {
            Vector3 playerDestination = cam.transform.forward * move.y + cam.transform.right * move.x;

            playerDestination = Vector3.ProjectOnPlane(playerDestination, isGroundedNormal).normalized;

            playerDestination.x = playerDestination.x * currentSpeed;
            playerDestination.z = playerDestination.z * currentSpeed;
            playerDestination.y = playerDestination.y * currentSpeed;

            rBody.AddForce(playerDestination, ForceMode.Impulse);
            //rBody.MovePosition(playerDestination);
        }

        //if (isGrounded)
        //{
        //    rBody.drag = 5f;
        //}
        //else
        //{
        //    rBody.drag = 0f;
        //}
        jump = false;
        //Quaternion velRotation = Quaternion.AngleAxis(transform.eulerAngles.y, Vector3.up);

        //transform.Translate(playerVel.normalized);

        //characterController.Move(playerVel.normalized * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        //RotPlayerView();

        if (jump && isGrounded)
        {
            playerVel.y = jumpSpeed;
        }
    }

    void RotPlayerView() 
    {
        if (Mathf.Abs(Time.timeScale) < float.Epsilon) return;

        if (_playerHead.transform.localRotation.y == 50f || _playerHead.transform.localRotation.y == -50f)
        {
            camLook.CameraLookRotation(value, transform, _playerHead.transform, cam.transform);
        }
    }
}
