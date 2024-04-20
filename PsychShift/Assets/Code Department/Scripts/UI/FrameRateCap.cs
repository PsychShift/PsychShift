using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FrameRateCap : MonoBehaviour
{
    public Slider fpsSlider;
    public Toggle fpsToggle;
    [SerializeField] TextMeshProUGUI fpsSliderTitle;

    private const string FPSCapKey = "FPSCap";
    private const string FPSLimitEnabledKey = "FPSLimitEnabled";

    private void Start()
    {
        // Load saved FPS cap value from PlayerPrefs
        int savedFPSCap = PlayerPrefs.GetInt(FPSCapKey, 60);
        fpsSlider.value = savedFPSCap;

        // Load saved FPS limit enabled state from PlayerPrefs
        bool savedFPSLimitEnabled = PlayerPrefs.GetInt(FPSLimitEnabledKey, 0) == 1;
        fpsToggle.isOn = savedFPSLimitEnabled;

        // Apply FPS limit based on saved state
        SetFPSCap(savedFPSCap, savedFPSLimitEnabled);

        // Add listeners for value change
        fpsSlider.onValueChanged.AddListener(delegate {
            OnSliderValueChanged();
        });
        fpsToggle.onValueChanged.AddListener(delegate {
            OnToggleValueChanged();
        });
    }
    void Update ()
    {
        fpsSliderTitle.text = "FPS: " + Mathf.Round(fpsSlider.value);
    }

    private void OnSliderValueChanged()
    {
        int selectedFPSCap = Mathf.RoundToInt(fpsSlider.value);
        PlayerPrefs.SetInt(FPSCapKey, selectedFPSCap);
        PlayerPrefs.Save();

        SetFPSCap(selectedFPSCap, fpsToggle.isOn);
    }

    private void OnToggleValueChanged()
    {
        bool fpsLimitEnabled = fpsToggle.isOn;
        PlayerPrefs.SetInt(FPSLimitEnabledKey, fpsLimitEnabled ? 1 : 0);
        PlayerPrefs.Save();

        SetFPSCap(Mathf.RoundToInt(fpsSlider.value), fpsLimitEnabled);
    }

    private void SetFPSCap(int fpsCap, bool enabled)
    {
        if (enabled)
        {
            Application.targetFrameRate = fpsCap;
        }
        else
        {
            Application.targetFrameRate = -1; // No FPS cap
        }
    }
}