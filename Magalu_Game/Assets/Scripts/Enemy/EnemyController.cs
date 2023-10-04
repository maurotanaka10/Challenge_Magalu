using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ProBuilder.MeshOperations;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    #region Components

    private NavMeshAgent _navMeshAgent;
    private EStatesEnemy _currentState;

    #endregion

    #region Actions

    public event Action<EStatesEnemy> OnIdleState;
    public event Action<EStatesEnemy> OnPatrolState;
    public event Action<EStatesEnemy> OnChaseState;
    public event Action<EStatesEnemy> OnAttackState;

    public event Action<EStatesEnemy> OnComboAttackState;

    //public event Action<EStatesEnemy> OnHitState;
    public event Action<EStatesEnemy> OnDieState;

    #endregion

    #region Delegates

    public delegate float EnemyVelocityReference();

    public delegate int EnemyLifeReference();

    public static EnemyVelocityReference EnemyVelocity;
    public static EnemyLifeReference EnemyLife;

    #endregion

    [SerializeField] private float _timeInIdle;
    [SerializeField] private float _visionRange;
    [SerializeField] private float _attackRange;
    [SerializeField] private Vector2 _minPosition;
    [SerializeField] private Vector2 _maxPosition;

    private float _distanceFromPlayer;
    private Vector3 _playerPosition;
    private bool _isIdleDelaying = false;
    private bool _isWalking = false;
    private float _delayTimeInIdle;
    private Vector3 _movePosition;
    private int _countAttack;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _currentState = EStatesEnemy.Idle;
        _navMeshAgent.speed = EnemyVelocity();
        _countAttack = 0;
    }

    private void Update()
    {
        StatesController();
        print($"O boss esta no estagio {_currentState}");

        _distanceFromPlayer = 0;
        _playerPosition = Vector3.zero;
        if (PlayerManager.PlayerPositionRef != null)
        {
            _playerPosition = PlayerManager.PlayerPositionRef();
            _distanceFromPlayer = Vector3.Distance(_playerPosition, transform.position);
        }
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
        _isIdleDelaying = true;
        _navMeshAgent.isStopped = true;
        if (_isIdleDelaying)
        {
            _delayTimeInIdle += Time.deltaTime;
            if (_delayTimeInIdle >= _timeInIdle)
            {
                _isIdleDelaying = false;
            }
        }

        if (!_isIdleDelaying)
        {
            _delayTimeInIdle = 0f;
            if (_distanceFromPlayer <= _visionRange)
            {
                _currentState = EStatesEnemy.Chase;
            }
            else if (_distanceFromPlayer > _visionRange)
            {
                _currentState = EStatesEnemy.Patrol;
                _isWalking = false;
            }
        }
    }

    private void PatrolHandler()
    {
        OnPatrolState?.Invoke(_currentState);
        _navMeshAgent.isStopped = false;
        if (!_isWalking)
        {
            _navMeshAgent.SetDestination(SetBossPosition());
            _isWalking = true;
        }

        if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            _currentState = EStatesEnemy.Idle;
        }
        else if (_distanceFromPlayer <= _visionRange)
            _currentState = EStatesEnemy.Chase;
    }

    private void ChaseHandler()
    {
        OnChaseState?.Invoke(_currentState);
        _navMeshAgent.SetDestination(_playerPosition);
        _navMeshAgent.isStopped = false;

        if (_distanceFromPlayer > _visionRange)
        {
            _currentState = EStatesEnemy.Idle;
        }
        else if (_distanceFromPlayer <= _attackRange && _countAttack/3 != 0)
        {
            _currentState = EStatesEnemy.Attack;
        }
        else if (_distanceFromPlayer <= _attackRange && _countAttack / 3 == 0)
        {
            _currentState = EStatesEnemy.ComboAttack;
        }
        else if (EnemyLife() == 0)
        {
            _currentState = EStatesEnemy.Die;
        }
    }

    private void AttackHandler()
    {
        OnAttackState?.Invoke(_currentState);
        _countAttack++;
        _navMeshAgent.isStopped = true;

        if (_distanceFromPlayer >= _attackRange)
        {
            _currentState = EStatesEnemy.Chase;
        }
        else if (EnemyLife() == 0)
        {
            _currentState = EStatesEnemy.Die;
        }
    }

    private void ComboAttackHandler()
    {
        OnComboAttackState?.Invoke(_currentState);
        _navMeshAgent.isStopped = true;
        _countAttack++;

        if (_distanceFromPlayer >= _attackRange)
        {
            _currentState = EStatesEnemy.Chase;
        }
        else if (EnemyLife() == 0)
        {
            _currentState = EStatesEnemy.Die;
        }
    }

    private void DieHandler()
    {
        OnDieState?.Invoke(_currentState);
        _navMeshAgent.isStopped = true;
        _navMeshAgent.speed = 0f;
    }

    private Vector3 SetBossPosition()
    {
        _movePosition = new Vector3(Random.Range(_minPosition.x, _maxPosition.x), transform.position.y,
            Random.Range(_minPosition.y, _maxPosition.y));

        return _movePosition;
    }
}