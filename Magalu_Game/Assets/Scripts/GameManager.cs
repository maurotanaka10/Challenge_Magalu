using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private PlayerManager _playerManager;

    public static event Action<InputAction.CallbackContext> OnMoveInputContextReceived;
    public static event Action<bool> OnJumpInputContextReceived;
    public static event Action<bool> OnAttackInputContextReceived;
    public static event Action<bool> OnUsingItemInputContextReceived;

    private void Awake()
    {
        _inputManager.OnMove += OnMoveInputHandler;
        _inputManager.OnJump += OnJumpInputHandler;
        _inputManager.OnAttack += OnAttackInputHandler;
        _inputManager.OnUsingItem += OnUsingItemInputHandler;
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
    }
}
    
