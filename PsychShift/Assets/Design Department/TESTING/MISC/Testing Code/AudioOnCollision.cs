using System.Collections;
using UnityEngine;

public class AudioOnCollision : MonoBehaviour
{
    // Animation component attached to the GameObject

    public AudioSource AnimationWSound;
    
    // Array of sound effects to play
    public AudioClip[] soundEffects;

    // Array of delays between each sound effect
    public float[] delays;

    private bool hasBeenTriggered = false;

    // This method is called when any collider enters the Box Collider
    private void OnTriggerEnter(Collider other)
    {
        // Check if the trigger has already been used
        if (!hasBeenTriggered)
        {

                AnimationWSound.Play();

                // Play additional sound effects with individual delays
                StartCoroutine(PlaySoundsWithDelays());


            // Disable the trigger
            hasBeenTriggered = true;
        }
    }

    // Play additional sound effects with individual delays
    private IEnumerator PlaySoundsWithDelays()
    {
        for (int i = 0; i < soundEffects.Length; i++)
        {
            // Play sound effect
            AnimationWSound.PlayOneShot(soundEffects[i]);

            // Wait for the specified delay
            yield return new WaitForSeconds(delays[i]);
        }
    }
}