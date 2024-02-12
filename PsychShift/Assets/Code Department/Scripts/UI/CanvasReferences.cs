using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasReferences : MonoBehaviour
{
    public static CanvasReferences Instance { get; private set; }

    public GameObject SlowUI;
    


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // If another instance already exists, destroy this one
        }
    }
}
