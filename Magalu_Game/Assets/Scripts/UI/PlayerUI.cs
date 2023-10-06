using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private PlayerManager _playerManager;
    [SerializeField] private GameObject[] _lifeBar;

    private void Awake()
    {
        _lifeBar[0].SetActive(true);
        _lifeBar[1].SetActive(true);
        _lifeBar[2].SetActive(true);
    }

    private void Update()
    {
        if (_playerManager._lifes == 3)
        {
            _lifeBar[0].SetActive(true);
        }
        else if(_playerManager._lifes == 2)
        {
            _lifeBar[0].SetActive(false);
        }
        else if (_playerManager._lifes == 1)
        {
            _lifeBar[1].SetActive(false);
        }
        else if (_playerManager._lifes == 0)
        {
            _lifeBar[2].SetActive(false);
        }
    }
}
