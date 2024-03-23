using UnityEngine;
using System.Collections; // Add this line for IEnumerator

public class Enableprop : MonoBehaviour
{
    // GameObject to enable
    public GameObject objectToEnable;

    // Delay before enabling the object
    public float delay = 0f;

    // This method is called when any collider enters the Box Collider
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 15)
            StartCoroutine(EnableObjectWithDelay());
        // Start a coroutine to enable the object after the delay
        
    }

    // Coroutine to enable the object after a delay
    private IEnumerator EnableObjectWithDelay()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Enable the object if it's not null
        if (objectToEnable != null)
        {
            objectToEnable.SetActive(true);
        }
    }
}