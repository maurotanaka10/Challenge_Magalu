using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerManager : MonoBehaviour
{
    public static event Action<InputAction.CallbackContext, float> HandleMoveInput;
    public static event Action<bool, float> HandleJumpInput;
    public static event Action<bool> HandleAttackInput;
    public static event Action<bool> HandleUsingItemInput;

    public delegate CharacterController CharacterControllerReference();

    public static CharacterControllerReference CharacterControllerRef;

    [SerializeField] private float _velocity;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private int _lives;

    private void Awake()
    {
        GameManager.OnMoveInputContextReceived += HandleMove;
        GameManager.OnJumpInputContextReceived += HandleJump;
        GameManager.OnAttackInputContextReceived += HandleAttack;
        GameManager.OnUsingItemInputContextReceived += HandleUsingItem;
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


    private void OnDisable()
    {
        GameManager.OnMoveInputContextReceived -= HandleMove;
        GameManager.OnJumpInputContextReceived -= HandleJump;
        GameManager.OnAttackInputContextReceived -= HandleAttack;
        GameManager.OnUsingItemInputContextReceived -= HandleUsingItem;
    }
}
