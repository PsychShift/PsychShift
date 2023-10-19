using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ANYTHING HAVING TO DO WITH THE GUN SHOOTING

[CreateAssetMenu(fileName = "Shoot Config", menuName = "Guns/Shoot Configuration", order = 2)]
public class ShootConfigurationScriptableObject : ScriptableObject 
{
    public bool IsHitscan = true;
    public Bullet BullerPrefab;
    public float BulletSpawnForce = 1000;
    public LayerMask HitMask;
    public Vector3 Spread = new Vector3(0.1f, 0.1f, 0.1f);
    public float FireRate = 0.25f;

}

