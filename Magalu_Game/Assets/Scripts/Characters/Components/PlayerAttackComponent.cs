using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttackComponent : MonoBehaviour
{
    [SerializeField] private GameObject _playerSword;
    [SerializeField] private BoxCollider _boxCollider;

    private int _enemyLifes;

    private void Awake()
    {
        _playerSword.SetActive(false);
        _boxCollider.enabled = false;

        PlayerManager.HandleAttackInput += AttackHandler;
    }

    private void AttackHandler(bool attackPressed)
    {
        if (attackPressed)
        {
            _playerSword.SetActive(true);
            _boxCollider.enabled = true;
        }
    }

    private void SetActionFalseSword()
    {
        _playerSword.SetActive(false);
        _boxCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            _enemyLifes = EnemyController.EnemyLife();
            _enemyLifes--;
            print("jogador acertou o inimigo");
        }
    }

    private void OnDisable()
    {
        PlayerManager.HandleAttackInput -= AttackHandler;
    }
}