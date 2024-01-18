using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]

public class ObjectManipulator : MonoBehaviour, IManipulate
{
    public bool IsInteracted { get; set; }
    public bool CanInteract { get; set; }
    [Tooltip("This is set in code, but can be manually entered as well")]
    [SerializeField] private Vector3 startPosition; // Initial position of the object
    [Tooltip("This must be set in the inspector. It is the position the object will end in.")]
    [SerializeField] private Vector3 endPosition; // End Position
    public float animationTime = 2.0f; // Duration of the interaction in seconds
    public AudioSource audioSource; // Added AudioSource

    private void Start()
    {
        CanInteract = true;
        startPosition = transform.position;
    }

    public void Interacted()
    {
        if (!IsInteracted)
            Interact();
        else
            ReverseInteract();
    }

    public void Interact()
    {
        if (CanInteract)
        {
            IsInteracted = true;
            StartCoroutine(MoveObject(endPosition, animationTime));
            PlayAudio(); // Added PlayAudio call
        }
    }

    public void ReverseInteract()
    {
        if (CanInteract)
        {
            IsInteracted = false;
            StartCoroutine(MoveObject(startPosition, animationTime));
            PlayAudio(); // Added PlayAudio call
        }
    }

    private void PlayAudio()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    private IEnumerator MoveObject(Vector3 targetPosition, float duration)
    {
        CanInteract = false;
        float startTime = Time.time;
        float endTime = startTime + duration;
        Vector3 currentPosition = transform.position;

        while (Time.time < endTime)
        {
            float journeyFraction = (Time.time - startTime) / duration;
            transform.position = Vector3.Lerp(currentPosition, targetPosition, journeyFraction);
            yield return null;
        }

        transform.position = targetPosition;
        CanInteract = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Prop_Destroy")
        {
            if(other.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(999);
            }
        }
    }
}