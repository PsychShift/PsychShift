using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleMaster : MonoBehaviour
{
    private static ParticleMaster _instance;
    public static ParticleMaster Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new ParticleMaster();
            }
            return _instance;
        }
    }
    [Header("Mind Swap")]
    public GameObject MindSwapTunnel;
    public GameObject MindSwapShock;
    public GameObject MindSwapShockBeam;
}
