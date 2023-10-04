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
    private Vector3 _cameraRelativeMovement;

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
        PlayerManager.PlayerPositionRef = GetPlayerPosition;
    }

    private void FixedUpdate()
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
        _cameraRelativeMovement = ConvertToCameraSpace(_characterMovement);
        characterController.Move(_cameraRelativeMovement * (_playerVelocity * Time.deltaTime));
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
        _positionToLookAt = new Vector3(_cameraRelativeMovement.x, 0, _cameraRelativeMovement.z);
        Quaternion currentRotation = transform.rotation;

        if (_isMoving)
        {
            Quaternion lookAtRotation = Quaternion.LookRotation(_positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, lookAtRotation, _rotationVelocity * Time.deltaTime);
        }
    }
    
    private Vector3 ConvertToCameraSpace(Vector3 vectorToRotate)
    {
        float _currentYValue = vectorToRotate.y;

        Vector3 _cameraForward = Camera.main.transform.forward;
        Vector3 _cameraRight = Camera.main.transform.right;

        _cameraForward.y = 0;
        _cameraRight.y = 0;

        Vector3 _cameraForwardZProduct = _cameraForward * vectorToRotate.z;
        Vector3 _cameraRightXProduct = _cameraRight * vectorToRotate.x;

        Vector3 _directionToMovePlayer = _cameraForwardZProduct + _cameraRightXProduct;
        _directionToMovePlayer.y = _currentYValue;

        return _directionToMovePlayer;
    }

    private CharacterController GetCharacterController()
    {
        return characterController;
    }

    private Vector3 GetPlayerPosition()
    {
        return transform.position;
    }

    private void OnDisable()
    {
        PlayerManager.HandleMoveInput -= SetMoveInfo;
        PlayerManager.HandleJumpInput -= SetCharacterJump;
    }
}
