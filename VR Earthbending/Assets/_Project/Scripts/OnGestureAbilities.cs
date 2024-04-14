using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using TMPro;

public class OnGestureAbilities : MonoBehaviour
{
    //gestures list
    [SerializeField] private List<GameObject> oneHandGesturesList;
    [SerializeField] private List<GameObject> twoHandGesturesList;

    private Transform player;
    [SerializeField] private float spawnPositionY = 0.5f;

    //ray interactor
    [SerializeField] private XRRayInteractor RayInteractorObjectLeft;
    [SerializeField] private XRRayInteractor RayInteractorObjectRight;
    private XRRayInteractor rayInteractorLeft => RayInteractorObjectLeft;
    private XRRayInteractor rayInteractorRight => RayInteractorObjectRight;
    private Vector3 reticlePositionLeft;
    private Vector3 reticleNormalLeft;
    private Vector3 reticlePositionRight;
    private Vector3 reticleNormalRight;

    //objects selected to be manipulated (bended) from a far
    private GameObject interactableObject;
    private Rigidbody interactableObject_rb;
    [SerializeField] private GameObject rightDirectController; //use this to get direction of where to move the manipulated object
    [SerializeField] private GameObject leftDirectController;
    [SerializeField] private int hitPower = 20;

    private void Start()
    {
        player = GameObject.Find("Main Camera").transform;

        // RayInteractorObjectLeft = GameObject.Find("Downwards Ray Interactor Left").GetComponent<XRRayInteractor>();
        // RayInteractorObjectRight = GameObject.Find("Downwards Ray Interactor Right").GetComponent<XRRayInteractor>();
        // rightDirectController = GameObject.Find("Direct Interactor Right");
        // leftDirectController = GameObject.Find("Direct Interactor Left");
    }

    private void DoAbility(string gestureNameAndHand)
    {
        // Debug.Log(gestureNameAndHand);

        //if gesture is made while rayhover interacting with object
        if (gestureNameAndHand.Contains("RayHover|"))
        {

            string[] gestureNameAndHandSplitted = gestureNameAndHand.Split(':');

            string handUsedWithChecker = gestureNameAndHandSplitted[0]; // Get the string before last colon
            string gestureName = gestureNameAndHandSplitted[1]; // Get the string after last colon

            string[] handUsedWithCheckerSplitted = handUsedWithChecker.Split('|');

            string handUsed = handUsedWithCheckerSplitted[1];

            ManipulateAbility(gestureName, handUsed);
        }
        //if gesture is made without rayhover interacting with object
        else
        {
            //if two hand ability is used
            if (gestureNameAndHand.Contains("Both|"))
            {
                // Split the input string by '|'
                string[] parts = gestureNameAndHand.Split('|');

                // Initialize variables to store the extracted values
                string leftGesture = "";
                string rightGesture = "";

                // Loop through each part of the split string
                foreach (string part in parts)
                {
                    // Split the part by ':'
                    string[] subparts = part.Split(':');

                    // Check if the split resulted in two parts
                    if (subparts.Length == 2)
                    {
                        // Check if it's the Left or Right value
                        if (subparts[0] == "Left")
                        {
                            // Extract the Left value
                            leftGesture = subparts[1];
                        }
                        else if (subparts[0] == "Right")
                        {
                            // Extract the Right value
                            rightGesture = subparts[1];
                        }
                    }
                }

                // add underscore instead of whitespace
                leftGesture = ReplaceWhiteSpaceWithUnderscore(leftGesture);
                rightGesture = ReplaceWhiteSpaceWithUnderscore(rightGesture);

                // Debug.Log("leftGesture: " + leftGesture);
                // Debug.Log("rightGesture: " + rightGesture);

                //generate ability
                GenerateAbility(leftGesture: leftGesture, rightGesture: rightGesture);

            }
            //if one hand ability is used
            else
            {
                // Debug.Log(gestureNameAndHand);

                string[] gestureNameAndHandSplitted = gestureNameAndHand.Split(':');

                string handUsed = gestureNameAndHandSplitted[0]; // Get the string before last colon
                string gestureName = gestureNameAndHandSplitted[1]; // Get the string after last colon
                // Debug.Log(handUsed + ": " + gestureName);

                gestureName = ReplaceWhiteSpaceWithUnderscore(gestureName);

                if (handUsed == "Right")
                {
                    GenerateAbility(leftGesture: null, rightGesture: gestureName);
                }
                else if (handUsed == "Left")
                {
                    GenerateAbility(leftGesture: gestureName, rightGesture: null);
                }
            }
        }

    }

