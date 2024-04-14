using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
    public EnemyStateID GetID()
    {
        return EnemyStateID.Idle;
    }

    public void Enter(EnemyAgent agent)
    {
    }

    public void Exit(EnemyAgent agent)
    {
    }

    public void Update(EnemyAgent agent)
    {
        Vector3 playerDirection = agent.player.position - agent.transform.position;
        if (playerDirection.magnitude > agent.config.maxSightDistance)
        {
            return;
        }

        Vector3 agentDirection = agent.transform.forward;
        playerDirection.Normalize();
        float dotProduct = Vector3.Dot(playerDirection, agentDirection);

        if (dotProduct > 0f)
        {
            agent.stateMachine.ChangeState(EnemyStateID.ChasePlayer);
        }
    }
}
