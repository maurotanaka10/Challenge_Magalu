using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private HUDAudio _hudAudio;
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private GameObject _losePanel;
    [SerializeField] private GameObject _gamePanel;

    private void Awake()
    {
        _gameManager.OnGameIsOver += PanelControl;
    }

    private void PanelControl(bool gameIsOver, bool wasCompleted)
    {
        if(!gameIsOver) return;
        Cursor.lockState = CursorLockMode.None;
        _gamePanel.SetActive(false);

        if (wasCompleted)
        {
            _winPanel.SetActive(true);
            _hudAudio.PlayWinSound();
        }
        else
        {
            _losePanel.SetActive(true);
            _hudAudio.PlayLoseSound();
        }
    }
}
