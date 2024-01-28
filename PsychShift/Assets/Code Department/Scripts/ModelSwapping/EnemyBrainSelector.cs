using System;
using Guns;
using Guns.Demo;
using UnityEngine;

public class EnemyBrainSelector : MonoBehaviour
{
    // Buttons to choose what enemy brain this character will use
    // Function to add a new enemy brain, it should take a parameter to decide what component to add

    public EBrainType enemyType;
    public GunScriptableObject gunType;
    [Header("Doesn't do anything yet")]
    public EEnemyModifier modifier = EEnemyModifier.None;
    private EnemyBrain currentBrain;

    public void SwapBrain(GunScriptableObject g)
    {
        currentBrain = GetComponent<EnemyBrain>();
        CharacterBrainSwappingInfo oldInfo = new CharacterBrainSwappingInfo(currentBrain.agression);
        EnemyBrain newBrain = ChooseNewBrain(enemyType);
        if (newBrain == null) return;
        currentBrain = newBrain;
        TransferBrainData(oldInfo);

        // Now do gun swap and model changes
        EnemyGunSelector gunSelector = GetComponent<EnemyGunSelector>();

        gunSelector.StartGun = g;
        SwapModel(g);
    }
    private void SwapModel(GunScriptableObject g)
    {
        // Given gun type and enemy modifier, select a file name to load a model

        string gunTypeName = GunName(g);
        string modifierName = ModifierName(modifier);

        string fileName = gunTypeName + modifierName + "_EnemyModel";
        //Debug.Log(fileName);
        ModelSaving modelSaving = GetComponentInChildren<ModelSaving>();

        if(!modelSaving.Load(fileName, true))
        {
            Debug.LogError
            ("File not found, make sure the variables are filled out\nMake sure to check " 
            + Application.dataPath + 
            "/Design Department/CHARACTERS/ENEMY/EnemyModelPrefabs to see if the file exists there");
        }
    }

    public void SwapBrain()
    {
        currentBrain = GetComponent<EnemyBrain>();
        CharacterBrainSwappingInfo oldInfo = new CharacterBrainSwappingInfo(currentBrain.agression);
        EnemyBrain newBrain = ChooseNewBrain(enemyType);
        if (newBrain == null) return;
        currentBrain = newBrain;
        TransferBrainData(oldInfo);

        // Now do gun swap and model changes
        EnemyGunSelector gunSelector = GetComponent<EnemyGunSelector>();

        gunSelector.StartGun = gunType;
        SwapModel();
    }
    

    private void SwapModel()
    {
        // Given gun type and enemy modifier, select a file name to load a model

        string gunTypeName = GunName(gunType);
        string modifierName = ModifierName(modifier);

        string fileName = gunTypeName + modifierName + "_EnemyModel";
        //Debug.Log(fileName);
        ModelSaving modelSaving = GetComponentInChildren<ModelSaving>();

        if(!modelSaving.Load(fileName, true))
        {
            Debug.LogError
            ("File not found, make sure the variables are filled out\nMake sure to check " 
            + Application.dataPath + 
            "/Design Department/CHARACTERS/ENEMY/EnemyModelPrefabs to see if the file exists there");
        }
    }

    public string GunName(GunScriptableObject gun)
    {
        if(gun == null)
        {
            Debug.LogError("No gun is assigned to this character");
            return "";
        }
        switch(gun.Type)
        {
            case GunType.Rifle:
                return "Rifle";
            case GunType.Pistol:
                return "Pistol";
            case GunType.FrogGun:
                return "Frog";
            case GunType.None:
                return "None";
            default:
                return "None";
        }
    }


    public string ModifierName(EEnemyModifier modifier)
    {
        switch(modifier)
        {
            case EEnemyModifier.None:
                return "";
            case EEnemyModifier.NonSwap:
                return "NonSwap";
            case EEnemyModifier.Keycard:
                return "Keycard";
            case EEnemyModifier.Explosive:
                return "Explosive";
            default:
                return "";
        }
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

public enum EEnemyModifier
{
    None,
    NonSwap,
    Keycard,
    Explosive
}

public struct CharacterBrainSwappingInfo
{
    public AIAgression AIAgression { get; set; }

    public CharacterBrainSwappingInfo(AIAgression AIAgression)
    {
        this.AIAgression = AIAgression;
    }
}
