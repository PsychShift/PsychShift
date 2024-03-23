using System.Collections;
using UnityEngine;

public class PanelScript : MonoBehaviour
{
    // Animation component attached to the GameObject
    public Animation anim;
    public AudioSource AnimationWSound;
    
    // Array of sound effects to play
    public AudioClip[] soundEffects;

    // Array of delays between each sound effect
    public float[] delays;

    public GameObject enableObject1;
    public GameObject disableObject1;
    public float enableDisableDelay1;

    public GameObject enableObject2;
    public GameObject disableObject2;
    public float enableDisableDelay2;

    private bool hasBeenTriggered = false;

    // This method is called when any collider enters the Box Collider
    private void OnTriggerEnter(Collider other)
    {
        // Check if the trigger has already been used
        if (!hasBeenTriggered && other.gameObject.layer == 15)
        {
            // Enable the first GameObject
            if (enableObject1 != null)
                enableObject1.SetActive(true);

            // Trigger animation if there is an Animation component attached
            if (anim != null)
            {
                anim.Play();
                AnimationWSound.Play();

                // Play additional sound effects with individual delays
                StartCoroutine(PlaySoundsWithDelays());
            }

            // Disable the trigger
            hasBeenTriggered = true;

            // Enable and disable objects after delays
            StartCoroutine(EnableDisableObjects());
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

    // Enable and disable objects after delays
    private IEnumerator EnableDisableObjects()
    {
        yield return new WaitForSeconds(enableDisableDelay1);
        
        if (disableObject1 != null)
            disableObject1.SetActive(false);

        if (enableObject2 != null)
            enableObject2.SetActive(true);

        yield return new WaitForSeconds(enableDisableDelay2 - enableDisableDelay1);
        
        if (disableObject2 != null)
            disableObject2.SetActive(false);
    }
}