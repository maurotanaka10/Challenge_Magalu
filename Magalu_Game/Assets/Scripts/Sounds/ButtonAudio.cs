using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAudio : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _buttonClip;
    [SerializeField] private AudioClip _lightClip;
    [SerializeField, Range(0f, 100f)] private float _volume;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        _audioSource.volume = _volume/100;
    }

    public void PlayButtonSound()
    {
        _audioSource.clip = _buttonClip;
        _audioSource.Play();
    }

    public void PlayShowLightSound()
    {
        _audioSource.clip = _lightClip;
        _audioSource.Play();
    }
}
