using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dog : MonoBehaviour
{
    Rigidbody rigidBody;
    Animator animator;
    PlayerInputSystem playerInput;

    //Movimentacao do personagem
    private Vector2 dogMovementInput;
    private Vector3 dogMovement;
    private Vector3 d_positionToLookAt;
    private bool d_isMoving;
    private bool d_isWalking;
    private float d_rotationVelocity = 10f;
    private float velocityMovement;

    //Parametros do Animator
    private int a_dogIsWalking;
    private int a_dogIsRunning;

    [Header("Movement Variable")]
    [SerializeField] private float d_velocityRun;
    [SerializeField] private float d_velocityWalk;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerInput = new PlayerInputSystem();

        playerInput.Movement.Walk.started += OnMovementInput;
        playerInput.Movement.Walk.canceled += OnMovementInput;
        playerInput.Movement.Walk.performed += OnMovementInput;

        playerInput.Movement.WalkSlowly.started += OnWalkingInput;
        playerInput.Movement.WalkSlowly.canceled += OnWalkingInput;

        GetAnimatorParameters();
    }

    void Update()
    {
        AnimationHandler();
    }

    private void FixedUpdate()
    {
        SetDogMovement();
        RotationHandler();
    }

    void GetAnimatorParameters()
    {
        a_dogIsRunning = Animator.StringToHash("dogIsMoving");
        a_dogIsWalking = Animator.StringToHash("dogIsWalking");
    }

    void AnimationHandler()
    {
        bool dogIsMovingAnimation = animator.GetBool(a_dogIsRunning);
        bool dogIsWalkingAnimation = animator.GetBool(a_dogIsWalking);

        if(d_isMoving && !dogIsMovingAnimation)
        {
            animator.SetBool(a_dogIsRunning, true);
        }
        else if(!d_isMoving && dogIsMovingAnimation)
        {
            animator.SetBool(a_dogIsRunning, false);
        }

        if(d_isMoving && d_isWalking && !dogIsWalkingAnimation)
        {
            animator.SetBool(a_dogIsWalking, true);
        }
        else if(!d_isMoving || !d_isWalking && dogIsWalkingAnimation)
        {
            animator.SetBool(a_dogIsWalking, false);
        }
    }

    void SetDogMovement()
    {
        Vector3 dog_movement = dogMovement * velocityMovement * Time.fixedDeltaTime;

        rigidBody.MovePosition(dog_movement + transform.position);

        if (d_isMoving && !d_isWalking)
        {
            velocityMovement = d_velocityRun;
        }
        else if(d_isMoving && d_isWalking)
        {
            velocityMovement = d_velocityWalk;
        }
        else if (!d_isMoving)
        {
            velocityMovement = 0;
        }
    }

    void RotationHandler()
    {
        d_positionToLookAt = new Vector3(dogMovement.x, dogMovement.y, dogMovement.z);
        Quaternion currentRotation = transform.rotation;

        if (d_isMoving)
        {
            Quaternion lookAtRotation = Quaternion.LookRotation(d_positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, lookAtRotation, d_rotationVelocity * Time.deltaTime);
        }
    }

    void OnMovementInput(InputAction.CallbackContext context)
    {
        dogMovementInput = context.ReadValue<Vector2>();
        dogMovement = new Vector3(dogMovementInput.x, 0f, dogMovementInput.y);
        dogMovement = dogMovement.normalized;

        d_isMoving = dogMovementInput.x != 0 || dogMovementInput.y != 0;
    }

    void OnWalkingInput(InputAction.CallbackContext context)
    {
        d_isWalking = context.ReadValueAsButton();
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
