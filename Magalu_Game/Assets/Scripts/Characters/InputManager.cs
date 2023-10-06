using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInputSystem _playerInputSystem;

    [SerializeField] private GameManager _gameManager;

    public event Action<InputAction.CallbackContext> OnMove;
    public event Action<bool> OnJump;
    public event Action<bool> OnAttack;
    public event Action<bool> OnUsingItem;

    private void Awake()
    {
        _playerInputSystem = new PlayerInputSystem();

        _gameManager.OnGameIsOver += StopInputs;

        _playerInputSystem.Ninja.Run.started += OnMovementInput;
        _playerInputSystem.Ninja.Run.canceled += OnMovementInput;
        _playerInputSystem.Ninja.Run.performed += OnMovementInput;

        _playerInputSystem.Ninja.Jump.started += OnJumpInput;
        _playerInputSystem.Ninja.Jump.canceled += OnJumpInput;

        _playerInputSystem.Ninja.UseObjects.started += OnUsingItemInput;
        _playerInputSystem.Ninja.UseObjects.canceled += OnUsingItemInput;

        _playerInputSystem.Ninja.Attack.started += OnAttackingInput;
        _playerInputSystem.Ninja.Attack.canceled += OnAttackingInput;
    }

    private void StopInputs(bool gameIsOver, bool waCompleted)
    {
        if(!gameIsOver)return;
        _playerInputSystem.Disable();
    }

    void OnMovementInput(InputAction.CallbackContext context)
    {
        OnMove?.Invoke(context);
    }

    void OnJumpInput(InputAction.CallbackContext context)
    {
        OnJump?.Invoke(context.ReadValueAsButton());
    }

    void OnUsingItemInput(InputAction.CallbackContext context)
    {
        OnUsingItem?.Invoke(context.ReadValueAsButton());
    }
    void OnAttackingInput(InputAction.CallbackContext context)
    {
        OnAttack?.Invoke(context.ReadValueAsButton());
    }


    private void OnEnable()
    {
        _playerInputSystem.Enable();
    }

    private void OnDisable()
    {
        _playerInputSystem.Disable();
        
        _playerInputSystem.Ninja.Run.started -= OnMovementInput;
        _playerInputSystem.Ninja.Run.canceled -= OnMovementInput;
        _playerInputSystem.Ninja.Run.performed -= OnMovementInput;

        _playerInputSystem.Ninja.Jump.started -= OnJumpInput;
        _playerInputSystem.Ninja.Jump.canceled -= OnJumpInput;

        _playerInputSystem.Ninja.UseObjects.started -= OnUsingItemInput;
        _playerInputSystem.Ninja.UseObjects.canceled -= OnUsingItemInput;

        _playerInputSystem.Ninja.Attack.started -= OnAttackingInput;
        _playerInputSystem.Ninja.Attack.canceled -= OnAttackingInput;
        
        _gameManager.OnGameIsOver -= StopInputs;
    }
}
