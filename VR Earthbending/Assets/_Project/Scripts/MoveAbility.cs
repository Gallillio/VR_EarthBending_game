using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MoveAbility : MonoBehaviour
{
    public bool enemyAttackIsInteractable = false;
    public bool playerCanMoveAbility = true; //this checks if the ability is used by player or enemy, if enemy is using it then we dont want player to interact with it unless doing so on purpose

    private Rigidbody rb;
    public int hitPower = 20;

    // find MovementRecognizer gameObject and save it in this variable
    private GameObject MovementRecognizer;

    private MovementRecognizer movementRecognizerScript;

    XRGrabInteractable grabInteractable;
    private void Start()
    {
        // playerCanMoveAbility = true;

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
        if (other.gameObject.CompareTag("PlayerHands") && playerCanMoveAbility)
        {
            // Debug.Log("AAAA HIT");

            // rb.useGravity = true;
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
