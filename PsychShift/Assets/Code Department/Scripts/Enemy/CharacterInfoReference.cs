using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Player;
using CharacterInfo = Player.CharacterInfo;
using UnityEngine.AI;
using Guns.Demo;

[RequireComponent(typeof(CharacterController), typeof(EnemyGunSelector), typeof(NavMeshAgent))]
public class CharacterInfoReference : MonoBehaviour
{
    [Header("VERY IMPORTANT MUST FILL IN IN EDITOR")]
    [Tooltip("This is a thing that does stuff")]
    public GameObject vCamPrefab;

    [Header("Filled in in editor ATM")]
    public CharacterMovementStatsSO movementStats;
    public CharacterStatsSO characterStats;

    [Header("Filled in at runtime")]
    private CharacterInfo _characterInfo;
    public CharacterInfo characterInfo {
        get {
            if(_characterInfo == null)
            {
                SetUp();
            }
            return _characterInfo;
        }
    }

    [HideInInspector]public GameObject vCamParent;
    private CinemachineVirtualCamera vCam;
    private void SetUp()
    {
        if(vCamPrefab == null) Debug.LogError("vCamPrefab is null, please fill in in editor. Otherwise the game won't work. Which is bad. Please fix it now. Thank you. Have you done it yet? Ok good. \n It's located at Assets/Code Department/Scripts/Player/Swapping");
        else
        {
            vCamParent = Instantiate(vCamPrefab);
            vCamParent.transform.SetParent(transform);
            vCamParent.SetActive(false);
            vCam = vCamParent.GetComponent<CinemachineVirtualCamera>();
        }
        SetUpGun();

        _characterInfo = new CharacterInfo(gameObject, vCam, movementStats, characterStats);
        vCam.Follow = characterInfo.cameraRoot;
    }
    private void SetUpGun()
    {

    }

    public void ActivatePlayer()
    {
        
        characterInfo.model.GetComponent<ModelDisplay>().ActivateFirstPerson();
        characterInfo.enemyBrain.enabled = false;
        characterInfo.gunHandler.enabled = false;
        characterInfo.agent.enabled = false;

        characterInfo.characterContainer.layer = LayerMask.NameToLayer("Player");
        characterInfo.characterContainer.tag = "Player";
    }

    public void DeactivatePlayer()
    {
        characterInfo.model.GetComponent<ModelDisplay>().DeActivateFirstPerson();
        characterInfo.enemyBrain.enabled = true;
        characterInfo.gunHandler.enabled = true;
        characterInfo.agent.enabled = true;

        characterInfo.characterContainer.tag = "Swapable";
        characterInfo.characterContainer.layer = LayerMask.NameToLayer("Character");
    }

    public override string ToString()
    {
        return characterInfo.ToString();
    }
}
