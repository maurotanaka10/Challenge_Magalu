using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    #region Components

    [SerializeField] private InputManager _inputManager;
    [SerializeField] private PlayerManager _playerManager;
    [SerializeField] private ObstacleManager _obstacleManager;
    #endregion

    #region Actions

    public static event Action<InputAction.CallbackContext> OnMoveInputContextReceived;
    public static event Action<bool> OnJumpInputContextReceived;
    public static event Action<bool> OnAttackInputContextReceived;
    public static event Action<bool> OnUsingItemInputContextReceived;
    public static event Action<bool> OnPlayerDeathReceived;
    public static event Action<bool> OnStageCompleteReceived;
    public event Action<bool> OnGameIsOver; 
    #endregion

    #region Delegates

    public delegate int PlayerLifeReference();

    public static PlayerLifeReference PlayerLifeRef;
    #endregion

    [SerializeField] private int _stageComplete;
    [SerializeField] private bool _gameIsOver;
    private void Awake()
    {
        _inputManager.OnMove += OnMoveInputHandler;
        _inputManager.OnJump += OnJumpInputHandler;
        _inputManager.OnAttack += OnAttackInputHandler;
        _inputManager.OnUsingItem += OnUsingItemInputHandler;
        _obstacleManager.OnPlayerDeathHandler += OnPlayerDeathHandler;
        _playerManager.HandleStageComplete += OnStageCompleteHandler;
        
        Cursor.lockState = CursorLockMode.Locked;
        _gameIsOver = false;
    }

    private void Update()
    {
        if (PlayerLifeRef() == 0)
        {
            _gameIsOver = true;
            OnGameIsOver?.Invoke(_gameIsOver);
        }
    }

    private void OnStageCompleteHandler(bool stageComplete)
    {
        if (stageComplete) _stageComplete++;

        if (_stageComplete == 3)
        {
            _gameIsOver = true;
            OnGameIsOver?.Invoke(_gameIsOver);
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
    
