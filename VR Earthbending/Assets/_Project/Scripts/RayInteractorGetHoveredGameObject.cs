using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RayInteractorGetHoveredGameObject : MonoBehaviour
{
    public GameObject interactableObject;
    // private Rigidbody interactableObject_rb;

    public void OnHoverEntered(HoverEnterEventArgs args)
    {
        // Debug.Log($"{args.interactorObject} hovered over {args.interactableObject}", this);

        interactableObject = args.interactableObject.transform.gameObject;

        // interactableObject_rb = interactableObject.GetComponent<Rigidbody>();
        // interactableObject_rb.useGravity = true;
        // interactableObject_rb.velocity = transform.forward * 4;
    }

    public void OnHoverExited(HoverExitEventArgs args)
    {
        // Debug.Log($"{args.interactorObject} stopped hovering over {args.interactableObject}", this);
    }
}
