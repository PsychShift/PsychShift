using System.Collections;
using UnityEngine;


public class AudioWEnableObject : MonoBehaviour
{
    // GameObject to enable/disable
    public GameObject objectToToggle;

    // Time to disable the object
    public float disableTime = 2.0f;

    private bool hasBeenTriggered = false;

    // This method is called when any collider enters the Box Collider
    private void OnTriggerEnter(Collider other)
    {
        // Check if the trigger has already been used
        if (!hasBeenTriggered)
        {
            // Enable the object if it's not null
            if (objectToToggle != null)
            {
                objectToToggle.SetActive(true);

                // Start coroutine to disable the object after a certain time
                StartCoroutine(DisableObjectAfterDelay());
            }

            // Disable the trigger
            hasBeenTriggered = true;
        }
    }

    // Disable the object after a certain delay
    private IEnumerator DisableObjectAfterDelay()
    {
        // Wait for the specified time
        yield return new WaitForSeconds(disableTime);

        // Disable the object
        if (objectToToggle != null)
        {
            objectToToggle.SetActive(false);
        }
    }
}