using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimation : MonoBehaviour
{
    private Animator _animator;
    private CharacterController _characterControllerRef;

    private int _velocityHash;
    private int _isJumpingHash;
    private int _isSlashingHash;

    private bool _isJumping;
    private bool _isAttacking;
    private float _currentVelocity;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        GetAnimatorParameters();
        
        PlayerManager.HandleJumpInput += JumpAnimation;
        PlayerManager.HandleAttackInput += AttackAnimation;
    }

    private void Update()
    {
        _characterControllerRef = PlayerManager.CharacterControllerRef?.Invoke();
        MoveAnimation();
    }

    private void MoveAnimation()
    {
        _currentVelocity = _characterControllerRef.velocity.magnitude;
        _animator.SetFloat(_velocityHash, _currentVelocity);
    }

    private void JumpAnimation(bool jumpPressed, float jumpPower)
    {
        this._isJumping = jumpPressed;
        bool isJumpingAnimation = _animator.GetBool(_isJumpingHash);

        if (_isJumping && !isJumpingAnimation && _characterControllerRef.isGrounded)
        {
            _animator.SetBool(_isJumpingHash, true);
        }
        else if (isJumpingAnimation || _characterControllerRef.isGrounded)
        {
            _animator.SetBool(_isJumpingHash, false);
        }
    }

    private void AttackAnimation(bool attackPressed)
    {
        this._isAttacking = attackPressed;
        bool isAttackingAnimation = _animator.GetBool(_isSlashingHash);

        if (_isAttacking && !isAttackingAnimation)
        {
            _animator.SetBool(_isSlashingHash, true);
        }
        else if (isAttackingAnimation && !this._isAttacking)
        {
            _animator.SetBool(_isSlashingHash, false);
        }
    }

    private void GetAnimatorParameters()
    {
        _velocityHash = Animator.StringToHash("velocity");
        _isJumpingHash = Animator.StringToHash("isJumping");
        _isSlashingHash = Animator.StringToHash("isSlashing");
    }
}
