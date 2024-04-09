using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    private Transform player;

    private void Awake()
    {
        player = GameObject.Find("Main Camera").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        agent.destination = player.transform.position;
    }
}
