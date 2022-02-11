using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class RigidBodyFPSCntroller : MonoBehaviour
{    
    private CharacterController characterController;
    public Camera cam;
    //public CameraController camLook = new CameraController();

    //private Rigidbody rigidbody;
    private CapsuleCollider capsule;
    private Vector3 playerVel;
    private bool isGrounded = false;

    [Header("--- Movement Variables ---")]
    [SerializeField] private float walkSpeed = 0.5f;
    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private float runSpeed  = 2.0f;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float peekSpeed;    

    //private CapsuleCollider capsule;

    Vector2 move = Vector2.zero;
    private bool sprint = false;
    private bool walk   = false;
    private bool jump   = false;  
    
    void Start()
    {
        //rigidbody = GetComponent<Rigidbody>();
        capsule   = GetComponent<CapsuleCollider>();
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

    void FixedUpdate()
    {
        isGrounded = characterController.isGrounded;

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

        characterController.Move(playerVel.normalized * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        RotPlayerView();

        if (jump && isGrounded)
        {
            playerVel.y = jumpSpeed;
        }
    }

    void RotPlayerView() 
    {
        
    }
}
