using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTargetManager : MonoBehaviour
{
    private static EnemyTargetManager _instance;
    public static EnemyTargetManager Instance
    {
        get
        {
            return _instance;
        }
    }
    void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public Transform player;

    public void SetPlayer(Transform player)
    {
        this.player = player;
    }
}
