using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;

    private Animator animator;

    //follow player efficiently without updating every frame
    [Header("Player Following Agent")]
    [SerializeField] private float maxTime = 1f;
    [SerializeField] private float minDistance = 1f;
    private float timer = 0f;

    //enemy health
    [Header("Health")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    EnemyRagdoll enemyRagdoll;

    private void Start()
    {
        player = GameObject.Find("Main Camera").transform;
        agent = GetComponent<NavMeshAgent>();

        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        enemyRagdoll = GetComponent<EnemyRagdoll>();
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0f)
        {
            float sqDistance = (player.transform.position - agent.destination).sqrMagnitude;
            if (sqDistance > minDistance * minDistance)
            {
                agent.destination = player.transform.position;
            }
            timer = maxTime;
        }
        animator.SetFloat("Speed", agent.velocity.magnitude);


        if (currentHealth <= 0)
        {
            // Debug.Log("deadd");
            EnemyDie();
        }
    }

    /// Enemy Health and Take Damange
    private void TakeDamange(float damangeAmount)
    {
        currentHealth -= damangeAmount;
        if (currentHealth <= 0)
        {
            EnemyDie();
        }
    }
    private void EnemyDie()
    {
        enemyRagdoll.ActivateRagdoll();
    }
}
