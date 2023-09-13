using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    private PlayerInput _playerInput;
    private PlayerMovement _playerMovement;
    private PlayerAnimation _playerAnimation;

    private bool _jumpPressed;
    private bool _isGrounded;
    private bool _buttonEPressed;
    private bool _isMoving;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _characterVelocity;
    [SerializeField] private float _rotationVelocity;
    private bool _isSlashing;

    private void Awake()
    {
        #region Singleton
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        #endregion

        _playerInput = GetComponent<PlayerInput>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    public void SetCharacterMovementInput(Vector2 _characterMovementInput)
    {
        _playerMovement.CharacterMovementInput = _characterMovementInput;
    }
    public Vector2 GetCharacterMovementInput()
    {
        return _playerMovement.CharacterMovementInput;
    }
    public void SetIsMoving(bool _isMoving)
    {
        this._isMoving = _isMoving;
    }
    public bool GetIsMoving()
    {
        return _isMoving;
    }
    public void SetJumpPressed(bool _jumpPressed)
    {
        this._jumpPressed = _jumpPressed;
    }
    public bool GetJumpPressed()
    {
        return _jumpPressed;
    }
    public bool GetButtonEPressed()
    {
        return _buttonEPressed;
    }
    public void SetButtonEPressed(bool _buttonEPressed)
    {
        this._buttonEPressed = _buttonEPressed;
    }
    public bool GetIsSlashing()
    {
        return _isSlashing;
    }
    public void SetIsSlashing(bool _isSlashing)
    {
        this._isSlashing = _isSlashing;
    }
    public bool GetIsGrounded()
    {
        return _isGrounded;
    }
    public void SetIsGrounded(bool _isGrounded)
    {
        this._isGrounded = _isGrounded;
    }
    public float GetCharacterVelocity()
    {
        return _characterVelocity;
    }
    public float GetJumpHeight()
    {
        return _jumpHeight;
    }
    public float GetRotationVelocity()
    {
        return _rotationVelocity;
    }
}
