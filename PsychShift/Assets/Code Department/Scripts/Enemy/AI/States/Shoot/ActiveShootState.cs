using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveShootState : IState
{
    private EnemyBrain brain;
    private AIAgression agression;
    public ActiveShootState(EnemyBrain brain, AIAgression agression)
    {
        this.brain = brain;
        this.agression = agression;
    }
    float shootForSeconds = 0f;
    public void OnEnter()
    {
        if(brain.player != null)
        {
            brain.aim.SetTarget(brain.player);
        }
        
        shootForSeconds = Time.time + UnityEngine.Random.Range(FireRateAgro.FireRates[agression.FireRateAgression].MinWaitTime, FireRateAgro.FireRates[agression.FireRateAgression].MaxWaitTime);
        brain.Animator.SetBool("shooting", true);
    }

    public void OnExit()
    {
        brain.Animator.SetBool("shooting", false);
        brain.aim.ResetAim();
    }

    public void Tick()
    {
        if(brain.player != null) 
        {
            Vector3 lookPos = brain.player.transform.position - brain.transform.position;
            lookPos.y = 0;
            if(lookPos != Vector3.zero)
            {
                Quaternion rotation = Quaternion.LookRotation(lookPos);
                brain.CharacterInfo.model.transform.rotation = Quaternion.Slerp(brain.CharacterInfo.model.transform.rotation, rotation, Time.deltaTime * 5f);
            }
            brain.CharacterInfo.gunHandler.EnemyShoot();
            brain.CharacterInfo.animator.SetBool("shooting", true);
            brain.aim.Aim();
        }
    }
    public Color GizmoColor()
    {
        return Color.red;
    }
    private bool DelayOver()
    {
        return Time.time >= shootForSeconds;
    }

    public Func<bool> IsDone() => () => DelayOver();
}
