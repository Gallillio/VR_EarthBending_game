using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChasePlayerState : EnemyState
{
    public Transform player;
    private float timer = 0f;

    public WeaponIK weaponIK;

    public EnemyStateID GetID()
    {
        return EnemyStateID.ChasePlayer;
    }

    public void Enter(EnemyAgent agent)
    {
        // Debug.Log("EnemyChasePlayer ENTER is working");


    }

    public void Exit(EnemyAgent agent)
    {
    }

    public void Update(EnemyAgent agent)
    {
        //chasing player
        timer -= Time.deltaTime;
        if (timer < 0f)
        {
            float sqDistance = (agent.player.transform.position - agent.navMeshAgent.destination).sqrMagnitude;
            if (sqDistance > agent.config.minDistance * agent.config.minDistance)
            {
                agent.navMeshAgent.destination = agent.player.transform.position;
            }
            timer = agent.config.maxTime;
        }

        //attacking player
        agent.weaponIK.SetTargetTransform(agent.player);
    }


}
