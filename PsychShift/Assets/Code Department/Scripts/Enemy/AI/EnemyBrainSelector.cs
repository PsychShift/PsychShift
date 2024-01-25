using System;
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
        currentBrain = newBrain;
        TransferBrainData(oldInfo);
    }

    private void TransferBrainData(CharacterBrainSwappingInfo oldInfo)
    {
        currentBrain.SetUpBrainSwap(oldInfo);
    }

    private EnemyBrain ChooseNewBrain(EBrainType type)
    {
        switch (type)
        {
            case EBrainType.Stationary:
                DestroyImmediate(currentBrain);
                return gameObject.AddComponent<StationaryBrain>();
            case EBrainType.Patrol:
                DestroyImmediate(currentBrain);
                return gameObject.AddComponent<PatrolBrain>();
            case EBrainType.Chase:
                DestroyImmediate(currentBrain);
                return gameObject.AddComponent<ChaseBrain>();
            case EBrainType.Random:
                DestroyImmediate(currentBrain);
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
