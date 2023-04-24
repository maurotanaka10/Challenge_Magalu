using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class ChangePlayers : MonoBehaviour
{
    PlayerInputSystem playerInputSystem;

    public GameObject ninjaGameObject;
    public GameObject dogGameObject;
    public CinemachineVirtualCamera cameraControll;

    private bool changeNinjaPressed;
    private bool changeDogPressed;

    private void Awake()
    {
        playerInputSystem = new PlayerInputSystem();

        playerInputSystem.ChangePlayer.ChangeToNinja.started += InputChangeToNinja;
        playerInputSystem.ChangePlayer.ChangeToNinja.canceled += InputChangeToNinja;

        playerInputSystem.ChangePlayer.ChangeToDog.started += InputChangeToDog;
        playerInputSystem.ChangePlayer.ChangeToDog.canceled += InputChangeToDog;
    }

    void Update()
    {
        SetChangePlayers();
    }

    void SetChangePlayers()
    {
        if (changeNinjaPressed)
        {
            ninjaGameObject.SetActive(true);
            ninjaGameObject.transform.position = dogGameObject.transform.position;
            dogGameObject.SetActive(false);

            cameraControll.Follow = ninjaGameObject.transform;
        }
        else if (changeDogPressed)
        {
            dogGameObject.SetActive(true);
            dogGameObject.transform.position = ninjaGameObject.transform.position;
            ninjaGameObject.SetActive(false);

            cameraControll.Follow = dogGameObject.transform;

        }
    }

    void InputChangeToNinja(InputAction.CallbackContext context)
    {
        changeNinjaPressed = context.ReadValueAsButton();
    }

    void InputChangeToDog(InputAction.CallbackContext context)
    {
        changeDogPressed = context.ReadValueAsButton();
    }

    private void OnEnable()
    {
        playerInputSystem.Enable();
    }

    private void OnDisable()
    {
        playerInputSystem.Disable();
    }
}
