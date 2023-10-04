using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBehavior : MonoBehaviour
{
    public event Action<bool> OnPlayerDeath; 
    
    private CharacterController _characterControllerRef;

    [SerializeField] private Vector3[] _spawnPosition;

    private bool _firstStageComplete;
    private bool _secondStageComplete;
    private bool _playerIsDeath = false;

    private void Awake()
    {
        PlayerManager.HandlePlayerDeath += SetRespawnPlayer;
        
        _firstStageComplete = false;
        _secondStageComplete = false;
        _playerIsDeath = false;
    }

    private void Update()
    {
        _characterControllerRef = PlayerManager.CharacterControllerRef?.Invoke();
    }

    private void SetRespawnPlayer(bool playerDeath)
    {
        _playerIsDeath = false;
        if (!playerDeath)
            return;
        StartCoroutine(TimerToRespawnPlayer());
    }

    private IEnumerator TimerToRespawnPlayer()
    {
        _characterControllerRef.enabled = false;
        yield return new WaitForSeconds(2.2f);
        RespawnPosition();
        _characterControllerRef.enabled = true;
    }

    private void RespawnPosition()
    {
        if (!_firstStageComplete)
        {
            transform.position = _spawnPosition[0];
        }
        else if (_firstStageComplete)
        {
            transform.position = _spawnPosition[1];
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("FallTrigger"))
        {
            _playerIsDeath = true;
            OnPlayerDeath?.Invoke(_playerIsDeath);
        }
        
        if (other.gameObject.CompareTag("StageOneComplete"))
        {
            _firstStageComplete = true;
        }
    }
}