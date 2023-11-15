using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    public AudioSource audioSource;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            audioSource.Play();
            Debug.Log("Player collided with the object.");
        }
    }
}