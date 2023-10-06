using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private SpawnBehavior _spawnBehavior;
    [SerializeField] private PlayerAudio _playerAudio;

    #region Actions

    public static event Action<InputAction.CallbackContext, float> HandleMoveInput;
    public static event Action<bool, float> HandleJumpInput;
    public static event Action<bool> HandleAttackInput;
    public static event Action<bool> HandleUsingItemInput;
    public static event Action<bool> HandlePlayerDeath;

    #endregion

    #region Delegates

    public delegate CharacterController CharacterControllerReference();

    public delegate Vector3 PlayerPositionReference();

    public static CharacterControllerReference CharacterControllerRef;
    public static PlayerPositionReference PlayerPositionRef;

    #endregion

    #region Player Variables

    [SerializeField] private float _velocity;
    [SerializeField] private float _jumpHeight;
    [SerializeField] public int _lifes;

    #endregion

    private void Awake()
    {
        GameManager.OnMoveInputContextReceived += HandleMove;
        GameManager.OnJumpInputContextReceived += HandleJump;
        GameManager.OnAttackInputContextReceived += HandleAttack;
        GameManager.OnUsingItemInputContextReceived += HandleUsingItem;
        GameManager.OnPlayerDeathReceived += HandleDeath;
        _spawnBehavior.OnPlayerDeath += HandleDeath;
        GameManager.PlayerLifeRef = GetPlayerLifes;
    }
    
    private void HandleDeath(bool playerDeath)
    {
        if (_lifes == 0)
        {
            playerDeath = false;
        }

        if (playerDeath)
            _lifes--;
        HandlePlayerDeath?.Invoke(playerDeath);
    }

    private void HandleMove(InputAction.CallbackContext context)
    {
        HandleMoveInput?.Invoke(context, _velocity);
    }

    private void HandleJump(bool jumpPressed)
    {
        HandleJumpInput?.Invoke(jumpPressed, _jumpHeight);
    }

    private void HandleAttack(bool attackPressed)
    {
        HandleAttackInput?.Invoke(attackPressed);
    }

    private void HandleUsingItem(bool buttomPressed)
    {
        HandleUsingItemInput?.Invoke(buttomPressed);
    }

    private int GetPlayerLifes()
    {
        return _lifes;
    }


    private void OnDisable()
    {
        GameManager.OnMoveInputContextReceived -= HandleMove;
        GameManager.OnJumpInputContextReceived -= HandleJump;
        GameManager.OnAttackInputContextReceived -= HandleAttack;
        GameManager.OnUsingItemInputContextReceived -= HandleUsingItem;
        GameManager.OnPlayerDeathReceived -= HandleDeath;
        _spawnBehavior.OnPlayerDeath -= HandleDeath;
    }
}