using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private static TimeManager instance;

    public static TimeManager Instance
    {
        get 
        { 
            return instance; 
        }
    }
    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }
        CanvasReferences.Instance.SlowUI.SetActive(false);
        UndoSlowmotion();
    }

    public void DoSlowmotion(float slowdownFactor = 0.1f)
    {
        CanvasReferences.Instance.SlowUI.SetActive(true);
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        
    }

    public void UndoSlowmotion()
    {
        CanvasReferences.Instance.SlowUI.SetActive(false);
        Time.timeScale = 1;
        Time.fixedDeltaTime = Time.timeScale* 0.02f;   
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }
    public void UnPause()
    {
        Time.timeScale = 1;
    }
}
