using UnityEngine;

namespace Player
{
    public class ModelDisplay : MonoBehaviour
    {
        public void ActivateFirstPerson()
        {
            // Set full body model to invisible
            gameObject.SetActive(false);
            // Set first person arms active
            // Parent arms to the camera
        }
        public void DeActivateFirstPerson()
        {
            // Set full body model to visible
            gameObject.SetActive(true);
            // Set first person arms inactive
            // Parent arms to the model
        }
    }
}
