using UnityEngine;

public class Enableprop : MonoBehaviour
{
    // GameObject to enable
    public GameObject objectToEnable;

    // This method is called when any collider enters the Box Collider
    private void OnTriggerEnter(Collider other)
    {
        // Enable the object if it's not null
        if (objectToEnable != null)
        {
            objectToEnable.SetActive(true);
        }
    }
}