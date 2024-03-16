using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator60sec : MonoBehaviour
{
    public AudioSource audioSource;
    public GameObject firstGameObject;
    public GameObject secondGameObject;
    public float delayInSeconds = 60f;

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            // Play audio source
            if (audioSource != null)
            {
                audioSource.Play();
            }

            // Activate first game object
            if (firstGameObject != null)
            {
                firstGameObject.SetActive(true);
            }

            // Set trigger status to true
            hasTriggered = true;

            // Start delay coroutine
            StartCoroutine(ActivateSecondGameObjectAfterDelay());
        }
    }

    private IEnumerator ActivateSecondGameObjectAfterDelay()
    {
        yield return new WaitForSeconds(delayInSeconds);

        // Activate second game object
        if (secondGameObject != null)
        {
            secondGameObject.SetActive(true);
        }
    }
}
