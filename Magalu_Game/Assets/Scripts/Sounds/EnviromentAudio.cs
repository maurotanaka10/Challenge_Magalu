using System;
using UnityEngine;

public class EnviromentAudio : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioClip;
    [SerializeField, Range(0f, 100f)] private float _volume;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _audioClip;
        _audioSource.Play();
    }

    private void Update()
    {
        _audioSource.volume = _volume/100;
    }
}
