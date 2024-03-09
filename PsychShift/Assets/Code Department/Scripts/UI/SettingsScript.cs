using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsScript : MonoBehaviour
{
    //public Dropdown resolutionDropdown;
    public Toggle vsyncTog;
    public Toggle fsTog;
    private int fullscreenValue;

    //Resolution[] resolutions;

    // Start is called before the first frame update
    void Start()
    {
        //resolutions = Screen.resolutions;

        //resolutionDropdown.ClearOptions();

       // List<string> options = new List<string>();

        //int currentResolutionIndex = 0;
       // for (int i = 0; i < resolutions.Length; i++)
        //{
            //string option = resolutions[i].width + " x " + resolutions[i].height;
            //options.Add(option);

            //if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            //{
                //currentResolutionIndex = i;
            //}
        //}

        //resolutionDropdown.AddOptions(options);
        //resolutionDropdown.value = currentResolutionIndex;
        //resolutionDropdown.RefreshShownValue();
        if (QualitySettings.vSyncCount == 0)
        {
            vsyncTog.isOn = false;
        }
        else
        {
            vsyncTog.isOn = true;
        }
        Load();
        
    }
    void Update()
    {
        if (fsTog.isOn == true)
        {
            fullscreenValue = 1;
            Save();
        }
        else
        {
            fullscreenValue = 0;
            Save();
        }
    }

    public void SetFullscreen(bool isFullscreen)
    {
        if (fullscreenValue == 1)
        {
            Screen.fullScreen = isFullscreen;
            Debug.Log ("I am on");
        }
        else
        {
            Screen.fullScreen = !isFullscreen;
            Debug.Log ("I am off");
        }
        //Screen.fullScreen = isFullscreen;
    }
    public void ApplyGraphics()
    {
        if (vsyncTog.isOn)
        {
            QualitySettings.vSyncCount = 1;
            
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }
    }
    private void Save()
    {
        PlayerPrefs.SetInt("IsFullScreen" , fullscreenValue);
    }
     private void Load()
    {
        fullscreenValue = PlayerPrefs.GetInt("IsFullScreen");
    }
    

    //public void SetResolution (int resolutionIndex)
   // {
        //Resolution resolution = resolutions[resolutionIndex];
        //Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    //}

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

   
}
