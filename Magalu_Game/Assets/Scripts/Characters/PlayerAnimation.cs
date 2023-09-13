using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator _animator;

    private int a_isRunning;
    private int a_isJumping;
    private int a_isGuarding;
    private int a_isSlashing;


    private void Awake()
    {
        _animator = GetComponent<Animator>();

        GetAnimatorParameters();
    }

    private void Update()
    {
        AnimationHandler();
    }

    private void GetAnimatorParameters()
    {
        a_isRunning = Animator.StringToHash("isRunning");
        a_isJumping = Animator.StringToHash("isJumping");
        a_isGuarding = Animator.StringToHash("isGuarding");
        a_isSlashing = Animator.StringToHash("isSlashing");
    }

    void AnimationHandler()
    {
        bool isRunningAnimation = _animator.GetBool(a_isRunning);
        bool isJumpingAnimation = _animator.GetBool(a_isJumping);
        bool isSlashingAnimation = _animator.GetBool(a_isSlashing);

        if (PlayerManager.Instance.GetIsMoving() && !isRunningAnimation)
        {
            _animator.SetBool(a_isRunning, true);
        }
        else if (!PlayerManager.Instance.GetIsMoving() && isRunningAnimation)
        {
            _animator.SetBool(a_isRunning, false);
        }

        if (PlayerManager.Instance.GetJumpPressed() && !isJumpingAnimation)
        {
            _animator.SetBool(a_isJumping, true);
        }
        else if (!PlayerManager.Instance.GetJumpPressed() && isJumpingAnimation)
        {
            _animator.SetBool(a_isJumping, false);
        }
        else if (PlayerManager.Instance.GetJumpPressed() && isJumpingAnimation)
        {
            _animator.SetBool(a_isJumping, false);
        }

        /*if (playerIsInArena && !withSwordInHand)
        {
            _animator.SetBool(a_isGuarding, true);
            withSwordInHand = true;
        }
        else if ((playerIsInArena) && withSwordInHand)
        {
            _animator.SetBool(a_isGuarding, false);
        }*/

        if (PlayerManager.Instance.GetIsSlashing() && !isSlashingAnimation)
        {
            _animator.SetBool(a_isSlashing, true);
        }
        else if (!PlayerManager.Instance.GetIsSlashing() && isSlashingAnimation)
        {
            _animator.SetBool(a_isSlashing, false);
        }
    }
}
