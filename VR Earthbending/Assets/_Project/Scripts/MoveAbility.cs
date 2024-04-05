using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MoveAbility : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private int hitPower = 20;

    // public GameObject MovementRecognizer;
    // XRGrabInteractable grabInteractable;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        // grabInteractable = GetComponent<XRGrabInteractable>();
    }

    private void Update()
    {
        // grabInteractable.hoverEntered.AddListener(x => MovementRecognizer.GetComponent<MovementRecognizer>().OnRayHoverEnter());
        // grabInteractable.hoverExited.AddListener(x => MovementRecognizer.GetComponent<MovementRecognizer>().OnRayHoverExit());
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
