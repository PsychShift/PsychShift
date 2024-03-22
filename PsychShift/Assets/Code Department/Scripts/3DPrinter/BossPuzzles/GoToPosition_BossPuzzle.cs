using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoToPosition_BossPuzzle : AbstractBossPuzzle
{
    [SerializeField] private NavMeshAgent bossAgent;
    [SerializeField] private Transform target;
    public override void OnHealthGateReached()
    {
        bossAgent.SetDestination(target.position);
    }
}
