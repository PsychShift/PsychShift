using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class SubTest : MonoBehaviour
{
public TextMeshProUGUI textMeshPro;
    public string[] linesOfText; // Array of lines to display
    public float[] timeBetweenLines; // Array of time durations for each line

    private Coroutine textUpdateCoroutine;

    void OnEnable()
    {
        // Start or restart the coroutine when the object is activated
        if (textUpdateCoroutine != null)
            StopCoroutine(textUpdateCoroutine);

        textUpdateCoroutine = StartCoroutine(UpdateTextOverTime());
    }

    void OnDisable()
    {
        // Stop the coroutine when the object is deactivated
        if (textUpdateCoroutine != null)
            StopCoroutine(textUpdateCoroutine);
    }

    IEnumerator UpdateTextOverTime()
    {
        for (int i = 0; i < linesOfText.Length; i++)
        {
            // Update text
            textMeshPro.text = linesOfText[i];

            // Wait for the specified time before moving to the next line
            yield return new WaitForSeconds(timeBetweenLines[i]);
        }

        // Reset the coroutine when it finishes
        textUpdateCoroutine = StartCoroutine(UpdateTextOverTime());
    }
}
