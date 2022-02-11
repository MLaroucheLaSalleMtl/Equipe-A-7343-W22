using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LocomotionCharacterController))]
public class LocomotionCharacterController : MonoBehaviour
{    
    private CharacterController characterController;

    [Header("--- Movement Variables ---")]
    [SerializeField] private float walkSpeed = 0.5f;
    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private float runSpeed  = 2.0f;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float peekSpeed;
    
    private Vector3 playerVel;
    private bool isGrounded = false;

    private CapsuleCollider capsule;

    Vector2 move = Vector2.zero;
    private bool sprint = false;
    private bool walk   = false;
    private bool jump   = false;    

    public void OnMove(InputAction.CallbackContext context)
    {
        move =  context.ReadValue<Vector2>();
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

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        capsule = GetComponent<CapsuleCollider>();
    }

    void FixedUpdate()
    {
        isGrounded = characterController.isGrounded;

        //if (move.y < 0)
        //{
        //    speed -= moveSpeed; 
        //}
        //if (move.y > 0)
        //{
        //    speed = moveSpeed;
        //}       

        playerVel = new Vector3(move.x, 0f, move.y).normalized;

        float speed = moveSpeed;
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
        if (jump && isGrounded)
        {
            playerVel.y = jumpSpeed;
        }
    }
}
