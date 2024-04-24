using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Player;
using CharacterInfo = Player.CharacterInfo;
using UnityEngine.AI;
using Guns.Demo;
using Unity.VisualScripting;

[RequireComponent(typeof(CharacterController), typeof(EnemyGunSelector), typeof(NavMeshAgent))]
public class CharacterInfoReference : MonoBehaviour
{
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
        if(vCam == null)
        {
            if(PlayerFollowCameraReference.Instance != null)
            {
                vCamParent = Instantiate(PlayerFollowCameraReference.Instance.Prefab);
            }
            else
                vCamParent = Instantiate(GameAssets.Instance.level1VCamPrefab);
            vCamParent.transform.SetParent(transform);
            vCam = vCamParent.GetComponent<CinemachineVirtualCamera>();
        }
        vCamParent.SetActive(false);

        _characterInfo = new CharacterInfo(this, vCam, movementStats, characterStats);
        vCam.Follow = characterInfo.cameraRoot;
        GetComponent<RigColliderManager>().SwapTag("Enemy");

        return _characterInfo;
    }


    public void ActivatePlayerAllAtOnce()
    {
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
        characterInfo.enemyBrain.IsActive = false;

        characterInfo.agent.enabled = false;

        if (!IsAgentOnNavMesh())
        {
            characterInfo.enemyBrain.SetRagDollState();
        }
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

    private const float onMeshThreshold = 15f;
    private bool IsAgentOnNavMesh()
    {
        Vector3 agentPosition = characterInfo.characterContainer.transform.position;

        // Check for nearest point on navmesh to agent, within onMeshThreshold
        if (NavMesh.SamplePosition(agentPosition, out NavMeshHit hit, onMeshThreshold, NavMesh.AllAreas))
        {
            // Check if the positions are vertically aligned
            if (Mathf.Approximately(agentPosition.x, hit.position.x)
                && Mathf.Approximately(agentPosition.z, hit.position.z))
            {
                // Lastly, check if object is below navmesh
                return agentPosition.y >= hit.position.y;
            }
        }

        return false;
    }

    public override string ToString()
    {
        return characterInfo.ToString();
    }
}

