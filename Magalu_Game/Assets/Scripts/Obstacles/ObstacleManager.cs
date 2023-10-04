using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObstacleManager : MonoBehaviour
{
    public event Action<bool> OnPlayerDeathHandler;
    public static event Action<bool> OnUsingItemHandler;

    private void Awake()
    {
        LaserBehavior[] laserBehaviors = FindObjectsOfType<LaserBehavior>();

        foreach (var laserBehavior in laserBehaviors)
        {
            laserBehavior.OnPlayerDeath += OnPlayerDeath;
        }
        GameManager.OnUsingItemInputContextReceived += UsingItemBehavior;
    }

    private void UsingItemBehavior(bool context)
    {
        OnUsingItemHandler?.Invoke(context);
    }

    private void OnPlayerDeath(bool playerDeath)
    {
        OnPlayerDeathHandler?.Invoke(playerDeath);
    }

    private void OnDisable()
    {
        LaserBehavior[] laserBehaviors = FindObjectsOfType<LaserBehavior>();

        foreach (var laserBehavior in laserBehaviors)
        {
            laserBehavior.OnPlayerDeath -= OnPlayerDeath;
        }

        GameManager.OnUsingItemInputContextReceived -= UsingItemBehavior;
    }
}