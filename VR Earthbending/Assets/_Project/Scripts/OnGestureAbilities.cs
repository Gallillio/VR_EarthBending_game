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

    [SerializeField] private Transform player;

    //ray interactor
    [SerializeField] private XRRayInteractor RayInteractorObjectLeft;
    [SerializeField] private XRRayInteractor RayInteractorObjectRight;
    private XRRayInteractor rayInteractorLeft => RayInteractorObjectLeft;
    private XRRayInteractor rayInteractorRight => RayInteractorObjectRight;
    private Vector3 reticlePositionLeft;
    private Vector3 reticleNormalLeft;
    private Vector3 reticlePositionRight;
    private Vector3 reticleNormalRight;

    // public RayInteractorGetHoveredGameObject RayInteractorGetHoveredGameObjectRight;
    // public RayInteractorGetHoveredGameObject RayInteractorGetHoveredGameObjectLeft;

    private void Update()
    {
        // Debug.Log(RayInteractorGetHoveredGameObject.interactableObject);
    }

    private void DoAbility(string gestureNameAndHand)
    {
        //if gesture is made while rayhover interacting with object
        if (gestureNameAndHand.Contains("RayHover|"))
        {
            // Debug.Log(gestureNameAndHand);

            string[] gestureNameAndHandSplitted = gestureNameAndHand.Split(':');

            string handUsed = gestureNameAndHandSplitted[0]; // Get the string before last colon
            string gestureName = gestureNameAndHandSplitted[1]; // Get the string after last colon


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
        reticlePositionRight.y = 1f;
        reticlePositionLeft.y = 1f;

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
    private string ReplaceWhiteSpaceWithUnderscore(string input)
    {
        //// Unity removes all underscores and replaces them with a space, we dont want that as we need gestureName == ability.name to display ability
        return Regex.Replace(input, @"\s+", "_");
    }
}