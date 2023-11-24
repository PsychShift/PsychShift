
using UnityEngine;

public class PropDestroyer : MonoBehaviour
{
    // Define the tag for objects that can be destroyed
    public string destroyableTag = "Prop_Destroy";

    // This method is called when the collider enters the trigger zone
    private void OnTriggerEnter(Collider other)
    {
        // Check if the entering collider has the specified tag
        if (other.CompareTag(destroyableTag))
        {
            // Destroy the GameObject with the specified tag
            Destroy(other.gameObject);
        }
    }
}