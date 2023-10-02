using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ObjectManipulator : MonoBehaviour, IManipulate
{
    [Tooltip("This is set in code, but can be manually entered as well")]
    [SerializeField] private Vector3 startPosition; // Initial position of the object
    [Tooltip("This must be set in the inspector. It is the position the object will end in.")]
    [SerializeField] private Vector3 endPosition; // End Position
    private Vector3 difference;
    public float animationTime = 2.0f; // Duration of the interaction in seconds

    private bool isMoving = false;

    public bool IsInteracted { get; set; }

    private void Start()
    {
        // Initialize the object's position
        startPosition = transform.position;
        difference = endPosition - startPosition;
    }

    public void Interact()
    {
        if (!isMoving)
        {
            IsInteracted = true;
            StartCoroutine(MoveObject(startPosition + difference, animationTime));
        }
    }

    public void ReverseInteract()
    {
        if (!isMoving)
        {
            IsInteracted = false;
            StartCoroutine(MoveObject(startPosition, animationTime));
        }
    }

    IEnumerator MoveObject(Vector3 targetPosition, float duration)
    {
        isMoving = true;
        float startTime = Time.time;
        float endTime = startTime + duration;

        while (Time.time < endTime)
        {
            float journeyFraction = (Time.time - startTime) / duration;
            transform.position = Vector3.Lerp(startPosition, targetPosition, journeyFraction);
            yield return null;
        }

        transform.position = targetPosition;
        isMoving = false;
    }

    public void Interacted()
    {
        if (!IsInteracted)
            Interact();
        else
            ReverseInteract();
    }
}