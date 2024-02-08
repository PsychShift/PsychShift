using UnityEngine;

public class PropSway : MonoBehaviour
{
    public float swaySpeed = 1f; // Adjust this value to control the speed of swaying
    public float swaySize = 10f; // Adjust this value to control the size of swaying

    private Vector3 lastPosition;

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        // Calculate the difference in position since the last frame
        Vector3 movement = transform.position - lastPosition;

        // Calculate the sway amount based on self-movement
        float swayAmount = Mathf.Clamp(movement.magnitude * swaySpeed, 0f, 1f);

        // Apply sway to the prop's rotation
        float swayOffset = Mathf.Sin(Time.time * swaySpeed) * swayAmount * swaySize;
        transform.rotation = Quaternion.Euler(0f, swayOffset, 0f);

        // Update the last position
        lastPosition = transform.position;
    }
}