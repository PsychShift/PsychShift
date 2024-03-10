using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveShootState : IState
{
    private EnemyBrain brain;
    Vector3 defaultGunPosition;
    public ActiveShootState(EnemyBrain brain)
    {
        this.brain = brain;
        defaultGunPosition = brain.AnimMaster.GetDefaultGunPosition();
    }
    float shootForSeconds = 0f;
    public void OnEnter()
    {        
        shootForSeconds = Time.time + UnityEngine.Random.Range(FireRateAgro.FireRates[(int)brain.agression.FireRateAgression].MinWaitTime, FireRateAgro.FireRates[(int)brain.agression.FireRateAgression].MaxWaitTime);
        brain.AnimMaster.SetGunHandPosition(defaultGunPosition);
        brain.Animator.SetBool("shooting", true);
        brain.AnimMaster.StartCoroutine(brain.AnimMaster.SetWeightOverTime(1f, .2f));
    }

    public void OnExit()
    {
        brain.Animator.SetBool("shooting", false);
        //brain.AnimMaster.StopCoroutine(brain.AnimMaster.SetWeightOverTime(1f, .2f));
        //brain.AnimMaster.StartCoroutine(brain.AnimMaster.SetWeightOverTime(0f, .2f));
        //brain.AnimMaster.SetGunHandPosition(defaultGunPosition);
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
            //brain.AnimMaster.WeaponAim();
            brain.CharacterInfo.gunHandler.EnemyShoot();
            //brain.CharacterInfo.animator.SetBool("shooting", true);
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
