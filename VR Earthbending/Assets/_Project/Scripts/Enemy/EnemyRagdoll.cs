using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyRagdoll : MonoBehaviour
{
    Rigidbody[] rigidBodies; //rigid bodies of all children
    Animator animator;

    private WeaponIK weaponIKScript;

    void Start()
    {
        weaponIKScript = GetComponent<WeaponIK>();

        rigidBodies = GetComponentsInChildren<Rigidbody>();

        animator = GetComponent<Animator>();
        DeactivateRagdoll();
    }

    public void DeactivateRagdoll()
    {
        foreach (var rigidBody in rigidBodies)
        {
            rigidBody.isKinematic = true;
        }

        animator.enabled = true;
    }

    public void ActivateRagdoll()
    {
        foreach (var rigidBody in rigidBodies)
        {
            rigidBody.isKinematic = false;
        }

        animator.enabled = false;

        //deactivate bone aim at player
        weaponIKScript.aimTransform = null;
    }

    public void ApplyForce(Vector3 force)
    {
        var rigitBody = animator.GetBoneTransform(HumanBodyBones.Hips).GetComponent<Rigidbody>();
        rigitBody.AddForce(force, ForceMode.VelocityChange);
    }
}
