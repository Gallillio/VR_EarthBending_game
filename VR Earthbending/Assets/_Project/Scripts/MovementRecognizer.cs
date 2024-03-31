using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using Unity.XR.CoreUtils.Datums;
using PDollarGestureRecognizer;
using System.IO;
using UnityEngine.Events;
using Unity.VisualScripting;

public class MovementRecognizer : MonoBehaviour
{
    [SerializeField] private InputActionProperty rightActivate;
    [SerializeField] private InputActionProperty leftActivate;

    private float inputThreshold = 0.1f;
    private bool isPressedRight;
    private bool isPressedLeft;
    private bool isPressedBoth;

    private bool isMovingRight = false;
    private bool isMovingLeft = false;
    private bool isMovingBoth = false;

    [SerializeField] private Transform movementSourceRight;
    [SerializeField] private Transform movementSourceLeft;

    [SerializeField] private float newPositionThresholdDistance = 0.05f;
    [SerializeField] private GameObject debugCubePrefab;

    /// <summary>
    /// When creating a new gesture using creationMode, make sure to do the move in the middle of the screen for most accuracy when predicting
    /// </summary>
    [SerializeField] private bool creationMode; //tells if we are trying to create a new gesture or recognize an old one
    [SerializeField] private string newGestureName;
    private List<Gesture> trainingSet = new List<Gesture>();

    private List<Vector3> positionListRight = new List<Vector3>();
    private List<Vector3> positionListLeft = new List<Vector3>();

    [SerializeField] private float recognitionThreshold = 0.8f;

    [System.Serializable]
    public class UnityStringEvent : UnityEvent<string> { };
    [SerializeField] private UnityStringEvent OnRecognized;

    private float elapsedTimeRight;
    private float elapsedTimeLeft;
    private bool canUseBothHandsAbilitiesRight;
    private bool canUseBothHandsAbilitiesLeft;

    void Start()
    {
        string[] gestureFiles = Directory.GetFiles(Application.persistentDataPath, "*.xml");
        foreach (var item in gestureFiles)
        {
            trainingSet.Add(GestureIO.ReadGestureFromFile(item));
        }
    }

    void Update()
    {
        // trigger buttons bools
        isPressedRight = rightActivate.action.ReadValue<float>() > inputThreshold;
        isPressedLeft = leftActivate.action.ReadValue<float>() > inputThreshold;

        //Check if both triggers are pressed at the same time
        CheckIFBothHandAbilitiesAvailable();
        // Debug.Log(isPressedBoth);


        //start the movement Both
        if (!isMovingBoth && isPressedBoth)
        {
            StartMovementBoth();
        }
        //end the movement Both
        else if (isMovingBoth && !isPressedBoth)
        {
            EndMovementBoth();
        }
        //updating the movement Both
        else if (isMovingBoth && isPressedBoth)
        {
            UpdateMovementBoth();
        }

        //start the movement Right
        if (!isMovingRight && isPressedRight && !isPressedBoth)
        {
            StartMovementRight();
        }
        //end the movement Right
        else if (isMovingRight && !isPressedRight && !isPressedBoth)
        {
            EndMovementRight();
        }
        //updating the movement Right
        else if (isMovingRight && isPressedRight && !isPressedBoth)
        {
            UpdateMovementRight();
        }

        //start the movement Left
        if (!isMovingLeft && isPressedLeft && !isPressedBoth)
        {
            StartMovementLeft();
        }
        //end the movement Left
        else if (isMovingLeft && !isPressedLeft && !isPressedBoth)
        {
            EndMovementLeft();
        }
        //updating the movement Left
        else if (isMovingLeft && isPressedLeft && !isPressedBoth)
        {
            UpdateMovementLeft();
        }
    }



