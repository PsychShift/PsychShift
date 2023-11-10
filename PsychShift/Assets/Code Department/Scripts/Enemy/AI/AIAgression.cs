using UnityEngine;

[CreateAssetMenu(fileName = "AIAgression", menuName = "AI/Agression", order = 1)]
public class AIAgression : ScriptableObject
{
    [Tooltip("How far the AI can see the player")]
    public float DetectionRange;
    public float SphereCastDetectionRadius;
    [Tooltip("How long the AI will be aggressive at the player after they are detected")]
    public float StopChasingTime;
    [Tooltip("The higher this number, the more often the AI will fire at the player")]
    public float FireRateTendency;


    [Tooltip("When the enemy is chasing the player, it should stop this distance away")]
    public float PlayerStoppingDistance;
    [Tooltip("The distance the enemy should stop at when patroling or guarding")]
    public float DestinationStoppingDistance;

    [Tooltip("If true, the enemy will take cover when it is chasing the player or running away.")]
    public bool TakesCover;
}
