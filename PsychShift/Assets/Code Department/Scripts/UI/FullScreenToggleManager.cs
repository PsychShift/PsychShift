using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullScreenToggleManager : MonoBehaviour
{
public Toggle fullScreenToggle;

    private const string fullScreenKey = "FullScreenState";

    void Start()
    {
        // Load the full screen state from PlayerPrefs when the game starts
        LoadFullScreenState();

        // Add a listener to the toggle's onValueChanged event
        fullScreenToggle.onValueChanged.AddListener(OnToggleValueChanged);
    }

    void OnToggleValueChanged(bool isFullScreen)
    {
        // Set the full screen mode when the toggle changes
        SetFullScreen(isFullScreen);

        // Save the full screen state to PlayerPrefs
        SaveFullScreenState(isFullScreen);
    }

    void SaveFullScreenState(bool isFullScreen)
    {
        // Save the full screen state to PlayerPrefs
        PlayerPrefs.SetInt(fullScreenKey, isFullScreen ? 1 : 0);
        PlayerPrefs.Save(); // Make sure to call Save() to persist the changes
    }

    void LoadFullScreenState()
    {
        // Load the full screen state from PlayerPrefs
        if (PlayerPrefs.HasKey(fullScreenKey))
        {
            int savedFullScreenState = PlayerPrefs.GetInt(fullScreenKey);
            bool isFullScreen = savedFullScreenState == 1;

            // Set the toggle and adjust the full screen mode accordingly
            fullScreenToggle.isOn = isFullScreen;
            SetFullScreen(isFullScreen);
        }
    }

    void SetFullScreen(bool isFullScreen)
    {
        // Set the full screen mode
        Screen.fullScreen = isFullScreen;
    }
}
