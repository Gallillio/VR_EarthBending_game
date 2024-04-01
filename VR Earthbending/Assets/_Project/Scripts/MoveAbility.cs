using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAbility : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private int hitPower = 20;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerHands"))
        {
            // Debug.Log("AAAA HIT");
            rb.useGravity = true;
            rb.velocity = other.transform.forward * hitPower;

            Destroy(gameObject, 4f);
        }
    }
}