    private void CheckIFBothHandAbilitiesAvailable()
    {
        // isPressedBoth is set to true when both hand triggers are pressed at same time 
        if (canUseBothHandsAbilitiesLeft && canUseBothHandsAbilitiesRight)
        {
            isPressedBoth = true;
        }
        if (!isPressedRight || !isPressedLeft) //isPressedBoth is kept as true till one of the triggers is released
        {
            isPressedBoth = false;
        }

        // right check if both hands  
        if (!isPressedRight)
        {
            elapsedTimeRight = 0f;
            canUseBothHandsAbilitiesRight = false;
        }
        else if (isPressedRight)
        {
            elapsedTimeRight += Time.deltaTime;
            canUseBothHandsAbilitiesRight = false;

            if (elapsedTimeRight < 0.5f)
            {
                canUseBothHandsAbilitiesRight = true;
            }
        }

        // left check if both hands  
        if (!isPressedLeft)
        {
            elapsedTimeLeft = 0f;
            canUseBothHandsAbilitiesLeft = false;
        }
        else if (isPressedLeft)
        {
            elapsedTimeLeft += Time.deltaTime;
            canUseBothHandsAbilitiesLeft = false;

            if (elapsedTimeLeft < 0.5f)
            {
                canUseBothHandsAbilitiesLeft = true;
            }
        }
    }

    private void StartMovementBoth()
    {
        // Debug.Log("Start movement Both");

        isMovingBoth = true;

        positionListRight.Clear();
        positionListRight.Add(movementSourceRight.position);
        positionListLeft.Clear();
        positionListLeft.Add(movementSourceLeft.position);

        if (debugCubePrefab)
        {
            Destroy(Instantiate(debugCubePrefab, movementSourceRight.position, Quaternion.identity), 3);
            Destroy(Instantiate(debugCubePrefab, movementSourceLeft.position, Quaternion.identity), 3);
        }
    }
    private void EndMovementBoth()
    {
        // Debug.Log("End movement Both");

        isMovingBoth = false;

        //create gesture from position list
        Point[] pointArrayRight = new Point[positionListRight.Count];
        Point[] pointArrayLeft = new Point[positionListLeft.Count];

        for (int i = 0; i < positionListRight.Count; i++)
        {
            // pointArrayRight[i] = positionListRight[i];
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(positionListRight[i]);
            screenPoint = screenPoint.normalized; // scale the points so that gesture size doesnt matter
            pointArrayRight[i] = new Point(screenPoint.x, screenPoint.y, 0);
        }
        for (int i = 0; i < positionListLeft.Count; i++)
        {
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(positionListLeft[i]);
            screenPoint = screenPoint.normalized; // scale the points so that gesture size doesnt matter
            pointArrayLeft[i] = new Point(screenPoint.x, screenPoint.y, 0);
        }

        Gesture newGestureRight = new Gesture(pointArrayRight);
        Gesture newGestureLeft = new Gesture(pointArrayLeft);

        Result resultRight = PointCloudRecognizer.Classify(newGestureRight, trainingSet.ToArray());
        Result resultLeft = PointCloudRecognizer.Classify(newGestureLeft, trainingSet.ToArray());

        if (resultLeft.Score > recognitionThreshold && resultRight.Score > recognitionThreshold)
        {
            OnRecognized.Invoke("Both|Left:" + resultLeft.GestureClass + "|Right:" + resultRight.GestureClass);
        }
    }
    private void UpdateMovementBoth()
    {
        // Debug.Log("Updating Movement Both");
        if (isMovingLeft || isMovingRight)
        {
            isMovingLeft = false;
            isMovingRight = false;
        }


        Vector3 lastPositionRight = positionListRight[positionListRight.Count - 1];
        Vector3 lastPositionLeft = positionListLeft[positionListLeft.Count - 1];

        if (Vector3.Distance(movementSourceRight.position, lastPositionRight) > newPositionThresholdDistance && Vector3.Distance(movementSourceLeft.position, lastPositionLeft) > newPositionThresholdDistance)
        {
            positionListRight.Add(movementSourceRight.position);
            positionListLeft.Add(movementSourceLeft.position);
            if (debugCubePrefab)
            {
                Destroy(Instantiate(debugCubePrefab, movementSourceRight.position, Quaternion.identity), 3);
                Destroy(Instantiate(debugCubePrefab, movementSourceLeft.position, Quaternion.identity), 3);
            }
        }
    }

