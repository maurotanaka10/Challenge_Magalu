using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController characterController;

    [Header("Movement Variables")]
    private Vector2 _characterMovementInput;
    private Vector3 _characterMovement;
    private Vector3 _positionToLookAt;

    [Header("Physics")]
    private const float _gravity = -9.81f;
    private float _gravityVelocity;

    private float _playerVelocity;
    private bool _isMoving;
    [SerializeField] private float _gravityMultiplier;
    [SerializeField] private float _rotationVelocity;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        PlayerManager.HandleMoveInput += SetMoveInfo;
        PlayerManager.HandleJumpInput += SetCharacterJump;
        PlayerManager.CharacterControllerRef = GetCharacterController;
    }

    private void Update()
    {
        SetCharacterMovement(_characterMovementInput);
        GravityHandler();
    }

    private void SetCharacterJump(bool jumpPressed, float jumpHeight)
    {
        if(!jumpPressed) return;
        if(!characterController.isGrounded) return;

        _gravityVelocity = jumpHeight;
    }

    private void SetMoveInfo(InputAction.CallbackContext context, float velocity)
    {
        _playerVelocity = velocity;
        _characterMovementInput = context.ReadValue<Vector2>();
        _isMoving = _characterMovementInput.x != 0 || _characterMovementInput.y != 0;
    }

    private void SetCharacterMovement(Vector2 _characterMovementInput)
    {
        _characterMovement = new Vector3(this._characterMovementInput.x, _gravityVelocity, _characterMovementInput.y);
        characterController.Move(_characterMovement * (_playerVelocity * Time.deltaTime));
        RotationHandler();
    }

    private void GravityHandler()
    {
        if (characterController.isGrounded && _gravityVelocity < 0f)
        {
            _gravityVelocity = -1f;
        }
        else
        {
            _gravityVelocity += _gravity * _gravityMultiplier * Time.deltaTime;
        }
        _characterMovement.y = _gravityVelocity;
    }
    
    private void RotationHandler()
    {
        _positionToLookAt = new Vector3(_characterMovement.x, 0, _characterMovement.z);
        Quaternion currentRotation = transform.rotation;

        if (_isMoving)
        {
            Quaternion lookAtRotation = Quaternion.LookRotation(_positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, lookAtRotation, _rotationVelocity * Time.deltaTime);
        }
    }

    private CharacterController GetCharacterController()
    {
        return characterController;
    }

    private void OnDisable()
    {
        PlayerManager.HandleMoveInput -= SetMoveInfo;
        PlayerManager.HandleJumpInput -= SetCharacterJump;
    }
}
