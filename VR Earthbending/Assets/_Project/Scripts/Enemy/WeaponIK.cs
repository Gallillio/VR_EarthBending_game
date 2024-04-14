using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponIK : MonoBehaviour
{
    public Transform targetTransform;
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
}
