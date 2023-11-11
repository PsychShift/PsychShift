using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AIAgression", menuName = "AI/Agression", order = 1)]
public class AIAgression : ScriptableObject
{
    [Tooltip("How far the AI can see the player")]
    public float DetectionRange;
    public float SphereCastDetectionRadius;
    [Tooltip("How long the AI will be aggressive at the player after they are detected")]
    public float StopChasingTime;
    [Tooltip("Determines how often the AI will shoot, for how long, and how long it will wait between shots")]
    public FireRateAgression FireRateAgression;


    [Tooltip("When the enemy is chasing the player, it should stop this distance away")]
    public float PlayerStoppingDistance;
    [Tooltip("The distance the enemy should stop at when patroling or guarding")]
    public float DestinationStoppingDistance;

    [Tooltip("If true, the enemy will take cover when it is chasing the player or running away.")]
    public bool TakesCover;

    
}

public enum FireRateAgression
{
    NonExistant,
    VeryLow,
    Low,
    Medium,
    High,
    VeryHigh,
    NeverStops
}

public class FireRateValues
{
    public float MinFireTime;
    public float MaxFireTime;
    public float MinWaitTime;
    public float MaxWaitTime;
}

public static class FireRateAgro
{
    /// <summary>
    /// Defines the likelyhood of the enemy shooting, and how long it will wait between shots, based on the enemies FireRateAgression enum
    /// </summary>
    public static Dictionary<FireRateAgression, FireRateValues> FireRates = new Dictionary<FireRateAgression, FireRateValues>()
    {
        { FireRateAgression.NonExistant, new FireRateValues() { MinFireTime = 0.1f, MaxFireTime = 0.5f, MinWaitTime = 4f, MaxWaitTime = 8f } },
        { FireRateAgression.VeryLow, new FireRateValues() { MinFireTime = 0.4f, MaxFireTime = 0.8f, MinWaitTime = 3f, MaxWaitTime = 7f } },
        { FireRateAgression.Low, new FireRateValues() { MinFireTime = 0.5f, MaxFireTime = 1f, MinWaitTime = 2f, MaxWaitTime = 4f } },
        { FireRateAgression.Medium, new FireRateValues() { MinFireTime = 1f, MaxFireTime = 2f, MinWaitTime = 0.5f, MaxWaitTime = 1f } },
        { FireRateAgression.High, new FireRateValues() { MinFireTime = 2f, MaxFireTime = 4f, MinWaitTime = 0.3f, MaxWaitTime = 0.5f } },
        { FireRateAgression.VeryHigh, new FireRateValues() { MinFireTime = 3f, MaxFireTime = 6f, MinWaitTime = 0.3f, MaxWaitTime = 0.4f } },
        { FireRateAgression.NeverStops, new FireRateValues() { MinFireTime = 5f, MaxFireTime = 10f, MinWaitTime = 0.3f, MaxWaitTime = 0.4f } }
    };
}
