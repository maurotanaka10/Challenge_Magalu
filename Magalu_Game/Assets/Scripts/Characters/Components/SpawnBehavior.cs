using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBehavior : MonoBehaviour
{
    public event Action<bool> OnPlayerDeath;
    
    private CharacterController _characterControllerRef;

    [SerializeField] private GameManager _gameManager;
    [SerializeField] private Vector3[] _spawnPosition;
    [SerializeField] private GameObject[] _stagesColliders;
    [SerializeField] private CapsuleCollider _capsuleCollider;

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
        if (_gameManager._gameIsOver) return;
        
        if (!_firstStageComplete && !_secondStageComplete)
        {
            transform.position = _spawnPosition[0];
        }
        else if (_firstStageComplete && !_secondStageComplete)
        {
            transform.position = _spawnPosition[1];
        }
        else if (_firstStageComplete && _secondStageComplete)
        {
            transform.position = _spawnPosition[2];
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
            _stagesColliders[0].SetActive(false);
        }

        if (other.gameObject.CompareTag("StageTwoComplete"))
        {
            _secondStageComplete = true;
            _stagesColliders[1].SetActive(false);
            _capsuleCollider.enabled = false;
        }
    }

    private void OnDisable()
    {
        PlayerManager.HandlePlayerDeath -= SetRespawnPlayer;
    }
}