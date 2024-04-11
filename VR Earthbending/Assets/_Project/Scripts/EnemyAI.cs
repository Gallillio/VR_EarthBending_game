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
    [SerializeField] private float maxTime = 1f;
    [SerializeField] private float minDistance = 1f;
    private float timer = 0f;

    private void Awake()
    {
        player = GameObject.Find("Main Camera").transform;
        agent = GetComponent<NavMeshAgent>();

        animator = GetComponent<Animator>();
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
    }
}
