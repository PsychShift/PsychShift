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
    public EEnemyModifier modifier = EEnemyModifier.None;
    [HideInInspector] public EnemyBrain currentBrain;

    public void SwapBrain(GunScriptableObject gun, EBrainType brainType, EEnemyModifier modifier, AIAgression agression)
    {
        gunType = gun;
        enemyType = brainType;
        this.modifier = modifier;
        currentBrain = GetComponent<EnemyBrain>();
        CharacterBrainSwappingInfo oldInfo = new CharacterBrainSwappingInfo(agression);
        EnemyBrain newBrain = ChooseNewBrain(brainType);
        if (newBrain == null) return;
        currentBrain = newBrain;
        TransferBrainData(oldInfo, modifier);

        // Now do gun swap and model changes
        EnemyGunSelector gunSelector = GetComponent<EnemyGunSelector>();

        gunSelector.StartGun = gun;
        gunSelector.DespawnActiveGun();
        gunSelector.SetupGun(gun);
        SwapModel(gun, modifier);
    }
    public void SwapBrain(GunScriptableObject g, EEnemyModifier modifier)
    {
        currentBrain = GetComponent<EnemyBrain>();
        CharacterBrainSwappingInfo oldInfo = new CharacterBrainSwappingInfo(currentBrain.agression);
        EnemyBrain newBrain = ChooseNewBrain(enemyType);
        if (newBrain == null) return;
        currentBrain = newBrain;
        TransferBrainData(oldInfo, modifier);

        // Now do gun swap and model changes
        EnemyGunSelector gunSelector = GetComponent<EnemyGunSelector>();

        gunSelector.StartGun = g;
        SwapModel(g, modifier);
    }
    private void SwapModel(GunScriptableObject g, EEnemyModifier modifier)
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
        TransferBrainData(oldInfo, modifier);

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
            case GunType.ShotGun:
                return "ShotGun";
            case GunType.SMG:
                return "SMG";
            case GunType.FrogGun:
                return "Frog";
            case GunType.Minigun:
                return "Minigun";
            case GunType.SaltGun:
                return "SaltGun";
            case GunType.Revolver:
                return "Revolver";
            case GunType.RocketLauncher:
                return "RocketLauncher";
            case GunType.Crossbow:
                return "Crossbow";
            case GunType.None:
                return "None";
            default:
                return "None";
        }
    }


    public string ModifierName(EEnemyModifier modifier)
    {
        switch (modifier)
        {
            case EEnemyModifier.NonSwap:
                return "NonSwap";
            case EEnemyModifier.Keycard:
                return "Keycard";
            case EEnemyModifier.Explosive:
                return "Explosive";
            case EEnemyModifier.None:
                return "";
            default:
                return "";
        }
    }


    private void TransferBrainData(CharacterBrainSwappingInfo oldInfo, EEnemyModifier modifier)
    {
        currentBrain.SetUpBrainSwap(oldInfo, new EEnemyModifier[] { modifier });
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
            case EBrainType.FinalBoss:
                DestroyImmediate(currentBrain);
                return gameObject.AddComponent<BossFightBrain>();
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
    Random,
    FinalBoss
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
