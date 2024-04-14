using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyStateID
{
    ChasePlayer,
    Dead,
    Idle
}

public interface EnemyState
{
    EnemyStateID GetID();
    void Enter(EnemyAgent agent);
    void Update(EnemyAgent agent);
    void Exit(EnemyAgent agent);
}
