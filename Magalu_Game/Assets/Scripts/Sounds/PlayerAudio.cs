using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _attackClip;
    [SerializeField] private AudioClip _deathClip;
    [SerializeField, Range(0f, 100f)] private float _volume;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        _audioSource.volume = _volume/100;
    }

    private void PlayAttackSound()
    {
        _audioSource.clip = _attackClip;
        _audioSource.Play();
    }

    private void PlayDeathSound()
    {
        _audioSource.clip = _deathClip;
        _audioSource.Play();
    }
}
