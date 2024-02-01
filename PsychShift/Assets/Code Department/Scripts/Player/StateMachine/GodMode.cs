using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodModeScript : MonoBehaviour
{
    public bool GodMode {  get; private set; }


    private static GodModeScript instance;

    public static GodModeScript Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject();
                instance = go.AddComponent<GodModeScript>();
            }
            return instance;
        }
        set { instance = value; }
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
