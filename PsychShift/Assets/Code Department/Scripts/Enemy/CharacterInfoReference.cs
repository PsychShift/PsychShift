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
    public Transform cameraRoot;

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
    public CharacterInfo SetUp()
    {
        if(vCamPrefab == null) Debug.LogError("vCamPrefab is null, please fill in in editor. Otherwise the game won't work. Which is bad. Please fix it now. Thank you. Have you done it yet? Ok good. \n It's located at Assets/Code Department/Scripts/Player/Swapping");
        else
        {
            vCamParent = Instantiate(vCamPrefab);
            vCamParent.transform.SetParent(transform);
            vCamParent.SetActive(false);
            vCam = vCamParent.GetComponent<CinemachineVirtualCamera>();
        }

        _characterInfo = new CharacterInfo(this, vCam, movementStats, characterStats);
        vCam.Follow = characterInfo.cameraRoot;
        GetComponent<RigColliderManager>().SwapTag("Enemy");

        return _characterInfo;
    }


    public void ActivatePlayerAllAtOnce()
    {
        vCamParent.SetActive(true);
        characterInfo.model.GetComponent<ModelDisplay>().ActivateFirstPerson();
        characterInfo.enemyBrain.IsActive = false;
        characterInfo.agent.enabled = false;

        GetComponent<RigColliderManager>().SwapTag("Player");
        characterInfo.characterContainer.layer = LayerMask.NameToLayer("Player");
        characterInfo.characterContainer.tag = "Player";
    }

    public void DeactivatePlayerAllAtOnce()
    {
        characterInfo.model.GetComponent<ModelDisplay>().DeActivateFirstPerson();
        characterInfo.enemyBrain.IsActive = true;
        characterInfo.agent.enabled = true;

        GetComponent<RigColliderManager>().SwapTag("Enemy");
        characterInfo.characterContainer.tag = "Swapable";
        characterInfo.characterContainer.layer = LayerMask.NameToLayer("Character");
    }

    public void ActivateFirstPersonModel()
    {
        characterInfo.model.GetComponent<ModelDisplay>().ActivateFirstPerson();
    }
    public void ActivateThirdPersonModel()
    {
        characterInfo.model.GetComponent<ModelDisplay>().DeActivateFirstPerson();
    }

    public override string ToString()
    {
        return characterInfo.ToString();
    }
}
