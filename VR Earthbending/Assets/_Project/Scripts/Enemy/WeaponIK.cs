using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponIK : MonoBehaviour
{
    [SerializeField] private GameObject rockAttack;
    [SerializeField] private Transform rockAttackSpawnPosition;
    MoveAbility moveAbilityScript; // Use this script to get variable: hitPower. So I can use the same set variable
    private bool canFireRock = true;
    private float waitSecondsToSpawn = 2;

    [HideInInspector] public Transform targetTransform;
    public Transform aimTransform; // spine of enemy for now 
    private Transform bone; //spine of enemy body

    //stop bone from rotating if too close or at too much angle
    public float angleLimit = 90f;
    public float distanceLimit = 1.5f;

    private void Start()
    {
        Animator animator = GetComponent<Animator>();

        // bone = animator.GetBoneTransform(HumanBodyBones.UpperChest);
        bone = animator.GetBoneTransform(HumanBodyBones.Spine);
    }

    private Vector3 GetTargetPosition()
    {
        Vector3 targetDirection = targetTransform.position - aimTransform.position;
        Vector3 aimDirection = aimTransform.forward;
        float blendOut = 0f;

        float targetAngle = Vector3.Angle(targetDirection, aimDirection);
        if (targetAngle > angleLimit)
        {
            blendOut += (targetAngle - angleLimit) / 50f;
        }

        float targetDistance = targetDirection.magnitude;
        if (targetDistance < distanceLimit)
        {
            blendOut += distanceLimit - targetDistance;
        }

        Vector3 direction = Vector3.Slerp(targetDirection, aimDirection, blendOut);
        return aimTransform.position + direction;
    }

    private void LateUpdate()
    {
        if (aimTransform == null)
        {
            return;
        }
        if (targetTransform == null)
        {
            return;
        }
        Vector3 targetPosition = GetTargetPosition();
        AimAtTarget(bone, targetPosition);
    }

    private void AimAtTarget(Transform bone, Vector3 targetPosition)
    {
        //follow target
        Vector3 aimDirection = aimTransform.forward;
        Vector3 targetDirection = targetPosition - aimTransform.position;
        Quaternion aimTowards = Quaternion.FromToRotation(aimDirection, targetDirection);

        bone.rotation = aimTowards * bone.rotation;
    }

    public void SetTargetTransform(Transform target)
    {
        targetTransform = target;
    }

    public void SetAimTransform(Transform aim)
    {
        aimTransform = aim;
    }

    IEnumerator AttackPlayerIEnumerator()
    {
        canFireRock = false;
        //create rock
        GameObject instantiatedRockAttackObject = Instantiate(rockAttack, rockAttackSpawnPosition.position, transform.rotation, gameObject.transform);
        // spawns rock but doesnt push it
        Rigidbody instantiatedRockAttackObject_rb = instantiatedRockAttackObject.GetComponent<Rigidbody>();
        MoveAbility moveAbilityScript = instantiatedRockAttackObject.GetComponent<MoveAbility>();

        //make player hand collider not interact with rock
        moveAbilityScript.playerCanMoveAbility = false;

        //push rock after few secods
        // instantiatedRockAttackObject_rb.velocity = transform.forward * moveAbilityScript.hitPower;
        StartCoroutine(PushRockAfterSeconds(instantiatedRockAttackObject_rb, moveAbilityScript));

        //destroy rock if not hit
        Destroy(instantiatedRockAttackObject, 5f);

        //spawn rock one at a time
        yield return new WaitForSeconds(waitSecondsToSpawn);
        canFireRock = true;
    }
    IEnumerator PushRockAfterSeconds(Rigidbody instantiatedRockAttackObject_rb, MoveAbility moveAbilityScript)
    {
        yield return new WaitForSeconds(waitSecondsToSpawn - 1);
        //if it isnt destroyed yet
        if (instantiatedRockAttackObject_rb != null)
        {
            instantiatedRockAttackObject_rb.velocity = transform.forward * moveAbilityScript.hitPower;

        }
    }


    public void AttackPlayerStartCoroutine()
    {
        if (canFireRock)
        {
            StartCoroutine(AttackPlayerIEnumerator());
        }
    }
}
