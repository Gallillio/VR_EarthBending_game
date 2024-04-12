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
    UIHealthBar healthBarUI;

    private void Start()
    {
        player = GameObject.Find("Main Camera").transform;
        agent = GetComponent<NavMeshAgent>();

        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        enemyRagdoll = GetComponent<EnemyRagdoll>();
        healthBarUI = GameObject.Find("UIHealthBar").GetComponent<UIHealthBar>();
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0f && currentHealth <= 0)
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
    public void TakeDamange(int damangeAmount)
    {
        currentHealth -= damangeAmount;

        // Debug.Log("currentHealth: " + currentHealth);
        // Debug.Log("maxHealth: " + maxHealth);
        // Debug.Log("input percentage: " + currentHealth / maxHealth);

        healthBarUI.SetHealthBarPercentage(currentHealth / maxHealth);

        if (currentHealth <= 0)
        {
            EnemyDie();
        }
    }
    private void EnemyDie()
    {
        enemyRagdoll.ActivateRagdoll();

        healthBarUI.gameObject.SetActive(false);
    }
}
