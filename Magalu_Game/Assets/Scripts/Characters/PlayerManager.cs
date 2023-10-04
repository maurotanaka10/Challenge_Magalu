using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private SpawnBehavior _spawnBehavior;

    #region Actions

    public static event Action<InputAction.CallbackContext, float> HandleMoveInput;
    public static event Action<bool, float> HandleJumpInput;
    public static event Action<bool> HandleAttackInput;
    public static event Action<bool> HandleUsingItemInput;
    public static event Action<bool> HandlePlayerDeath;
    public event Action<bool> HandleStageComplete;

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
    [SerializeField] private int _lifes;

    #endregion

    private void Awake()
    {
        GameManager.OnMoveInputContextReceived += HandleMove;
        GameManager.OnJumpInputContextReceived += HandleJump;
        GameManager.OnAttackInputContextReceived += HandleAttack;
        GameManager.OnUsingItemInputContextReceived += HandleUsingItem;
        GameManager.OnPlayerDeathReceived += HandleDeath;
        _spawnBehavior.OnStageComplete += HandleStages;
        _spawnBehavior.OnPlayerDeath += HandleDeath;
        GameManager.PlayerLifeRef = GetPlayerLifes;
    }

    private void HandleStages(bool stageComplete)
    {
        HandleStageComplete?.Invoke(stageComplete);
    }

    private void HandleDeath(bool playerDeath)
    {
        HandlePlayerDeath?.Invoke(playerDeath);
        if (playerDeath) _lifes--;
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
        _spawnBehavior.OnStageComplete -= HandleStages;
        _spawnBehavior.OnPlayerDeath -= HandleDeath;
    }
}