    private void GenerateAbility(string leftGesture, string rightGesture)
    {
        //player position and rotation
        // Vector3 abilityPosition = (player.forward * 2) + player.position;
        Quaternion abilityRotation = new Quaternion(0, player.rotation.y, 0, player.rotation.w);
        //ray positions
        rayInteractorLeft.TryGetHitInfo(out reticlePositionLeft, out reticleNormalLeft, out _, out _);
        rayInteractorRight.TryGetHitInfo(out reticlePositionRight, out reticleNormalRight, out _, out _);

        //ray reticle position with y position changed
        reticlePositionRight.y = spawnPositionY;
        reticlePositionLeft.y = spawnPositionY;

        // Wall Ability
        if (leftGesture == "vertical" && rightGesture == "vertical")
        {
            foreach (var ability in twoHandGesturesList)
            {
                bool namesAreEqual = "wall_2" == ability.name;

                if (namesAreEqual)
                {
                    // Debug.Log(ability);
                    // Destroy(Instantiate(ability, reticlePositionRight, abilityRotation), 5);
                    Instantiate(ability, reticlePositionRight, abilityRotation);
                }
            }
        }

        // Rock Ability
        if (leftGesture == "vertical" && rightGesture == null)
        {
            foreach (var ability in oneHandGesturesList)
            {
                bool namesAreEqual = "rock_1" == ability.name;

                if (namesAreEqual)
                {
                    // Debug.Log(ability);
                    Instantiate(ability, reticlePositionLeft, abilityRotation);
                }
            }
        }
        if (rightGesture == "vertical" && leftGesture == null)
        {
            foreach (var ability in oneHandGesturesList)
            {
                bool namesAreEqual = "rock_1" == ability.name;

                if (namesAreEqual)
                {
                    // Debug.Log(ability);
                    Instantiate(ability, reticlePositionRight, abilityRotation);
                }
            }
        }
    }

    private void ManipulateAbility(string gestureName, string handUsed)
    {
        // Debug.Log("gestureName: " + gestureName);
        // Debug.Log("interactableObject name: " + interactableObject.name);
        // Debug.Log("handUsed: " + handUsed);

        // push selected rock
        if (gestureName == "horizontal" && interactableObject.name == "rock_1(Clone)" && handUsed == "Right")
        {
            // Debug.Log("I CAN MAKE A ROCK FROM A FAR BRO");

            interactableObject_rb.useGravity = true;
            interactableObject_rb.velocity = rightDirectController.transform.forward * hitPower;
        }
        else if (gestureName == "horizontal" && interactableObject.name == "rock_1(Clone)" && handUsed == "Left")
        {
            // Debug.Log("I CAN MAKE A ROCK FROM A FAR BRO");

            interactableObject_rb.useGravity = true;
            interactableObject_rb.velocity = rightDirectController.transform.forward * hitPower;
        }
    }

    public void SetInteractableObject(GameObject interactableObjectReciever)
    {
        interactableObject = interactableObjectReciever;
        interactableObject_rb = interactableObject.GetComponent<Rigidbody>();

        // Debug.Log(interactableObject);
    }
    private string ReplaceWhiteSpaceWithUnderscore(string input)
    {
        //// Unity removes all underscores and replaces them with a space, we dont want that as we need gestureName == ability.name to display ability
        return Regex.Replace(input, @"\s+", "_");
    }
}
