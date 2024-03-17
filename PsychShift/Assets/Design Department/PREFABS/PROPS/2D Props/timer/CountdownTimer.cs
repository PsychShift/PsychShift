using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public TextMeshPro timerText;
    public float countdownDuration = 60f;

    private float currentTime;

    private void Start()
    {
        // Initialize timer
        currentTime = countdownDuration;

        // Ensure TextMeshPro component is assigned
        if (timerText == null)
        {
            Debug.LogError("TextMeshPro component is not assigned to the timerText field!");
        }
    }

    private void Update()
    {
        // Update timer
        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            // Timer has reached zero
            currentTime = 0;
            // Optionally, you can perform any action here when the timer reaches zero
        }

        // Update text display
        UpdateTimerText();
    }

    private void UpdateTimerText()
    {
        // Format remaining time as minutes and seconds
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);

        // Update the TextMeshPro text
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}