using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    #region Components

    [SerializeField] private InputManager _inputManager;
    [SerializeField] private PlayerManager _playerManager;
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private ObstacleManager _obstacleManager;
    #endregion

    #region Actions

    public static event Action<InputAction.CallbackContext> OnMoveInputContextReceived;
    public static event Action<bool> OnJumpInputContextReceived;
    public static event Action<bool> OnAttackInputContextReceived;
    public static event Action<bool> OnUsingItemInputContextReceived;
    public static event Action<bool> OnPlayerDeathReceived;
    public event Action<bool, bool> OnGameIsOver; 
    #endregion

    #region Delegates

    public delegate int PlayerLifeReference();

    public static PlayerLifeReference PlayerLifeRef;
    #endregion
    
    [SerializeField] public bool _gameIsOver;

    private bool _wasCompleted;
    private void Awake()
    {
        _inputManager.OnMove += OnMoveInputHandler;
        _inputManager.OnJump += OnJumpInputHandler;
        _inputManager.OnAttack += OnAttackInputHandler;
        _inputManager.OnUsingItem += OnUsingItemInputHandler;
        _obstacleManager.OnPlayerDeathHandler += OnPlayerDeathHandler;
        
        Cursor.lockState = CursorLockMode.Locked;
        _gameIsOver = false;
    }

    private void Update()
    {
        if (PlayerLifeRef() == 0)
        {
            _gameIsOver = true;
            _wasCompleted = false;
            OnGameIsOver?.Invoke(_gameIsOver, _wasCompleted);
        }
        else if (_enemyManager._enemyLife == 0)
        {
            _gameIsOver = true;
            _wasCompleted = true;
            OnGameIsOver?.Invoke(_gameIsOver, _wasCompleted);
        }
    }

    private void OnPlayerDeathHandler(bool playerDeath)
    {
        OnPlayerDeathReceived?.Invoke(playerDeath);
    }

    private void OnMoveInputHandler(InputAction.CallbackContext context)
    {
        OnMoveInputContextReceived?.Invoke(context);
    }

    private void OnJumpInputHandler(bool jumpPressed)
    {
        OnJumpInputContextReceived?.Invoke(jumpPressed);
    }

    private void OnAttackInputHandler(bool attackPressed)
    {
        OnAttackInputContextReceived?.Invoke(attackPressed);
    }

    private void OnUsingItemInputHandler(bool buttomPressed)
    {
        OnUsingItemInputContextReceived?.Invoke(buttomPressed);
    }

    private void OnDisable()
    {
        _inputManager.OnMove -= OnMoveInputHandler;
        _inputManager.OnJump -= OnJumpInputHandler;
        _inputManager.OnAttack -= OnAttackInputHandler;
        _inputManager.OnUsingItem -= OnUsingItemInputHandler;
        _obstacleManager.OnPlayerDeathHandler -= OnPlayerDeathHandler;
    }
}
    
