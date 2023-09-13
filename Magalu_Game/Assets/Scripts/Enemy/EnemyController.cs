using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private Animator animator;

    [SerializeField] private NavMeshAgent enemyAgent;
    [SerializeField] private PlayerManager player;

    [Header("State Variables")]
    [SerializeField] private float seenRadius;
    public float life;

    private void Awake()
    {
        enemyAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (CheckIfPlayerIsSeen())
        {
            AttackPlayer();
        }

        if(life <= 0)
        {
            gameObject.SetActive(false);
        }

        EnemyAnimationHandler();
    }

    void AttackPlayer()
    {
        enemyAgent.SetDestination(player.transform.position);
    }

    private bool CheckIfPlayerIsSeen()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        return distance < seenRadius;
    }

    void EnemyAnimationHandler()
    {
        if (CheckIfPlayerIsSeen())
        {
            animator.SetBool("isChasingPlayer", true);
        }
    }
}
