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
}
