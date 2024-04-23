using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollowCameraReference : MonoBehaviour
{
    public GameObject Prefab;


    public static PlayerFollowCameraReference Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void OnDestroy()
    {
        Instance = null;
    }
}
