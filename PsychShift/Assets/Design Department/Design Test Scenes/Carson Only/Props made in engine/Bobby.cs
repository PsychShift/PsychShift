using UnityEngine;

public class Bobby : MonoBehaviour
{
    public float bobFrequency = 1.0f;  // Frequency of the bobbing motion.
    public float bobAmplitude = 0.1f; // Amplitude (how much the object bobs).

    private Vector3 originalPosition;
    private float timer = 0.0f;

    private void Start()
    {
        originalPosition = transform.localPosition;
    }

    private void Update()
    {
        // Calculate the bobbing motion using a sine wave.
        float yOffset = Mathf.Sin(timer * bobFrequency) * bobAmplitude;

        // Apply the bobbing to the object's position.
        transform.localPosition = originalPosition + new Vector3(0.0f, yOffset, 0.0f);

        // Increment the timer.
        timer += Time.deltaTime;
    }
}