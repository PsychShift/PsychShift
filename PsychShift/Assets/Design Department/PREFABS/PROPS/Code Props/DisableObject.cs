using UnityEngine;

public class DisableObject : MonoBehaviour
{
    // GameObject to disable
    public GameObject objectToDisable;

    // This method is called when any collider enters the Box Collider
    private void OnTriggerEnter(Collider other)
    {
        // Disable the object if it's not null
        if (objectToDisable != null)
        {
            objectToDisable.SetActive(false);
        }
    }
}