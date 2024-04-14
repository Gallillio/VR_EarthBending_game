using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathState : EnemyState
{
    public Vector3 hitDirection;

    public EnemyStateID GetID()
    {
        return EnemyStateID.Dead;
    }

    public void Enter(EnemyAgent agent)
    {
        agent.enemyRagdoll.ActivateRagdoll();

        agent.healthBarUI.gameObject.SetActive(false);

        //disable player from being able to hit big collider
        agent.hitDetectorBoxCollider.enabled = false;

        //apply force on ragdoll when hit
        hitDirection.y = 1;
        agent.enemyRagdoll.ApplyForce(hitDirection * agent.config.dieForce);

        //turn on ragdoll capsule colldier to get pushed by abilities
        agent.ragdollCapsuleCollider.enabled = true;
    }

    public void Exit(EnemyAgent agent)
    {
    }

    public void Update(EnemyAgent agent)
    {
    }
}
