using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodModeScript : MonoBehaviour
{
    public bool GodMode;


    private static GodModeScript instance;

    public static GodModeScript Instance
    {
        get
        {
            return instance;
        }
        set { instance = value; }
    }
    private void Start() {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            SwitchMode();
        }
    }


    public void SwitchMode()
    {
        GodMode = !GodMode;
    }
}
