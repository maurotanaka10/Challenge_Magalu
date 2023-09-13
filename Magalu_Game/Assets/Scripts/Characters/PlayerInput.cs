using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private PlayerInputSystem _playerInputSystem;

    private void Awake()
    {
        _playerInputSystem = new PlayerInputSystem();

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

    void OnMovementInput(InputAction.CallbackContext context)
    {
        PlayerManager.Instance.SetCharacterMovementInput(context.ReadValue<Vector2>());
    }

    void OnJumpInput(InputAction.CallbackContext context)
    {
        PlayerManager.Instance.SetJumpPressed(context.ReadValueAsButton());
    }

    void OnUsingItemInput(InputAction.CallbackContext context)
    {
        PlayerManager.Instance.SetButtonEPressed(context.ReadValueAsButton());

        if (GameObject.Find("Button").GetComponent<ColorOrderBehavior>().canPressButton == true && PlayerManager.Instance.GetButtonEPressed())
        {
            GameObject.Find("Button").GetComponent<ColorOrderBehavior>().ShowChallenge();
        }
    }
    void OnAttackingInput(InputAction.CallbackContext context)
    {
        PlayerManager.Instance.SetIsSlashing(context.ReadValueAsButton());
    }


    private void OnEnable()
    {
        _playerInputSystem.Enable();
    }

    private void OnDisable()
    {
        _playerInputSystem.Disable();
    }
}
