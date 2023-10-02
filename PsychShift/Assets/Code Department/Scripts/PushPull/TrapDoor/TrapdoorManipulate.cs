using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapdoorManipulate : MonoBehaviour, IManipulate
{
    public bool IsInteracted { get; set; }
    public bool CanInteract { get; set; }

    [SerializeField] bool canUseMoreThanOnce;
    private GameObject hinge1;
    private GameObject hinge2;

    private void Start()
    {
        CanInteract = true;
        hinge1 = gameObject.transform.GetChild(0).gameObject;
        hinge2 = gameObject.transform.GetChild(1).gameObject;
    }
    public void Interacted()
    {
        if(!IsInteracted)
            Interact();
        else
            ReverseInteract();
    }

    public void Interact()
    {
        if(CanInteract)
        {
            StartCoroutine(RotateHinges(true, 1));
            IsInteracted = true;
        }
    }

    public void ReverseInteract()
    {
        if(CanInteract)
        {
            StartCoroutine(RotateHinges(false, 5));
            IsInteracted = false;
        }
    }

    private IEnumerator RotateHinges(bool open, float duration)
    {
        CanInteract = false;
        float initialRotation1 = hinge1.transform.localRotation.eulerAngles.z;
        float initialRotation2 = hinge2.transform.localRotation.eulerAngles.z;
        
        float targetRotation1 = open ? (initialRotation1 + 80f) : initialRotation1;
        float targetRotation2 = open ? (initialRotation2 - 80f) : initialRotation2;
        
        float startTime = Time.time;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime = Time.time - startTime;

            float t = elapsedTime / duration; // Interpolation factor.

            float newRotation1 = open
                ? Mathf.Lerp(initialRotation1, targetRotation1, t)
                : Mathf.Lerp(hinge1.transform.localRotation.eulerAngles.z, initialRotation1, t);

            float newRotation2 = open
                ? Mathf.Lerp(initialRotation2, targetRotation2, t)
                : Mathf.Lerp(hinge2.transform.localRotation.eulerAngles.z, initialRotation2, t);

            hinge1.transform.localRotation = Quaternion.Euler(0f, 0f, newRotation1);
            hinge2.transform.localRotation = Quaternion.Euler(0f, 0f, newRotation2);

            yield return null;
        }

        // Ensure the hinges reach their target rotation.
        hinge1.transform.localRotation = Quaternion.Euler(0f, 0f, targetRotation1);
        hinge2.transform.localRotation = Quaternion.Euler(0f, 0f, targetRotation2);

        if (canUseMoreThanOnce)
        {
            CanInteract = true;
        }
    }
}
