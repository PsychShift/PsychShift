using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsScript : MonoBehaviour
{

    public Toggle vsyncTog;




    // Start is called before the first frame update
    void Start()
    {
  
        if (QualitySettings.vSyncCount == 0)
        {
            vsyncTog.isOn = false;
        }
        else
        {
            vsyncTog.isOn = true;
        }

        
    }


    //public void SetFullscreen(bool isFullscreen)
    //{
        //if (fullscreenValue == 1)
        //{
            //Screen.fullScreen = isFullscreen;
            //Debug.Log ("I am on");
        //}
        //else
        //{
            //Screen.fullScreen = !isFullscreen;
            //Debug.Log ("I am off");
       // }
        //Screen.fullScreen = isFullscreen;
    
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


    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

   
}
