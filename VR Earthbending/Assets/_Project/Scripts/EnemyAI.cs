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

    //ragdoll state
    private BoxCollider hitDetectorBoxCollider;
    [SerializeField] private float dieForce;
    private CapsuleCollider ragdollCapsuleCollider; // this capsule turns on when entering ragdoll state, used to get pushed by abilities when in ragdoll state

    private void Start()
    {
        player = GameObject.Find("Main Camera").transform;
        agent = GetComponent<NavMeshAgent>();
        hitDetectorBoxCollider = GetComponent<BoxCollider>();
        hitDetectorBoxCollider.enabled = true;

        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        enemyRagdoll = GetComponent<EnemyRagdoll>();
        healthBarUI = GameObject.Find("UIHealthBar").GetComponent<UIHealthBar>();

        ragdollCapsuleCollider = gameObject.transform.GetChild(1).GetChild(0).gameObject.GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0f && currentHealth > 0)
        {
            float sqDistance = (player.transform.position - agent.destination).sqrMagnitude;
            if (sqDistance > minDistance * minDistance)
            {
                agent.destination = player.transform.position;
            }
            timer = maxTime;
        }
        animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    /// Enemy Health and Take Damange
    public void TakeDamange(int damangeAmount, Vector3 hitDirection)
    {
        currentHealth -= damangeAmount;

        // Debug.Log("currentHealth: " + currentHealth);
        // Debug.Log("maxHealth: " + maxHealth);
        // Debug.Log("input percentage: " + currentHealth / maxHealth);

        healthBarUI.SetHealthBarPercentage(currentHealth / maxHealth);

        if (currentHealth <= 0)
        {
            EnemyDie(hitDirection);
        }
    }
    private void EnemyDie(Vector3 hitDirection)
    {
        enemyRagdoll.ActivateRagdoll();

        healthBarUI.gameObject.SetActive(false);

        //disable player from being able to hit big collider
        hitDetectorBoxCollider.enabled = false;

        //apply force on ragdoll when hit
        hitDirection.y = 1;
        enemyRagdoll.ApplyForce(hitDirection * dieForce);

        //turn on ragdoll capsule colldier to get pushed by abilities
        ragdollCapsuleCollider.enabled = true;
    }
}
