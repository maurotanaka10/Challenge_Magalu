using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDAudio : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _startClip;
    [SerializeField] private AudioClip _homeClip;
    [SerializeField] private AudioClip _winClip;
    [SerializeField] private AudioClip _loseClip;
    [SerializeField, Range(0f, 100f)] private float _clickVolume;
    [SerializeField, Range(0f, 100f)] private float _hudVolume;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    
    public void PlayStartSound()
    {
        _audioSource.clip = _startClip;
        _audioSource.volume = _clickVolume/100;
        _audioSource.Play();
    }
    public void PlayHomeSound()
    {
        _audioSource.clip = _homeClip;
        _audioSource.volume = _clickVolume/100;
        _audioSource.Play();
    }

    public void PlayWinSound()
    {
        _audioSource.clip = _winClip;
        _audioSource.volume = _hudVolume/100;
        _audioSource.Play();
    }
    public void PlayLoseSound()
    {
        _audioSource.clip = _loseClip;
        _audioSource.volume = _hudVolume/100;
        _audioSource.Play();
    }
}
