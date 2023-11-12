using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtCoverState1 : ShootingSuperState1
{
    public AtCoverState1(EnemyBrain brain, AIAgression agression)
    {
        SetUp(brain, agression);
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
        if(stateMachine._currentState is ActiveShootState1)
        {
            brain.CharacterInfo1.animator.SetFloat("cover", 0);
        }
        else
        {
            brain.CharacterInfo1.animator.SetFloat("cover", 1);
        }
    }

}
