using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackComponent : MonoBehaviour
{
    [SerializeField] private GameObject _playerSword;

    private void Awake()
    {
        _playerSword.SetActive(false);

        PlayerManager.HandleAttackInput += AttackHandler;
    }

    private void AttackHandler(bool attackPressed)
    {
        if(attackPressed)
            _playerSword.SetActive(true);
    }
    
    private void SetActionFalseSword()
    {
        _playerSword.SetActive(false);
    }
}
