using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MoveAbility : MonoBehaviour
{
    private Rigidbody rb;
    public int hitPower = 20;

    // find MovementRecognizer gameObject and save it in this variable
    private GameObject MovementRecognizer;

    private MovementRecognizer movementRecognizerScript;

    XRGrabInteractable grabInteractable;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        grabInteractable = GetComponent<XRGrabInteractable>();

        MovementRecognizer = GameObject.Find("Movement Recognizer");
        movementRecognizerScript = MovementRecognizer.GetComponent<MovementRecognizer>();
    }

    private void Update()
    {
        HoverEnterAndExit();
    }

    //when punching the ability from close
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

    //when bending the ability from far using ray
    private void HoverEnterAndExit()
    {
        grabInteractable.hoverEntered.AddListener(x => movementRecognizerScript.OnRayHoverEnter());
        grabInteractable.hoverExited.AddListener(x => movementRecognizerScript.OnRayHoverExit());
    }

}
