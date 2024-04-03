using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyBrain : EnemyBrain
{
    protected override void SetUp()
    {
        Agent.enabled = true;
        StartCoroutine(WaitPlease());
    }
    public override void StateMachineSetup()
    {
        
    }
}
