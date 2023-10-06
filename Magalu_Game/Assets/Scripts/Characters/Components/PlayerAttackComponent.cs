using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttackComponent : MonoBehaviour
{
    [SerializeField] private EnemyManager _enemyManager;
    
    [SerializeField] private GameObject _playerSword;
    [SerializeField] private Transform _swordPosition;
    [SerializeField] private float _attackRange;
    [SerializeField] private LayerMask _enemyMask;

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
        }
    }

    private IEnumerator EnabelHitAfterDelay(EnemyController enemyController)
    {
        yield return new WaitForSeconds(0.5f);
        enemyController._canBeHit = true;
    }

    private void SetActionFalseSword()
    {
        _playerSword.SetActive(false);
    }

    private void SetColliderOn()
    {
        Collider[] hittedEnemies = Physics.OverlapSphere(_swordPosition.position, _attackRange, _enemyMask);
        foreach(Collider hittedEnemy in hittedEnemies)
        {
            print($"acertou alguem");
            EnemyController _enemyController = hittedEnemy.GetComponent<EnemyController>();
            if (_enemyController != null & _enemyController._canBeHit)
            {
                print("acertou um inimigo");
                _enemyController._canBeHit = false;
                _enemyManager._enemyLife--;
                StartCoroutine(EnabelHitAfterDelay(_enemyController));
            }
        }
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