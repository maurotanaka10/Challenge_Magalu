using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    //Componentes do Objeto
    Animator animator;
    PlayerInputSystem playerInput;
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
    private float rotationVelocity = 10f;
    private float startNumberJumps;

    //Parametros do Animator
    private int a_isWalking;
    private int a_isJumping;

    [Header("Run Variables")]
    [SerializeField] private float velocity;

    [Header("Jump Variables")]
    [SerializeField] private float numberOfJumps;
    [SerializeField] private float jumpHeight;
    [SerializeField] private Transform groundVerify;
    [SerializeField] private LayerMask sceneryMask;
    [SerializeField] private float secondJumpTimerVariable;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        playerInput = new PlayerInputSystem();

        playerInput.Movement.Walk.started += OnWalkInput;
        playerInput.Movement.Walk.canceled += OnWalkInput;
        playerInput.Movement.Walk.performed += OnWalkInput;

        playerInput.Movement.Jump.started += OnJumpInput;
        playerInput.Movement.Jump.canceled += OnJumpInput;

        startNumberJumps = numberOfJumps;

        GetAnimatorParameters();
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
        characterController.Move(characterWalk * velocity * Time.deltaTime);
    }

    void SetJump()
    {
        isGrounded = Physics.CheckSphere(groundVerify.position, 0.3f, sceneryMask);

        if (numberOfJumps > 0)
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
            numberOfJumps = startNumberJumps;
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

    public void OnWalkInput(InputAction.CallbackContext context)
    {
        characterWalkInput = context.ReadValue<Vector2>();
        characterWalk = new Vector3(characterWalkInput.x, 0, characterWalkInput.y);

        isMoving = characterWalkInput.x != 0 || characterWalkInput.y != 0;
    }
    public void OnJumpInput(InputAction.CallbackContext context)
    {
        jumpPressed = context.ReadValueAsButton();
    }

    void GetAnimatorParameters()
    {
        a_isWalking = Animator.StringToHash("isWalking");
        a_isJumping = Animator.StringToHash("isJumping");
    }

    void AnimationHandler()
    {
        bool isWalkingAnimation = animator.GetBool(a_isWalking);
        bool isJumpingAnimation = animator.GetBool(a_isJumping);

        if (isMoving && !isWalkingAnimation)
        {
            animator.SetBool(a_isWalking, true);
        }
        else if (!isMoving && isWalkingAnimation)
        {
            animator.SetBool(a_isWalking, false);
        }

        if((isMoving && isWalkingAnimation) && verticalVelocity > 0 && !isJumpingAnimation)
        {
            animator.SetBool(a_isJumping, true);
        }
        else if((!isMoving && !isWalkingAnimation) && verticalVelocity > 0 && !isJumpingAnimation)
        {
            animator.SetBool(a_isJumping, true);
        }
        else if((isMoving && isWalkingAnimation) && verticalVelocity < 0 && isJumpingAnimation)
        {
            animator.SetBool(a_isJumping, false);
        }
    }
    private void OnEnable()
    {
        playerInput.Movement.Enable();
    }

    private void OnDisable()
    {
        playerInput.Movement.Disable();
    }
}
