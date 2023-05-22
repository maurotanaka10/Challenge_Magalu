using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    //Components
    private CharacterController characterController;
    private Animator animator;
    private PlayerInputSystem playerInputSystem;

    //Animator Parameters
    private int a_isRunning;
    private int a_isJumping;
    private int a_isGuarding;
    private int a_isSlashing;

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

    [Header("Cinemachine")]
    [SerializeField] private CinemachineVirtualCamera playerCamera;

    [Header("Change Characters")]
    [SerializeField] private GameObject ninja;
    [SerializeField] private GameObject billy;

    [SerializeField] private bool changePlayer;
    [SerializeField] private bool ninjaIsPlaying;
    [SerializeField] private bool billyIsPlaying;

    [Header("Attack Variables")]
    [SerializeField] private GameObject sword;
    private bool playerIsInArena;
    private bool withSwordInHand = false;
    private bool isSlashing;

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

        playerInputSystem.ActionExtras.ChangePlayer.started += OnChangePlayerInput;
        playerInputSystem.ActionExtras.ChangePlayer.canceled += OnChangePlayerInput;

        playerInputSystem.Ninja.Attack.started += OnAttackingInput;
        playerInputSystem.Ninja.Attack.canceled += OnAttackingInput;
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
        a_isGuarding = Animator.StringToHash("isGuarding");
        a_isSlashing = Animator.StringToHash("isSlashing");
    }

    void AnimationHandler()
    {
        bool isRunningAnimation = animator.GetBool(a_isRunning);
        bool isJumpingAnimation = animator.GetBool(a_isJumping);
        bool isSlashingAnimation = animator.GetBool(a_isSlashing);

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

        if (playerIsInArena && !withSwordInHand)
        {
            animator.SetBool(a_isGuarding, true);
            withSwordInHand = true;
        }
        else if((playerIsInArena) && withSwordInHand)
        {
            animator.SetBool(a_isGuarding, false);
        }

        if(isSlashing && !isSlashingAnimation)
        {
            animator.SetBool(a_isSlashing, true);
        }
        else if(!isSlashing && isSlashingAnimation)
        {
            animator.SetBool(a_isSlashing, false);
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
        if (changePlayer && !billyIsPlaying)
        {
            billy.transform.position = ninja.transform.position;
            billy.SetActive(true);
            ninja.SetActive(false);
            billyIsPlaying = true;
            ninjaIsPlaying = false;
            playerCamera.Follow = billy.transform;
        }

        if (changePlayer && !ninjaIsPlaying)
        {
            ninja.transform.position = billy.transform.position;
            ninja.SetActive(true);
            billy.SetActive(false);
            ninjaIsPlaying = true;
            billyIsPlaying = false;
            playerCamera.Follow = ninja.transform;
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

    void OnChangePlayerInput(InputAction.CallbackContext context)
    {
        changePlayer = context.ReadValueAsButton();

        ChangeCharacters();
    }

    void OnAttackingInput(InputAction.CallbackContext context)
    {
        isSlashing = context.ReadValueAsButton();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.tag == "ArenaObstacle")
        {
            playerIsInArena = true;
            sword.SetActive(true);
        }
    }

    private void OnEnable()
    {
        playerInputSystem.Ninja.Enable();
        playerInputSystem.Billy.Enable();
        playerInputSystem.ActionExtras.Enable();
    }
    private void OnDisable()
    {
        playerInputSystem.Ninja.Disable();
        playerInputSystem.Billy.Disable();
        playerInputSystem.ActionExtras.Disable();
    }
}
