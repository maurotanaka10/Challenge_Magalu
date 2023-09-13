using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private bool playerIsInArena;
    private bool withSwordInHand = false;
    [SerializeField] private GameObject sword;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "ArenaObstacle")
        {
            playerIsInArena = true;
            sword.SetActive(true);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Coin1")
        {
            GameObject.Find("CoinController").GetComponent<CoinOneBehavior>().CoinOneCollect();
        }
        else if (other.gameObject.tag == "Coin2")
        {
            GameObject.Find("CoinController").GetComponent<CoinOneBehavior>().CoinTwoCollect();
        }
    }
}
