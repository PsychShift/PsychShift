using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsScript : MonoBehaviour
{

    public Toggle vSyncToggle;
    public TMP_Dropdown qualityDropdown;

    private void Start()
    {
        // Load the saved state of the toggle from PlayerPrefs
        vSyncToggle.isOn = PlayerPrefs.GetInt("VSyncEnabled", 1) == 1; // Default is true (1)
        
        // Add listener for value change
        vSyncToggle.onValueChanged.AddListener(delegate {
            ToggleValueChanged(vSyncToggle);
        });
         // Populate dropdown options
        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(new System.Collections.Generic.List<string>(QualitySettings.names));

        // Load the saved quality level from PlayerPrefs
        int savedQualityLevel = PlayerPrefs.GetInt("QualityLevel", QualitySettings.GetQualityLevel());
        qualityDropdown.SetValueWithoutNotify(savedQualityLevel);

        // Add listener for value change
        qualityDropdown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(qualityDropdown);
        });
        
    }

    private void ToggleValueChanged(Toggle change)
    {
        // Save the state of the toggle to PlayerPrefs
        PlayerPrefs.SetInt("VSyncEnabled", change.isOn ? 1 : 0);
        PlayerPrefs.Save();

        // Apply the new VSync setting
        QualitySettings.vSyncCount = change.isOn ? 1 : 0;
    }


    private void DropdownValueChanged(TMP_Dropdown change)
    {
        // Save the selected quality level to PlayerPrefs
        int selectedQualityLevel = change.value;
        PlayerPrefs.SetInt("QualityLevel", selectedQualityLevel);
        PlayerPrefs.Save();

        // Apply the new quality level
        QualitySettings.SetQualityLevel(selectedQualityLevel, true);
    }

   
}
