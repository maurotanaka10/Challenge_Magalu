using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private EStatesEnemy _currentState;

    public event Action<EStatesEnemy> OnIdleState;
    public event Action<EStatesEnemy> OnPatrolState;
    public event Action<EStatesEnemy> OnChaseState;
    public event Action<EStatesEnemy> OnAttackState;
    public event Action<EStatesEnemy> OnComboAttackState;
    //public event Action<EStatesEnemy> OnHitState;
    public event Action<EStatesEnemy> OnDieState;
    
    public delegate float EnemyVelocityReference();

    public static EnemyVelocityReference EnemyVelocity;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _currentState = EStatesEnemy.Patrol;
        //_navMeshAgent.speed = EnemyVelocity();
    }

    private void Update()
    {
        StatesController();
    }

    private void StatesController()
    {
        switch (_currentState)
        {
            case EStatesEnemy.Idle:
                IdleHandler();
                break;
            case EStatesEnemy.Patrol:
                PatrolHandler();
                break;
            case EStatesEnemy.Chase:
                ChaseHandler();
                break;
            case EStatesEnemy.Attack:
                AttackHandler();
                break;
            case EStatesEnemy.ComboAttack:
                ComboAttackHandler();
                break;
            case EStatesEnemy.Die:
                DieHandler();
                break;
        }
    }

    private void IdleHandler()
    {
        OnIdleState?.Invoke(_currentState);
    }

    private void PatrolHandler()
    {
        OnPatrolState?.Invoke(_currentState);
    }

    private void ChaseHandler()
    {
        OnChaseState?.Invoke(_currentState);
    }

    private void AttackHandler()
    {
        OnAttackState?.Invoke(_currentState);
    }

    private void ComboAttackHandler()
    {
        OnComboAttackState?.Invoke(_currentState);
    }

    private void DieHandler()
    {
        OnDieState?.Invoke(_currentState);
    }
}
