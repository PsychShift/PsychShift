using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "LaserBeamStats")]
public class LaserBeamStats : ScriptableObject
{
    [Header("General Stats")]
    [Range(0.5f, 5f)]
    public float Width = 1f;
    public float MaxLength = 1000f;

    [Tooltip("Damage per tick.")] [Range(1f, 15f)]
    public float Damage = 1f;

    [Tooltip("How often damage ticks.")] [Range(0.01f, 0.75f)]
    public float DamageSpeed = 1f;

    public LayerMask DontHitTheseLayers;


    [Header("For Stationary Lasers")]
    public float ShootForTime = 0.5f;
    public float RestForTime = 3f;
}
