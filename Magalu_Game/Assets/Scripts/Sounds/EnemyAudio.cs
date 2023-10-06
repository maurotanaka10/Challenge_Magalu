using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _idleClip;
    [SerializeField] private AudioClip _patrolClip;
    [SerializeField] private AudioClip _chaseClip;
    [SerializeField] private AudioClip _attackClip;
    [SerializeField] private AudioClip _attack2Clip;
    [SerializeField] private AudioClip _dieClip;
    [SerializeField, Range(0f, 100f)] private float _volume;

    private bool _soundPlaying = false;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        EnemyManager.OnIdleStateReceived += IdleSound;
        EnemyManager.OnPatrolStateReceived += PatrolSound;
        EnemyManager.OnChaseStateReceived += PlayChaseSound;
        EnemyManager.OnAttackStateReceived += PlayAttackSound;
        EnemyManager.OnComboAttackStateReceived += PlayAttack2Sound;
        EnemyManager.OnDieStateReceived += PlayDeathSound;
    }

    private void Update()
    {
        _audioSource.volume = _volume / 100;
    }

    private void IdleSound(EStatesEnemy currentState)
    {
        if (!_soundPlaying)
        {
            _audioSource.clip = _idleClip;
            _audioSource.Play();
            StartCoroutine(TimerToPlayOtherSound());
        }
    }

    private void PatrolSound(EStatesEnemy currentState)
    {
        if (!_soundPlaying)
        {
            _audioSource.clip = _patrolClip;
            _audioSource.Play();
            StartCoroutine(TimerToPlayOtherSound());
        }
    }

    private void PlayChaseSound(EStatesEnemy currentState)
    {
        if (!_soundPlaying)
        {
            _audioSource.clip = _chaseClip;
            _audioSource.Play();
            StartCoroutine(TimerToPlayOtherSound());
        }
    }

    public void PlayAttackSound(EStatesEnemy currentState)
    {
        if (!_soundPlaying)
        {
            _audioSource.clip = _attackClip;
            _audioSource.Play();
            StartCoroutine(TimerToPlayOtherSound());
        }
    }

    public void PlayAttack2Sound(EStatesEnemy currentState)
    {
        if (!_soundPlaying)
        {
            _audioSource.clip = _attack2Clip;
            _audioSource.Play();
            StartCoroutine(TimerToPlayOtherSound());
        }
    }

    public void PlayDeathSound(EStatesEnemy currentState)
    {
        if (!_soundPlaying)
        {
            _audioSource.clip = _dieClip;
            _audioSource.Play();
            StartCoroutine(TimerToPlayOtherSound());
        }
    }

    IEnumerator TimerToPlayOtherSound()
    {
        _soundPlaying = true;
        yield return new WaitForSeconds(1.0f);
        _soundPlaying = false;
    }

    private void OnDisable()
    {
        EnemyManager.OnIdleStateReceived -= IdleSound;
        EnemyManager.OnPatrolStateReceived -= PatrolSound;
        EnemyManager.OnChaseStateReceived -= PlayChaseSound;
        EnemyManager.OnAttackStateReceived -= PlayAttackSound;
        EnemyManager.OnComboAttackStateReceived -= PlayAttack2Sound;
        EnemyManager.OnDieStateReceived -= PlayDeathSound;
    }
}