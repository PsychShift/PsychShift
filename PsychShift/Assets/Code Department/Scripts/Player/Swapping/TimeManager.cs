using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager1 : MonoBehaviour
{
    private float fixedDeltaTime;

    private static TimeManager1 instance;

    public static TimeManager1 Instance
    {
        get { return instance; }
    }
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            // If an instance already exists, destroy this duplicate
            Destroy(gameObject);
        }

        this.fixedDeltaTime = Time.fixedDeltaTime;
    }

    public void DoSlowmotion(float slowdownFactor = 0.1f)
    {
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale * 0.05f;
    }

    public void UndoSlowmotion()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale * 0.05f;
    }
}
