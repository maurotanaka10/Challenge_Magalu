using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttackComponent : MonoBehaviour
{
    [SerializeField] private EnemyManager _enemyManager;

    [SerializeField] private GameObject _playerSword;
    [SerializeField] private Transform _swordPosition;
    [SerializeField] private float _attackRange;
    [SerializeField] private LayerMask _enemyMask;

    private int _enemyLifes;
    private bool _isInvulnerable;

    private void Awake()
    {
        _playerSword.SetActive(false);

        PlayerManager.HandleAttackInput += AttackHandler;
    }

    private void AttackHandler(bool attackPressed)
    {
        if (attackPressed)
        {
            _playerSword.SetActive(true);
            _isInvulnerable = true;

            Collider[] hittedEnemies = Physics.OverlapSphere(_swordPosition.position, _attackRange, _enemyMask);
            foreach(Collider hittedEnemy in hittedEnemies)
            {
                _enemyManager._enemyLife--;
                print("acertou um inimigo");
            }
        }
    }

    private void SetActionFalseSword()
    {
        _playerSword.SetActive(false);
        _isInvulnerable = false;
    }

    private void OnDrawGizmos()
    {
        if (_swordPosition == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_swordPosition.position, _attackRange);
    }

    private void OnDisable()
    {
        PlayerManager.HandleAttackInput -= AttackHandler;
    }
}