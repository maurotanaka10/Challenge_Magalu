using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    #region Components

    [SerializeField] private PlayerManager _playerManager;
    private NavMeshAgent _navMeshAgent;
    private Rigidbody _rigidbody;
    private EStatesEnemy _currentState;
    private EStatesEnemy _previousState;

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
    [SerializeField] private float _attackSpace;
    [SerializeField] private Vector2 _minPosition;
    [SerializeField] private Vector2 _maxPosition;
    [SerializeField] private Transform _hit;
    [SerializeField] private LayerMask _playerMask;

    private float _distanceFromPlayer;
    private Vector3 _playerPosition;
    private bool _isIdleDelaying = false;
    private bool _isWalking = false;
    private float _delayTimeInIdle;
    private Vector3 _movePosition;
    private bool _hasUsedComboAttack = false;
    public bool _canBeHit;

    private Dictionary<EStatesEnemy, Action> _statesMethods = new Dictionary<EStatesEnemy, Action>();

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _rigidbody = GetComponent<Rigidbody>();
        _currentState = EStatesEnemy.Idle;
        _navMeshAgent.speed = EnemyVelocity();

        _statesMethods[EStatesEnemy.Idle] = IdleHandler;
        _statesMethods[EStatesEnemy.Patrol] = PatrolHandler;
        _statesMethods[EStatesEnemy.Chase] = ChaseHandler;
        _statesMethods[EStatesEnemy.Attack] = AttackHandler;
        _statesMethods[EStatesEnemy.ComboAttack] = ComboAttackHandler;
        _statesMethods[EStatesEnemy.Die] = DieHandler;

        ChangeState(EStatesEnemy.Idle);
    }

    private void Update()
    {
        _distanceFromPlayer = 0;
        _playerPosition = Vector3.zero;
        if (PlayerManager.PlayerPositionRef != null)
        {
            _playerPosition = PlayerManager.PlayerPositionRef();
            _distanceFromPlayer = Vector3.Distance(_playerPosition, transform.position);
        }

        _statesMethods[_currentState].Invoke();
        CheckStateTransitions();
        
        print($"O inimigo esta em {_currentState}");
    }

    private void CheckStateTransitions()
    {
        switch (_currentState)
        {
            case EStatesEnemy.Idle:
                if (!_isIdleDelaying && _distanceFromPlayer <= _visionRange)
                    ChangeState(EStatesEnemy.Chase);
                else if (!_isIdleDelaying && _distanceFromPlayer > _visionRange)
                    ChangeState(EStatesEnemy.Patrol);
                break;
            case EStatesEnemy.Patrol:
                if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
                    ChangeState(EStatesEnemy.Idle);
                else if (_distanceFromPlayer <= _visionRange)
                    ChangeState(EStatesEnemy.Chase);
                break;
            case EStatesEnemy.Chase:
                if (_distanceFromPlayer > _visionRange)
                    ChangeState(EStatesEnemy.Idle);
                else if (_distanceFromPlayer <= _attackRange)
                    ChangeState(EStatesEnemy.Attack);
                else if (_distanceFromPlayer <= _attackRange && EnemyLife() == 1 && !_hasUsedComboAttack)
                    ChangeState(EStatesEnemy.ComboAttack);
                else if (EnemyLife() <= 0)
                    ChangeState(EStatesEnemy.Die);
                break;
            case EStatesEnemy.Attack:
                if (_distanceFromPlayer >= _attackRange)
                    ChangeState(EStatesEnemy.Chase);
                else if(_distanceFromPlayer <= _attackRange)
                    ChangeState(EStatesEnemy.Idle);
                else if (EnemyLife() <= 0)
                    ChangeState(EStatesEnemy.Die);
                break;
            case EStatesEnemy.ComboAttack:
                if (_distanceFromPlayer >= _attackRange)
                    ChangeState(EStatesEnemy.Chase);
                else if(_distanceFromPlayer <= _attackRange)
                    ChangeState(EStatesEnemy.Idle);
                else if (EnemyLife() <= 0)
                    ChangeState(EStatesEnemy.Die);
                break;
        }
    }

    private void ChangeState(EStatesEnemy newState)
    {
        _previousState = _currentState;
        _currentState = newState;
    }


    private void IdleHandler()
    {
        OnIdleState?.Invoke(_currentState);
        _navMeshAgent.isStopped = true;
        _isWalking = false;
        if (_isIdleDelaying)
        {
            _delayTimeInIdle += Time.deltaTime;
            if (_delayTimeInIdle >= _timeInIdle)
            {
                _isIdleDelaying = false;
            }
        }
    }

    private void PatrolHandler()
    {
        OnPatrolState?.Invoke(_currentState);
        _navMeshAgent.isStopped = false;
        _isIdleDelaying = true;
        if (!_isWalking)
        {
            _navMeshAgent.SetDestination(SetBossPosition());
            _isWalking = true;
        }
    }

    private void ChaseHandler()
    {
        OnChaseState?.Invoke(_currentState);
        _navMeshAgent.SetDestination(_playerPosition);
        _navMeshAgent.isStopped = false;
        _isIdleDelaying = true;
    }

    private void AttackHandler()
    {
        OnAttackState?.Invoke(_currentState);
        _navMeshAgent.isStopped = true;
        _isIdleDelaying = true;
    }

    private void ComboAttackHandler()
    {
        OnComboAttackState?.Invoke(_currentState);
        _navMeshAgent.isStopped = true;
        _hasUsedComboAttack = true;
        _isIdleDelaying = true;
    }

    private void DieHandler()
    {
        OnDieState?.Invoke(_currentState);
        _navMeshAgent.isStopped = true;
        _rigidbody.constraints |= RigidbodyConstraints.FreezeRotationY;
        _navMeshAgent.speed = 0f;
    }

    private void EnemyAttackCollider()
    {
        Collider[] hitPlayer = Physics.OverlapSphere(_hit.position, _attackSpace, _playerMask);
        foreach (Collider hit in hitPlayer)
        {
            _playerManager._lifes--;
            print($"acertou o jogador");
        }
    }

    private void OnDrawGizmos()
    {
        if (_hit == null) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(_hit.position, _attackSpace);
    }

    private Vector3 SetBossPosition()
    {
        _movePosition = new Vector3(Random.Range(_minPosition.x, _maxPosition.x), transform.position.y,
            Random.Range(_minPosition.y, _maxPosition.y));

        return _movePosition;
    }
}