using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //Components
    private CharacterController characterController;
    private Animator animator;
    private PlayerInputSystem playerInputSystem;

    //Animator Parameters
    private int a_isRunning;
    private int a_isJumping;

    [Header("Movement Variables")]
    private Vector2 characterMovementInput;
    private Vector3 characterMovement;
    private bool isMoving;
    private Vector3 positionToLookAt;
    [SerializeField] private float characterVelocity;
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

    [Header("Change Characters")]
    [SerializeField] private GameObject ninja;
    [SerializeField] private GameObject billy;

    [SerializeField] private bool ninjaChosen;
    [SerializeField] private bool ninjaIsPlaying;
    [SerializeField] private bool billyChosen;
    [SerializeField] private bool billyIsPlaying;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        playerInputSystem = new PlayerInputSystem();

        GetAnimatorParameters();

        playerInputSystem.Ninja.Run.started += OnMovementInput;
        playerInputSystem.Ninja.Run.canceled += OnMovementInput;
        playerInputSystem.Ninja.Run.performed += OnMovementInput;
        playerInputSystem.Billy.Walk.started += OnMovementInput;
        playerInputSystem.Billy.Walk.canceled += OnMovementInput;
        playerInputSystem.Billy.Walk.performed += OnMovementInput;

        playerInputSystem.Ninja.Jump.started += OnJumpInput;
        playerInputSystem.Ninja.Jump.canceled += OnJumpInput;
        playerInputSystem.Billy.Jump.started += OnJumpInput;
        playerInputSystem.Billy.Jump.canceled += OnJumpInput;

        playerInputSystem.Ninja.UseObjects.started += OnUsingItemInput;
        playerInputSystem.Ninja.UseObjects.canceled += OnUsingItemInput;
        playerInputSystem.Billy.UseObjects.started += OnUsingItemInput;
        playerInputSystem.Billy.UseObjects.canceled += OnUsingItemInput;

        playerInputSystem.Ninja.ChangeToBilly.started += OnChangeToBillyInput;
        playerInputSystem.Ninja.ChangeToBilly.canceled += OnChangeToBillyInput;
        playerInputSystem.Billy.ChangeToNinja.started += OnChangeToNinjaInput;
        playerInputSystem.Billy.ChangeToNinja.canceled += OnChangeToNinjaInput;
    }

    void Update()
    {
        SetMovement();
        RotationHandler();
        AnimationHandler();
        ChangeCharacters();
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

        if (jumpPressed && !isJumpingAnimation)
        {
            animator.SetBool(a_isJumping, true);
        }
        else if (!jumpPressed && isJumpingAnimation)
        {
            animator.SetBool(a_isJumping, false);
        }
        else if (jumpPressed && isJumpingAnimation)
        {
            animator.SetBool(a_isJumping, false);
        }
    }

    void SetMovement()
    {
        characterController.Move(characterMovement * characterVelocity * Time.deltaTime);

        verticalVelocity += gravity * Time.deltaTime;

        characterController.Move(new Vector3(0, verticalVelocity, 0) * Time.deltaTime);
    }

    void RotationHandler()
    {
        positionToLookAt = new Vector3(characterMovement.x, characterMovement.y, characterMovement.z);
        Quaternion currentRotation = transform.rotation;

        if (isMoving)
        {
            Quaternion lookAtRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, lookAtRotation, rotationVelocity * Time.deltaTime);
        }
    }

    void ChangeCharacters()
    {
        if (billyChosen && !billyIsPlaying)
        {
            ninja.SetActive(false);
            billy.SetActive(true);
            billy.transform.position = ninja.transform.position;
            billyIsPlaying = true;
            ninjaIsPlaying = false;
        }

        if (ninjaChosen && !ninjaIsPlaying)
        {

            billy.SetActive(false);
            ninja.SetActive(true);
            ninja.transform.position = billy.transform.position;
            ninjaIsPlaying = true;
            billyIsPlaying = false;
        }
    }

    void OnMovementInput(InputAction.CallbackContext context)
    {
        characterMovementInput = context.ReadValue<Vector2>();
        characterMovement = new Vector3(characterMovementInput.x, 0f, characterMovementInput.y);

        isMoving = characterMovementInput.x != 0 || characterMovementInput.y != 0;
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

        if (GameObject.Find("Button").GetComponent<ColorOrderBehavior>().canPressButton == true && buttonEPressed)
        {
            GameObject.Find("Button").GetComponent<ColorOrderBehavior>().ShowChallenge();
        }
    }

    void OnChangeToBillyInput(InputAction.CallbackContext context)
    {
        billyChosen = context.ReadValueAsButton();
    }

    void OnChangeToNinjaInput(InputAction.CallbackContext context)
    {
        ninjaChosen = context.ReadValueAsButton();
    }

    private void OnEnable()
    {
        playerInputSystem.Ninja.Enable();
        playerInputSystem.Billy.Enable();
    }
    private void OnDisable()
    {
        playerInputSystem.Ninja.Disable();
        playerInputSystem.Billy.Disable();
    }
}