    private void StartMovementRight()
    {
        // Debug.Log("Start movement Right");
        isMovingRight = true;

        positionListRight.Clear();
        positionListRight.Add(movementSourceRight.position);

        if (debugCubePrefab)
        {
            Destroy(Instantiate(debugCubePrefab, movementSourceRight.position, Quaternion.identity), 3);
            // Destroy(Instantiate(debugCubePrefab), 3);
        }
    }
    private void EndMovementRight()
    {
        // Debug.Log("End movement Right");
        isMovingRight = false;

        //create gesture from position list
        Point[] pointArrayRight = new Point[positionListRight.Count];

        for (int i = 0; i < positionListRight.Count; i++)
        {
            // pointArrayRight[i] = positionListRight[i];
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(positionListRight[i]);
            screenPoint = screenPoint.normalized; // scale the points so that gesture size doesnt matter
            // Debug.Log(screenPoint.normalized);

            pointArrayRight[i] = new Point(screenPoint.x, screenPoint.y, 0);
        }

        Gesture newGestureRight = new Gesture(pointArrayRight);

        // Add new gesture to training set
        if (creationMode && newGestureName != "")
        {
            newGestureRight.Name = newGestureName;
            trainingSet.Add(newGestureRight);
            Debug.Log(newGestureRight.Name + "Gesture Created");

            string fileName = Application.persistentDataPath + "/" + newGestureName + ".xml";
            GestureIO.WriteGesture(pointArrayRight, newGestureName, fileName);
        }
        // newGestureName is empty (not named in engine)
        else if (creationMode && newGestureName == "")
        {
            Debug.Log("no Gesture name written you retard");
        }
        //recognize old training set
        else
        {
            Result resultRight = PointCloudRecognizer.Classify(newGestureRight, trainingSet.ToArray());
            // Debug.Log("Right Hand Result: " + resultRight.GestureClass + ": " + resultRight.Score);

            if (resultRight.Score > recognitionThreshold)
            {
                OnRecognized.Invoke("Right:" + resultRight.GestureClass);
            }
        }
    }
    private void UpdateMovementRight()
    {
        // Debug.Log("Updating movement Right");

        Vector3 lastPositionRight = positionListRight[positionListRight.Count - 1];

        if (Vector3.Distance(movementSourceRight.position, lastPositionRight) > newPositionThresholdDistance)
        {
            positionListRight.Add(movementSourceRight.position);
            if (debugCubePrefab)
            {
                Destroy(Instantiate(debugCubePrefab, movementSourceRight.position, Quaternion.identity), 3);
            }
        }
    }

    private void StartMovementLeft()
    {
        // Debug.Log("Start movement Left");

        isMovingLeft = true;

        positionListLeft.Clear();
        positionListLeft.Add(movementSourceLeft.position);

        if (debugCubePrefab)
        {
            Destroy(Instantiate(debugCubePrefab, movementSourceLeft.position, Quaternion.identity), 3);
            // Destroy(Instantiate(debugCubePrefab), 3);
        }
    }
    private void EndMovementLeft()
    {
        // Debug.Log("End movement Left");

        isMovingLeft = false;

        //create gesture from position list
        Point[] pointArrayLeft = new Point[positionListLeft.Count];

        for (int i = 0; i < positionListLeft.Count; i++)
        {
            // pointArrayRight[i] = positionListRight[i];
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(positionListLeft[i]);
            screenPoint = screenPoint.normalized; // scale the points so that gesture size doesnt matter
            pointArrayLeft[i] = new Point(screenPoint.x, screenPoint.y, 0);
        }

        Gesture newGestureLeft = new Gesture(pointArrayLeft);
        Result resultLeft = PointCloudRecognizer.Classify(newGestureLeft, trainingSet.ToArray());
        // Debug.Log("Left Hand Result: " + resultLeft.GestureClass + ": " + resultLeft.Score);

        if (resultLeft.Score > recognitionThreshold)
        {
            OnRecognized.Invoke("Left:" + resultLeft.GestureClass);
        }

    }
    private void UpdateMovementLeft()
    {
        // Debug.Log("Updating movement Left");

        Vector3 lastPositionLeft = positionListLeft[positionListLeft.Count - 1];

        if (Vector3.Distance(movementSourceLeft.position, lastPositionLeft) > newPositionThresholdDistance)
        {
            positionListLeft.Add(movementSourceLeft.position);
            if (debugCubePrefab)
            {
                Destroy(Instantiate(debugCubePrefab, movementSourceLeft.position, Quaternion.identity), 3);
            }
        }
    }

}
