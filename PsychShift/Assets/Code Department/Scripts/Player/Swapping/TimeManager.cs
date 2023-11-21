using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private float fixedDeltaTime;
    public GameObject SlowUI;

    private static TimeManager instance;

    public static TimeManager Instance
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
    void Start()
    {
        SlowUI.SetActive(false);
    }

    public void DoSlowmotion(float slowdownFactor = 0.1f)
    {
        SlowUI.SetActive(true);
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale * 0.05f;
        
    }

    public void UndoSlowmotion()
    {
        SlowUI.SetActive(false);
        Time.timeScale = 1;
        Time.fixedDeltaTime = this.fixedDeltaTime;
        
    }
}
