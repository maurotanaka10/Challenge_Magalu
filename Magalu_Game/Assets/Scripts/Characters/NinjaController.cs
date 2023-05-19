using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NinjaController : MonoBehaviour
{
    //Components
    private CharacterController characterController;
    private Animator animator;
    private PlayerInputSystem playerInputSystem;

    //Animator Parameters
    private int a_isRunning;
    private int a_isJumping;

    [Header("Movement Variables")]
    private Vector2 ninjaMovementInput;
    private Vector3 ninjaMovement;
    private bool isMoving;
    private Vector3 positionToLookAt;
    [SerializeField] private float ninjaVelocity;
    [SerializeField] private float rotationVelocity;

    [Header("Jump Variables")]
    [SerializeField] private float jumpHeight;
    [SerializeField] private Transform groundVerify;
    [SerializeField] private LayerMask sceneryMask;
    private bool jumpPressed;
    private bool isGrounded;

    [Header("Physics")]
    [SerializeField] private float gravity = -9.81f;
    private float verticalVelocity;

    [Header("Using Item")]
    public bool buttonEPressed;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        playerInputSystem = new PlayerInputSystem();

        GetAnimatorParameters();

        playerInputSystem.Player.Walk.started += OnMovementInput;
        playerInputSystem.Player.Walk.canceled += OnMovementInput;
        playerInputSystem.Player.Walk.performed += OnMovementInput;

        playerInputSystem.Player.Jump.started += OnJumpInput;
        playerInputSystem.Player.Jump.canceled += OnJumpInput;

        playerInputSystem.Player.UseItem.started += OnUsingItemInput;
        playerInputSystem.Player.UseItem.canceled += OnUsingItemInput;
    }

    void Update()
    {
        SetMovement();
        RotationHandler();
        AnimationHandler();
    }

    void GetAnimatorParameters()
    {
        a_isRunning = Animator.StringToHash("isRunning");
        a_isJumping = Animator.StringToHash("isJumping");
    }

    void AnimationHandler()
    {
        bool isRunningAnimation = animator.GetBool(a_isRunning);
        bool isJumpingAnimation = animator.GetBool(a_isJumping);

        if (isMoving && !isRunningAnimation)
        {
            animator.SetBool(a_isRunning, true);
        }
        else if (!isMoving && isRunningAnimation)
        {
            animator.SetBool(a_isRunning, false);
        }

        if(jumpPressed && !isJumpingAnimation)
        {
            animator.SetBool(a_isJumping, true);
        }
        else if(!jumpPressed && isJumpingAnimation)
        {
            animator.SetBool(a_isJumping, false);
        }
        else if(jumpPressed && isJumpingAnimation)
        {
            animator.SetBool(a_isJumping, false);
        }
    }

    void SetMovement()
    {
        characterController.Move(ninjaMovement * ninjaVelocity * Time.deltaTime);

        verticalVelocity += gravity * Time.deltaTime;

        characterController.Move(new Vector3(0, verticalVelocity, 0) * Time.deltaTime);
    }

    void RotationHandler()
    {
        positionToLookAt = new Vector3(ninjaMovement.x, ninjaMovement.y, ninjaMovement.z);
        Quaternion currentRotation = transform.rotation;

        if (isMoving)
        {
            Quaternion lookAtRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, lookAtRotation, rotationVelocity * Time.deltaTime);
        }
    }

    void OnMovementInput(InputAction.CallbackContext context)
    {
        ninjaMovementInput = context.ReadValue<Vector2>();
        ninjaMovement = new Vector3(ninjaMovementInput.x, 0f, ninjaMovementInput.y);

        isMoving = ninjaMovementInput.x != 0 || ninjaMovementInput.y != 0;
    }

    void OnJumpInput(InputAction.CallbackContext context)
    {
        jumpPressed = context.ReadValueAsButton();

        isGrounded = Physics.CheckSphere(groundVerify.position, 0.3f, sceneryMask);

        if (jumpPressed && isGrounded)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -1f;
        }
    }

    void OnUsingItemInput(InputAction.CallbackContext context)
    {
        buttonEPressed = context.ReadValueAsButton();

        if(GameObject.Find("Button").GetComponent<ColorOrderBehavior>().canPressButton == true && buttonEPressed)
        {
            GameObject.Find("Button").GetComponent<ColorOrderBehavior>().ShowChallenge();
        }
    }

    private void OnEnable()
    {
        playerInputSystem.Player.Enable();
    }
    private void OnDisable()
    {
        playerInputSystem.Player.Disable();
    }
}
