using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DogController : MonoBehaviour
{
    //Components
    private CharacterController characterController;
    private Animator animator;
    private PlayerInputSystem playerInputSystem;

    //Animator Parameters
    private int a_dogIsWalking;

    [Header("Movement Variables")]
    private Vector2 dogMovementInput;
    private Vector3 dogMovement;
    private bool dogIsMoving;
    private bool runPressed;
    [SerializeField] private float dogWalkVelocity;
    [SerializeField] private float dogRunVelocity;
    private Vector3 dogPositionToLookAt;
    [SerializeField] private float dogRotationVelocity;

    [Header("Physics")]
    [SerializeField] private float gravity = -9.81f;
    private float verticalVelocity;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        playerInputSystem = new PlayerInputSystem();

        GetAnimatorParameters();

        playerInputSystem.Player.Walk.started += OnMovementInput;
        playerInputSystem.Player.Walk.canceled += OnMovementInput;
        playerInputSystem.Player.Walk.performed += OnMovementInput;

        playerInputSystem.Player.Run.started += OnRunningInput;
        playerInputSystem.Player.Run.canceled += OnRunningInput;
    }

    void FixedUpdate()
    {
        SetMovement();
        RotationHandler();
        AnimationHandler();
    }

    void AnimationHandler()
    {
        bool dogIsWalkingAnimation = animator.GetBool(a_dogIsWalking);

        if(dogIsMoving && !dogIsWalkingAnimation)
        {
            animator.SetBool(a_dogIsWalking, true);
        }
        else if(!dogIsMoving && dogIsWalkingAnimation)
        {
            animator.SetBool(a_dogIsWalking, false);
        }
    }

    void SetMovement()
    {
        characterController.Move(dogMovement * dogWalkVelocity * Time.deltaTime);

        verticalVelocity += gravity * Time.deltaTime;

        characterController.Move(new Vector3(0, verticalVelocity, 0) * Time.deltaTime);

        if (runPressed)
        {
            characterController.Move(dogMovement * dogRunVelocity * Time.deltaTime);
        }
    }

    void RotationHandler()
    {
        dogPositionToLookAt = new Vector3(dogMovement.x, dogMovement.y, dogMovement.z);
        Quaternion currentRotation = transform.rotation;

        if (dogIsMoving)
        {
            Quaternion lookAtRotation = Quaternion.LookRotation(dogPositionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, lookAtRotation, dogRotationVelocity * Time.deltaTime);
        }
    }

    void GetAnimatorParameters()
    {
        a_dogIsWalking = Animator.StringToHash("dogIsWalking");
    }

    void OnMovementInput(InputAction.CallbackContext context)
    {
        dogMovementInput = context.ReadValue<Vector2>();
        dogMovement = new Vector3(dogMovementInput.x, 0f, dogMovementInput.y);
        dogMovement = dogMovement.normalized;

        dogIsMoving = dogMovementInput.x != 0 || dogMovementInput.y != 0;
    }

    void OnRunningInput(InputAction.CallbackContext context)
    {
        runPressed = context.ReadValueAsButton();
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
