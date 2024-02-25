using System;
using System.Collections.Generic;
using System.Linq;
using Guns;
using Guns.Demo;
using Guns.Modifiers;
using UnityEngine;

public class EnemyBrainSelector : MonoBehaviour
{
    // Buttons to choose what enemy brain this character will use
    // Function to add a new enemy brain, it should take a parameter to decide what component to add

    public EBrainType enemyType;
    public GunScriptableObject gunType;
    [Header("Doesn't do anything yet")]
    public EEnemyModifier[] modifiers = new EEnemyModifier[] { EEnemyModifier.None };
    private EnemyBrain currentBrain;

    public void SwapBrain(GunScriptableObject gun, EBrainType brainType, EEnemyModifier[] modifiers, AIAgression agression, bool spawnerEnemy = false)
    {
        gunType = gun;
        enemyType = brainType;
        this.modifiers = modifiers;
        currentBrain = GetComponent<EnemyBrain>();
        CharacterBrainSwappingInfo oldInfo = new CharacterBrainSwappingInfo(agression);
        EnemyBrain newBrain = ChooseNewBrain(brainType);
        if (newBrain == null) return;
        currentBrain = newBrain;
        TransferBrainData(oldInfo, modifiers);

        // Now do gun swap and model changes
        EnemyGunSelector gunSelector = GetComponent<EnemyGunSelector>();

        gunSelector.StartGun = gun;
        gunSelector.DespawnActiveGun();
        gunSelector.SetupGun(gun);
        SwapModel(gun, modifiers);
        currentBrain.SpawnerEnemy = spawnerEnemy;
    }
    public void SwapBrain(GunScriptableObject g, EEnemyModifier[] modifiers)
    {
        currentBrain = GetComponent<EnemyBrain>();
        CharacterBrainSwappingInfo oldInfo = new CharacterBrainSwappingInfo(currentBrain.agression);
        EnemyBrain newBrain = ChooseNewBrain(enemyType);
        if (newBrain == null) return;
        currentBrain = newBrain;
        TransferBrainData(oldInfo, modifiers);

        // Now do gun swap and model changes
        EnemyGunSelector gunSelector = GetComponent<EnemyGunSelector>();

        gunSelector.StartGun = g;
        SwapModel(g, modifiers);
    }
    private void SwapModel(GunScriptableObject g, EEnemyModifier[] modifiers)
    {
        // Given gun type and enemy modifier, select a file name to load a model

        string gunTypeName = GunName(g);
        string modifierName = ModifierName(modifiers);

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
        TransferBrainData(oldInfo, modifiers);

        // Now do gun swap and model changes
        EnemyGunSelector gunSelector = GetComponent<EnemyGunSelector>();

        gunSelector.StartGun = gunType;
        SwapModel();
    }
    

    private void SwapModel()
    {
        // Given gun type and enemy modifier, select a file name to load a model

        string gunTypeName = GunName(gunType);
        string modifierName = ModifierName(modifiers);

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
            case GunType.ShotGun:
                return "ShotGun";
            case GunType.SMG:
                return "SMG";
            case GunType.FrogGun:
                return "Frog";
            case GunType.Minigun:
                return "Minigun";
            case GunType.None:
                return "None";
            default:
                return "None";
        }
    }


    public string ModifierName(EEnemyModifier[] modifiers)
    {
        // Sort the list by the integer value of the enum
        var sortedModifiers = modifiers.OrderBy(x => (int)x);

        string names = "";
        foreach (var modifier in sortedModifiers)
        {
            switch (modifier)
            {
                case EEnemyModifier.NonSwap:
                    names += "NonSwap";
                    break;
                case EEnemyModifier.Keycard:
                    names += "Keycard";
                    break;
                case EEnemyModifier.Explosive:
                    names += "Explosive";
                    break;
                case EEnemyModifier.None:
                    names += "";
                    break;
                default:
                    names += "";
                    break;
            }
        }
        return names;
    }


    private void TransferBrainData(CharacterBrainSwappingInfo oldInfo, EEnemyModifier[] modifiers)
    {
        currentBrain.SetUpBrainSwap(oldInfo, modifiers);
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
