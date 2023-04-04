using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Rigidbody rigidBody;
    Animator animator;
    PlayerInput playerInput;
    CharacterController characterController;

    //Movimentacao do personagem
    private Vector2 characterWalkInput;
    private Vector3 characterWalk;
    private Vector3 positionToLookAt;
    private bool isMoving;
    private bool jumpPressed;
    private bool isGrounded;
    private float gravity = -9.81f;
    private float verticalVelocity;
    private float rotationVelocity = 5f;

    [SerializeField] private float velocityWalk;
    [SerializeField] private float numberOfJumps;
    [SerializeField] private float jumpHeight;
    [SerializeField] private Transform groundVerify;
    [SerializeField] private LayerMask sceneryMask;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        SetMovement();
        AnimationHandler();
        RotationHandler();
        SetJump();
    }

    void SetMovement()
    {
        characterController.Move(characterWalk * velocityWalk * Time.deltaTime);
    }

    void SetJump()
    {
        isGrounded = Physics.CheckSphere(groundVerify.position, 0.3f, sceneryMask);

        if (numberOfJumps > 0 && isGrounded)
        {
            if (jumpPressed)
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
                numberOfJumps -= 1;
            }
        }
        if(isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -1f;
            numberOfJumps = 1;
        }

        verticalVelocity += gravity * Time.deltaTime;

        characterController.Move(new Vector3(0, verticalVelocity, 0) * Time.deltaTime);
    }

    void RotationHandler()
    {
        positionToLookAt = new Vector3(characterWalk.x, characterWalk.y, characterWalk.z);
        Quaternion currentRotation = transform.rotation;

        if (isMoving)
        {
            Quaternion lookAtRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, lookAtRotation, rotationVelocity * Time.deltaTime);
        }
    }

    void AnimationHandler()
    {
        if (isMoving && !animator.GetBool("isWalking"))
        {
            animator.SetBool("isWalking", true);
        }
        if (!isMoving && animator.GetBool("isWalking"))
        {
            animator.SetBool("isWalking", false);
        }
    }

    public void WalkInput(InputAction.CallbackContext context)
    {
        characterWalkInput = context.ReadValue<Vector2>();
        characterWalk = new Vector3(characterWalkInput.x, 0, characterWalkInput.y);

        isMoving = characterWalkInput.x != 0 || characterWalkInput.y != 0;
    }

    public void JumpInput(InputAction.CallbackContext context)
    {
        jumpPressed = context.ReadValueAsButton();
    }
}
