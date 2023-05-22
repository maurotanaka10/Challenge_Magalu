using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBehavior : MonoBehaviour
{
    [SerializeField] private GameObject coin1, coin2;
    [SerializeField] private EnemyController enemy;
    private bool coinCollected;

    private void Awake()
    {
        
    }

    private void Update()
    {
        if (enemy.life == 0 && !coinCollected)
        {
            coin1.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            coinCollected = true;
            gameObject.SetActive(false);
        }
    }
}
