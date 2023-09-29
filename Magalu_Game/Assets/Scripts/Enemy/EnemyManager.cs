using System;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private EnemyController _enemyController;
    
    public static event Action<EStatesEnemy> OnIdleStateReceived;
    public static event Action<EStatesEnemy> OnPatrolStateReceived;
    public static event Action<EStatesEnemy> OnChaseStateReceived;
    public static event Action<EStatesEnemy> OnAttackStateReceived;
    public static event Action<EStatesEnemy> OnComboAttackStateReceived;
    public static event Action<EStatesEnemy> OnDieStateReceived;

    [SerializeField] private float _enemyVelocity;
    private void Awake()
    {
        EnemyController.EnemyVelocity = GetEnemyVelocity;
        _enemyController.OnIdleState += OnIdleStateHandler;
        _enemyController.OnPatrolState += OnPatrolStateHandler;
        _enemyController.OnChaseState += OnChaseStateHandler;
        _enemyController.OnAttackState += OnAttackStateHandler;
        _enemyController.OnComboAttackState += OnComboAttackStateHandler;
        _enemyController.OnDieState += OnDieStateHandler;
    }

    private void OnIdleStateHandler(EStatesEnemy currentState)
    {
        OnIdleStateReceived?.Invoke(currentState);
    }

    private void OnPatrolStateHandler(EStatesEnemy currentState)
    {
        OnPatrolStateReceived?.Invoke(currentState);
    }

    private void OnChaseStateHandler(EStatesEnemy currentState)
    {
        OnChaseStateReceived?.Invoke(currentState);
    }

    private void OnAttackStateHandler(EStatesEnemy currentState)
    {
        OnAttackStateReceived?.Invoke(currentState);
    }

    private void OnComboAttackStateHandler(EStatesEnemy currentState)
    {
        OnComboAttackStateReceived?.Invoke(currentState);
    }

    private void OnDieStateHandler(EStatesEnemy currentState)
    {
        OnDieStateReceived?.Invoke(currentState);
    }

    private float GetEnemyVelocity()
    {
        return _enemyVelocity;
    }

    private void OnDisable()
    {
        _enemyController.OnIdleState -= OnIdleStateHandler;
        _enemyController.OnPatrolState -= OnPatrolStateHandler;
        _enemyController.OnChaseState -= OnChaseStateHandler;
        _enemyController.OnAttackState -= OnAttackStateHandler;
        _enemyController.OnComboAttackState -= OnComboAttackStateHandler;
        _enemyController.OnDieState -= OnDieStateHandler;
    }
}
