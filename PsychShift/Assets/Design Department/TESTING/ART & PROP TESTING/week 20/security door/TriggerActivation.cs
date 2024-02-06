using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerActivation : MonoBehaviour

{
    // Public object to be enabled
    public GameObject objectToEnable;

    // Variable to keep track of whether the object has been enabled
    private bool isObjectEnabled = false;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the triggering object has a specific tag (you can customize this condition)
        if (other.CompareTag("Player") && !isObjectEnabled)
        {
            // Enable the public object
            objectToEnable.SetActive(true);
            isObjectEnabled = true;

            // Debug message to indicate that the object is enabled
            Debug.Log("Object Enabled!");
        }
    }

}
