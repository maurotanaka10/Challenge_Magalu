using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBehavior : MonoBehaviour
{
    [SerializeField] private float damageSword;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            SetDamage();
            Debug.Log("acertei");
        }
    }

    void SetDamage()
    {
        GameObject.Find("Enemy").GetComponent<EnemyController>().life -= damageSword;
    }
}
