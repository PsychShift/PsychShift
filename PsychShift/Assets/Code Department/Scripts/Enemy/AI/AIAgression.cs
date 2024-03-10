using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AIAgression", menuName = "AI/Agression", order = 1)]
public class AIAgression : ScriptableObject
{
    [Tooltip("How far the AI can see the player")]
    public float DetectionRange;

    [Tooltip("Determines how often the AI will shoot, for how long, and how long it will wait between shots")]
    public FireRateAgression FireRateAgression;


    [Tooltip("When the enemy is chasing the player, it should stop this distance away")]
    public float PlayerStoppingDistance;

    [Tooltip("Sometimes these guys dilly dally. This determines about how long they should. The x is the min, the y is the max")]
    public Vector2 WaitAroundTime = new Vector2(2f, 7f);
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
    public static FireRateValues[] FireRates = new FireRateValues[]
    {
        new () { MinFireTime = 0.1f, MaxFireTime = 0.5f, MinWaitTime = 4f, MaxWaitTime = 8f }, //FireRateAgression.NonExistant
        new () { MinFireTime = 0.4f, MaxFireTime = 0.8f, MinWaitTime = 3f, MaxWaitTime = 7f }, //FireRateAgression.VeryLow
        new () { MinFireTime = 0.5f, MaxFireTime = 1f, MinWaitTime = 2f, MaxWaitTime = 4f },   //FireRateAgression.Low
        new () { MinFireTime = 1f, MaxFireTime = 2f, MinWaitTime = 0.5f, MaxWaitTime = 1f },   //FireRateAgression.Medium
        new () { MinFireTime = 2f, MaxFireTime = 4f, MinWaitTime = 0.3f, MaxWaitTime = 0.5f }, //FireRateAgression.High
        new () { MinFireTime = 3f, MaxFireTime = 6f, MinWaitTime = 0.3f, MaxWaitTime = 0.4f }, //FireRateAgression.VeryHigh
        new () { MinFireTime = 5f, MaxFireTime = 10f, MinWaitTime = 0.3f, MaxWaitTime = 0.4f } //FireRateAgression.NeverStops
    };
}
