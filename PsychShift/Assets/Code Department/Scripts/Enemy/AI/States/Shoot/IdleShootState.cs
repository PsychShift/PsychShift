using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleShootState : IState
{
    private EnemyBrain brain;
    private AIAgression agression;

    public IdleShootState(EnemyBrain brain, AIAgression agression)
    {
        this.brain = brain;
        this.agression = agression;
    }

    float waitToShoot = 0f;
    public void OnEnter()
    {
        brain.Animator.SetBool("shooting", false);
        waitToShoot = Time.time + UnityEngine.Random.Range(FireRateAgro.FireRates[agression.FireRateAgression].MinWaitTime, FireRateAgro.FireRates[agression.FireRateAgression].MaxWaitTime);
    }

    public void OnExit()
    {
        
    }

    public void Tick()
    {
        /* if(brain.player != null) 
        {
            Vector3 lookPos = brain.player.transform.position - brain.transform.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            brain.CharacterInfo.model.transform.rotation = Quaternion.Slerp(brain.CharacterInfo.model.transform.rotation, rotation, Time.deltaTime * 5f);
        } */
    }
    public Color GizmoColor()
    {
        // return pink
        return new Color(1f, 0.5f, 0.5f);
    }

    private bool DelayOver()
    {
        return Time.time >= waitToShoot;
    }

    public Func<bool> IsDone() => () => DelayOver();
}
