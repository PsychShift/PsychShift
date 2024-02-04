using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleShootState : IState
{
    private EnemyBrain brain;

    public IdleShootState(EnemyBrain brain)
    {
        this.brain = brain;
    }

    float waitToShoot = 0f;
    public void OnEnter()
    {
        //brain.Animator.SetBool("shooting", false);
        waitToShoot = Time.time + UnityEngine.Random.Range(FireRateAgro.FireRates[brain.agression.FireRateAgression].MinWaitTime, FireRateAgro.FireRates[brain.agression.FireRateAgression].MaxWaitTime);
        //brain.AnimMaster.StartCoroutine(brain.AnimMaster.SetWeightOverTime(0f, .2f));
    }

    public void OnExit()
    {
        //brain.AnimMaster.StopCoroutine(brain.AnimMaster.SetWeightOverTime(1f, .2f));
        //brain.AnimMaster.StartCoroutine(brain.AnimMaster.SetWeightOverTime(1f, .2f));
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
