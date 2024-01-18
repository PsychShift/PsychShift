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
        EnemyBrain newBrain = ChooseNewBrain(enemyType);

        TransferBrainData(newBrain, currentBrain);
    }

    private void TransferBrainData(EnemyBrain to, EnemyBrain from)
    {
        
    }

    private EnemyBrain ChooseNewBrain(EBrainType type)
    {
        switch (type)
        {
            case EBrainType.Stationary:
                return gameObject.AddComponent<StationaryBrain>();
            case EBrainType.Patrol:
                return gameObject.AddComponent<PatrolBrain>();
            case EBrainType.Chase:
                return gameObject.AddComponent<ChaseBrain>();
            case EBrainType.Random:
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
