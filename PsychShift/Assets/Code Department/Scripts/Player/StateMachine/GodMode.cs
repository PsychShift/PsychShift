using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodModeScript : MonoBehaviour
{
        private static GodModeScript instance;

    public static GodModeScript Instance
    {
        get
        {
            return instance;
        }
        set { instance = value; }
    }
}
