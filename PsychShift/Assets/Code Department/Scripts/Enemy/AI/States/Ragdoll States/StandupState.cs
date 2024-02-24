using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandupState : IState
{
    RigColliderManager rigColliderManager;
    EnemyBrain brain;
    float OGanimTime = 3f;
    float animTime;
    public bool isStanding = false;
    public StandupState(EnemyBrain brain, RigColliderManager rigColliderManager)
    {
        this.brain = brain;
        this.rigColliderManager = rigColliderManager;
    }
    public void OnEnter()
    {
        Debug.Log("StandUp");
        //brain.CharacterInfo.controller.transform.position = brain.CharacterInfo.model.transform.position; 
        rigColliderManager.EnableAnimator();
        brain.Animator.SetBool("Standup", true);
        animTime = OGanimTime;
        brain.gameObject.transform.position = brain.CharacterInfo.model.transform.position;
        
    }

    public void OnExit()
    {
        /* Debug.Log("StandUpExit");
        brain.Animator.SetBool("Standup", false);
        brain.Agent.isStopped = true;
        //brain.CharacterInfo.controller.transform.position = brain.CharacterInfo.model.transform.position; 
        brain.CharacterInfo.controller.enabled = true; */
        
    }

    public void Tick()
    {
        animTime-=Time.deltaTime;
        if(animTime <= 0)
        {
            isStanding = true;
            Debug.Log("StandUpExit");
            brain.Animator.SetBool("Standup", false);
            brain.Agent.isStopped = true;
            brain.CharacterInfo.controller.enabled = true;
        }
    }

    public Color GizmoColor()
    {
        return Color.white;
    }
}
