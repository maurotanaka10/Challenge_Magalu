using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinOneBehavior : MonoBehaviour
{
    [SerializeField] private GameObject coin1, coin2;
    [SerializeField] private GameObject exitLabyrinth;

    private bool coinOneCollected = false;
    private bool coinTwoCollected = false;


    public void CoinOneCollect()
    {
        coinOneCollected = true;
        coin1.SetActive(false);
    }

    public void CoinTwoCollect()
    {
        coinTwoCollected = true;
        coin2.SetActive(false);
        exitLabyrinth.SetActive(false);
    }
}
