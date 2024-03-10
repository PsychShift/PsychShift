using UnityEngine;

public class TorsoRotationController : MonoBehaviour
{
    [SerializeField] private Transform torso;
    public Transform target; // The target to follow
    public bool isRotating = true; // Bool to activate/deactivate rotation
    [SerializeField] private float rotationSpeed = 5f; // Speed of rotation
    [SerializeField] private float minDistance = 2f; // Minimum distance to start rotating
    [SerializeField] private float maxDistance = 10f; // Maximum distance to stop rotating
    [SerializeField] private float minHeightDifference = 0.5f; // Minimum height difference to start rotating
    [SerializeField] private float maxHeightDifference = 2f; // Maximum height difference to stop rotating
    [SerializeField] private Animator animator; // Reference to the Animator component
    [SerializeField] private string rotationParameter = "TorsoRotation"; // Name of the parameter in the Animator
    public Vector3 rotationOffset = Vector3.zero; // Adjustable offset for the rotation

    private void Update()
    {
        if (isRotating && target != null)
        {
            Vector3 directionToTarget = target.position - torso.transform.position;
            float distance = directionToTarget.magnitude;
            float heightDifference = Mathf.Abs(target.position.y - torso.transform.position.y);

            if (distance > minDistance && distance < maxDistance &&
                heightDifference > minHeightDifference && heightDifference < maxHeightDifference)
            {
                // Calculate the rotation towards the target with the offset applied
                Quaternion targetRotation = Quaternion.LookRotation(directionToTarget, Vector3.up) * Quaternion.Euler(rotationOffset);
                // Smoothly rotate the torso towards the target
                torso.transform.rotation = Quaternion.Slerp(torso.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                // Update the Animator parameter
                //animator.SetFloat(rotationParameter, Mathf.Clamp01(heightDifference / maxHeightDifference));
            }
            else
            {
                // Reset the Animator parameter when not rotating
                //animator.SetFloat(rotationParameter, 0);
            }
        }
    }
}

