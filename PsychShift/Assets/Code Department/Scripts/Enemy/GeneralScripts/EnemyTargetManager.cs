using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTargetManager : MonoBehaviour
{
    public delegate void TargetChange(Transform target);
    public event TargetChange OnTargetChange;
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
        OnTargetChange?.Invoke(player);
    }
}
