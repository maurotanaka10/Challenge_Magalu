using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DogController : MonoBehaviour
{
    //Components
    private Rigidbody rigidBody;
    private Animator animator;
    private PlayerInputSystem playerInputSystem;

    [Header("Movement Variables")]
    private Vector2 dogMovementInput;
    private Vector3 dogMovement;
    private bool dogIsMoving;
    [SerializeField] private float dogWalkVelocity;
    private Vector3 dogPositionToLookAt;
    [SerializeField] private float dogRotationVelocity;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerInputSystem = new PlayerInputSystem();

        playerInputSystem.Movement.Walk.started += OnMovementInput;
        playerInputSystem.Movement.Walk.canceled += OnMovementInput;
        playerInputSystem.Movement.Walk.performed += OnMovementInput;
    }

    void FixedUpdate()
    {
        SetMovement();
        RotationHandler();
    }

    void SetMovement()
    {
        rigidBody.MovePosition(transform.position + dogMovement * dogWalkVelocity * Time.deltaTime);
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

    void OnMovementInput(InputAction.CallbackContext context)
    {
        dogMovementInput = context.ReadValue<Vector2>();
        dogMovement = new Vector3(dogMovementInput.x, 0f, dogMovementInput.y);
        dogMovement = dogMovement.normalized;

        dogIsMoving = dogMovementInput.x != 0 || dogMovementInput.y != 0;
    }

    private void OnEnable()
    {
        playerInputSystem.Movement.Enable();
    }
    private void OnDisable()
    {
        playerInputSystem.Movement.Disable();
    }
}
