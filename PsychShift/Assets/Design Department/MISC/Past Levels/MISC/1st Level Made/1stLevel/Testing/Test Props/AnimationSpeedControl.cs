using UnityEngine;

public class AnimationSpeedControl : MonoBehaviour
{
    public Animation anim;
    public Transform cameraTransform;
    public float speedMultiplier = 1f; // Adjust this value to control the speed multiplier

    private Vector3 lastCameraPosition;

    void Start()
    {
        if (anim == null)
        {
            Debug.LogError("Animation component not assigned!");
            enabled = false;
        }

        if (cameraTransform == null)
        {
            Debug.LogError("Camera transform not assigned!");
            enabled = false;
        }

        lastCameraPosition = cameraTransform.position;
    }

    void Update()
    {
        // Calculate the difference in camera position since the last frame
        Vector3 cameraMovement = cameraTransform.position - lastCameraPosition;

        // Calculate the speed multiplier based on camera movement
        float speedMultiplier = Mathf.Max(cameraMovement.magnitude, 1f);

        // Adjust the animation playback speed
        anim[anim.clip.name].speed = speedMultiplier;

        // Update the last camera position
        lastCameraPosition = cameraTransform.position;
    }
}