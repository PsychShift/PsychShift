using System;
using UnityEditor.SceneManagement;
using UnityEngine;

public class EnemyBrainSelector : MonoBehaviour
{
    // Buttons to choose what enemy brain this character will use
    // Function to add a new enemy brain, it should take a parameter to decide what component to add

    public EBrainType enemyType;
    private EnemyBrain currentBrain;

    public void SwapBrain()
    {
        currentBrain = GetComponent<EnemyBrain>();
        CharacterBrainSwappingInfo oldInfo = new CharacterBrainSwappingInfo(currentBrain.agression);
        EnemyBrain newBrain = ChooseNewBrain(enemyType);
        if (newBrain == null) return;
        TransferBrainData(oldInfo);
    }

    private void TransferBrainData(CharacterBrainSwappingInfo oldInfo)
    {
        
    }

    private EnemyBrain ChooseNewBrain(EBrainType type)
    {
        switch (type)
        {
            case EBrainType.Stationary:
                Destroy(currentBrain);
                return gameObject.AddComponent<StationaryBrain>();
            case EBrainType.Patrol:
                Destroy(currentBrain);
                return gameObject.AddComponent<PatrolBrain>();
            case EBrainType.Chase:
                Destroy(currentBrain);
                return gameObject.AddComponent<ChaseBrain>();
            case EBrainType.Random:
                Destroy(currentBrain);
                return gameObject.AddComponent<RandomBrain>();
            default:
                Debug.LogError("Enemy (" + gameObject + ") doesn't have a brain");
                return null;
        }
    }
}

public enum EBrainType
{
    Stationary,
    Patrol,
    Chase,
    Random
}

public struct CharacterBrainSwappingInfo
{
    public AIAgression AIAgression { get; set; }

    public CharacterBrainSwappingInfo(AIAgression AIAgression)
    {
        this.AIAgression = AIAgression;
    }
}
