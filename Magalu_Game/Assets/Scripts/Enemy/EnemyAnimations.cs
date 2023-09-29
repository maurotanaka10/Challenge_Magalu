using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimations : MonoBehaviour
{
    private Animator _animator;

    private int _isIdleHash;
    private int _isPatrollingHash;
    private int _isChasingHash;
    private int _isAttackingHash;
    private int _isComboAttackingHash;
    private int _isDieHash;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        GetEnemyAnimatorParameters();

        EnemyManager.OnIdleStateReceived += IdleAnimationHandler;
        EnemyManager.OnPatrolStateReceived += PatrolAnimationHandler;
        EnemyManager.OnChaseStateReceived += ChaseAnimationHandler;
        EnemyManager.OnAttackStateReceived += AttackAnimationHandler;
        EnemyManager.OnComboAttackStateReceived += ComboAttackAnimationHandler;
        EnemyManager.OnDieStateReceived += DieAnimationHandler;
    }

    private void IdleAnimationHandler(EStatesEnemy currentState)
    {
        _animator.SetTrigger(_isIdleHash);
    }

    private void PatrolAnimationHandler(EStatesEnemy currentState)
    {
        _animator.SetTrigger(_isPatrollingHash);
    }

    private void ChaseAnimationHandler(EStatesEnemy currentState)
    {
        _animator.SetTrigger(_isChasingHash);
    }

    private void AttackAnimationHandler(EStatesEnemy currentState)
    {
        _animator.SetTrigger(_isAttackingHash);
    }

    private void ComboAttackAnimationHandler(EStatesEnemy currentState)
    {
        _animator.SetTrigger(_isComboAttackingHash);
    }

    private void DieAnimationHandler(EStatesEnemy currentState)
    {
        _animator.SetTrigger(_isDieHash);
    }

    private void GetEnemyAnimatorParameters()
    {
        _isIdleHash = Animator.StringToHash("isIdle");
        _isPatrollingHash = Animator.StringToHash("isPatrolling");
        _isChasingHash = Animator.StringToHash("isChasing");
        _isAttackingHash = Animator.StringToHash("isAttacking");
        _isComboAttackingHash = Animator.StringToHash("isComboAttacking");
        _isDieHash = Animator.StringToHash("isDeath");
    }

    private void OnDisable()
    {
        EnemyManager.OnIdleStateReceived -= IdleAnimationHandler;
        EnemyManager.OnPatrolStateReceived -= PatrolAnimationHandler;
        EnemyManager.OnChaseStateReceived -= ChaseAnimationHandler;
        EnemyManager.OnAttackStateReceived -= AttackAnimationHandler;
        EnemyManager.OnComboAttackStateReceived -= ComboAttackAnimationHandler;
        EnemyManager.OnDieStateReceived -= DieAnimationHandler;
    }
}
