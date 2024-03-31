using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtCoverState : ShootingSuperState
{
    public AtCoverState(EnemyBrain brain)
    {
        SetUp(brain);
    }
    public override void OnEnter()
    {
        base.OnEnter();
        // set animator bool "combat" to true
    }

    public override void OnExit()
    {
        base.OnExit();
        // set animator bool "combat" to false
    }

    public override void Tick()
    {
        base.Tick();
        /* if(stateMachine._currentState is ActiveShootState)
        {
            brain.CharacterInfo.animator.SetFloat("cover", 0);
        }
        else
        {
            brain.CharacterInfo.animator.SetFloat("cover", 1);
        } */
    }

}
