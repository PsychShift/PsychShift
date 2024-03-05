using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class FirstEnemy : MonoBehaviour
{
    public PlayerStateMachine playerStateMachine { get; set; }

    public void Swap(Transform transform)
    {
        playerStateMachine.OnSwapPlayer -= Swap;
        EnemyBrain brain = GetComponent<EnemyBrain>();
        // remove all tags from the enemy
        brain.gameObject.tag = "Untagged";
        gameObject.layer = LayerMask.NameToLayer("Default");
        Destroy(brain);
    }
}
