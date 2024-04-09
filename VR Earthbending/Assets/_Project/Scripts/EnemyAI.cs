using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform player;

    // [SerializeField] private LayerMask whatIsGround, whatIsPlayer;
    // [SerializeField] private int health;

    // //Partroling
    // [SerializeField] private Vector3 walkPoint;
    // private bool walkPointSet;
    // [SerializeField] private float walkPointRange;

    // //Attacking
    // [SerializeField] private float timeBetweenAttacks;
    // private bool alreadyAttacked;
    // [SerializeField] private GameObject rockAttack;

    // //States
    // [SerializeField] private float sightRange, attackRange;
    // [SerializeField] private bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        // player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        agent.destination = player.transform.position;

        // check for player sight and attack
        // playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        // playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        // if (!playerInSightRange && !playerInAttackRange)
        // {
        //     Partroling();
        // }
        // if (playerInSightRange && !playerInAttackRange)
        // {
        //     ChasePlayer();
        // }
        // if (playerInSightRange && playerInAttackRange)
        // {
        //     AttackPlayer();
        // }
    }

    // private void Partroling()
    // {
    //     if (!walkPointSet)
    //     {
    //         SearchWalkPoint();
    //     }

    //     if (walkPointSet)
    //     {
    //         agent.SetDestination(walkPoint);
    //     }

    //     Vector3 distanceToWalkingPoint = transform.position - walkPoint;

    //     //walking point reached
    //     if (distanceToWalkingPoint.magnitude < 1f)
    //     {
    //         walkPointSet = false;
    //     }
    // }

    // private void SearchWalkPoint()
    // {
    //     //Calculate random point in range
    //     float randomZ = Random.Range(-walkPointRange, walkPointRange);
    //     float randomX = Random.Range(-walkPointRange, walkPointRange);

    //     walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
    //     if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
    //     {
    //         walkPointSet = true;
    //     }

    // }

    // private void ChasePlayer()
    // {
    //     agent.SetDestination(player.position);
    // }

    // private void AttackPlayer()
    // {
    //     //make sure enemy does not move
    //     agent.SetDestination(transform.position);

    //     transform.LookAt(player);

    //     if (!alreadyAttacked)
    //     {
    //         /// Attack code here
    //         Rigidbody rb = Instantiate(rockAttack, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
    //         rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
    //         rb.AddForce(transform.up * 8f, ForceMode.Impulse);

    //         // Destroy(rb.gameObject, 5); //destroy gameobject after 5 seconds


    //         /// 

    //         alreadyAttacked = true;
    //         Invoke(nameof(ResetAttack), timeBetweenAttacks);
    //     }
    // }

    // private void ResetAttack()
    // {
    //     alreadyAttacked = false;
    // }

    // public void TakeDamage(int damange)
    // {
    //     health -= damange;

    //     if (health <= 0)
    //     {
    //         Invoke(nameof(DestroyEnemy), 0.5f);
    //     }
    // }

    // private void DestroyEnemy()
    // {
    //     Destroy(gameObject);
    // }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere(transform.position, attackRange);

    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawWireSphere(transform.position, sightRange);
    // }

}
