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
            if (_instance == null)
            {
                _instance = FindObjectOfType<EnemyTargetManager>();
            }
            return _instance;
        }
    }

    public Transform player;

    public void SetPlayer(Transform player)
    {
        this.player = player;
    }
}
