using UnityEngine;


public class MetalDetectorTrigger : MonoBehaviour
{
    public AudioSource MetalDetectorSound;

    void OnTriggerEnter(Collider other) 
    {
        MetalDetectorSound.Play();
    }
}
