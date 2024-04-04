using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class SubTest : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public string[] linesOfText; // Array of lines to display
    public float[] timeBetweenLines; // Array of time durations for each line

    private int currentLineIndex = 0; // Index of the current line
    private float timer = 0f;

    void Start()
    {
        if (textMeshPro == null)
            textMeshPro = GetComponent<TextMeshProUGUI>();

        // Start displaying text immediately
        UpdateText();
    }

    void Update()
    {
        // Update timer
        timer += Time.deltaTime;

        // Check if it's time to change the line
        if (timer >= timeBetweenLines[currentLineIndex])
        {
            // Reset timer
            timer = 0f;

            // Move to the next line or loop back to the beginning if at the end
            currentLineIndex = (currentLineIndex + 1) % linesOfText.Length;

            // Update the text
            UpdateText();
        }
    }

    void UpdateText()
    {
        // Update the TextMeshPro component with the current line of text
        textMeshPro.text = linesOfText[currentLineIndex];
    }
}
