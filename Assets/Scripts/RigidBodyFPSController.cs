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
    public TempCamLook camLook = new TempCamLook();

    private Rigidbody rBody;
    private CapsuleCollider capsule;
    private Vector3 playerVel;
    private bool isGrounded = false;

    [Header("--- Movement Variables ---")]
    [SerializeField] private float walkSpeed = 0.5f;
    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private float runSpeed  = 2.0f;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float peekSpeed;
    [SerializeField] private LayerMask allButPlayer;

    //private CapsuleCollider capsule;

    Vector2 move = Vector2.zero;
    Vector2 value;
    private bool sprint = false;
    private bool walk   = false;
    private bool jump   = false;  
    
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        capsule   = GetComponent<CapsuleCollider>();
        camLook.InitSettings(transform, cam.transform);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        move =  context.ReadValue<Vector2>();

        //Debug
        Debug.Log("Move X value : " + move.x + ", Move Y value : " + move.y);
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
        value = context.ReadValue<Vector2>();
        RotPlayerView();
    }

    void CheckGrouded() 
    {
        RaycastHit hitInfo;
        if (Physics.SphereCast(transform.position, 0.25f, Vector3.down, 
        out hitInfo, 0.75f, allButPlayer, QueryTriggerInteraction.Ignore)) 
        {
            isGrounded = true;
        }
    }

    void FixedUpdate()
    {
        CheckGrouded();

        playerVel = new Vector3(move.x, 0f, move.y).normalized;

        float speed = moveSpeed;
        //if (move.y < 0)
        //{
        //    speed -= moveSpeed;
        //}
        //if (move.y > 0)
        //{
        //    speed = moveSpeed;
        //}

        if (sprint && isGrounded)
        {
            speed = sprint ? moveSpeed : runSpeed;
        }
        if (walk && isGrounded)
        {
            speed = walk ? moveSpeed : walkSpeed;
        }
        playerVel *= speed;

        //Quaternion velRotation = Quaternion.AngleAxis(transform.eulerAngles.y, Vector3.up);

        //transform.Translate(playerVel.normalized);

        //characterController.Move(playerVel.normalized * Time.deltaTime);
        Vector3 playerDestination = rBody.position + playerVel * Time.fixedDeltaTime;
        rBody.MovePosition(playerDestination);
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

        camLook.CameraLookRotation(value, transform, cam.transform);
    }
}
