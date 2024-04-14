using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class EnemyAgentConfig : ScriptableObject
{
    public float maxTime = 1f;
    public float minDistance = 1f;
    public float dieForce = 10f;
    public float maxSightDistance = 5f;

}
