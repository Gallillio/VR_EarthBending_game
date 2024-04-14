using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;

    private Animator animator;

    //enemy health
    [Header("Health")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;

    EnemyAgent agent;

    UIHealthBar healthBarUI;

    private void Start()
    {
        agent = GetComponent<EnemyAgent>();

        navMeshAgent = GetComponent<NavMeshAgent>();

        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);
    }

    /// Enemy Health and Take Damange
    public void TakeDamange(int damangeAmount, Vector3 hitDirection)
    {
        currentHealth -= damangeAmount;

        healthBarUI = agent.healthBarUI;
        healthBarUI.SetHealthBarPercentage(currentHealth / maxHealth);

        if (currentHealth <= 0)
        {
            EnemyDie(hitDirection);
        }
    }
    private void EnemyDie(Vector3 hitDirection)
    {
        EnemyDeathState deathState = agent.stateMachine.GetState(EnemyStateID.Dead) as EnemyDeathState;
        deathState.hitDirection = hitDirection;
        agent.stateMachine.ChangeState(EnemyStateID.Dead);
    }
}
