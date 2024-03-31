using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HMDInfoManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        if (!XRSettings.isDeviceActive)
        {
            Debug.Log("No Headset Connected & No Mock HMD");
        }
        else if (XRSettings.isDeviceActive && (XRSettings.loadedDeviceName == "MockHMD Display" || XRSettings.loadedDeviceName == "MockHMD" || XRSettings.loadedDeviceName == "MockHMDDisplay"))
        {
            Debug.Log("Using Mock HMD");
        }
        else
        {
            Debug.Log("Headset Connected: " + XRSettings.loadedDeviceName);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
