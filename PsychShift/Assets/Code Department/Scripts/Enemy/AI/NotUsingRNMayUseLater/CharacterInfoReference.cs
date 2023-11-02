using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using CharacterInfo = Player.CharacterInfo;
public class CharacterInfoReference : MonoBehaviour
{
    [Header("Filled in in editor ATM")]
    public CharacterMovementStatsSO movementStats;
    public CharacterStatsSO characterStats;

    [Header("Filled in at runtime")]
    public CharacterInfo characterInfo;


    void Awake()
    {
        characterInfo = new CharacterInfo(gameObject, movementStats, characterStats);
    }

    public void ActivateCharacter()
    {
        characterInfo.model.GetComponent<ModelDisplay>().ActivateFirstPerson();
        characterInfo.characterContainer.GetComponent<EnemyBrain>().enabled = false;
        characterInfo.characterContainer.GetComponent<EnemyBrain>().isActive = false;
        characterInfo.agent.enabled = false;

        characterInfo.characterContainer.layer = LayerMask.NameToLayer("Player");
        characterInfo.characterContainer.tag = "Player";
    }

    public void DeactivateCharacter()
    {
        characterInfo.model.GetComponent<ModelDisplay>().DeActivateFirstPerson();
        characterInfo.characterContainer.GetComponent<EnemyBrain>().enabled = true;
        characterInfo.characterContainer.GetComponent<EnemyBrain>().isActive = true;
        characterInfo.agent.enabled = true;

        characterInfo.characterContainer.tag = "Swapable";
        characterInfo.characterContainer.layer = LayerMask.NameToLayer("Character");
    }
}
