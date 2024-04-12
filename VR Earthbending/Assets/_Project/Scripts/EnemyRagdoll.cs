using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRagdoll : MonoBehaviour
{
    Rigidbody[] rigidBodies; //rigid bodies of all children
    CapsuleCollider[] colliders; //capsule colliders of all children
    Animator animator;

    void Start()
    {
        rigidBodies = GetComponentsInChildren<Rigidbody>();
        colliders = GetComponentsInChildren<CapsuleCollider>();

        animator = GetComponent<Animator>();
        DeactivateRagdoll();
    }

    public void DeactivateRagdoll()
    {
        foreach (var rigidBody in rigidBodies)
        {
            rigidBody.isKinematic = true;
        }
        foreach (var collider in colliders)
        {
            collider.isTrigger = true;
        }

        animator.enabled = true;
    }

    public void ActivateRagdoll()
    {
        foreach (var rigidBody in rigidBodies)
        {
            rigidBody.isKinematic = false;
        }
        foreach (var collider in colliders)
        {
            collider.isTrigger = false;
        }

        animator.enabled = false;
    }
}
