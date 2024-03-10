using System.Collections;
using System.Collections.Generic;
using Guns.Demo;
using UnityEngine;

public class DifficultySelector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Easy()
    {
        EnemyGunSelector.DamageReduction = 0.1f;
    }
    public void Medium()
    {
        EnemyGunSelector.DamageReduction = 0.2f;
    }
    public void Hard()
    {
        EnemyGunSelector.DamageReduction = 0.3f;
    }
}
