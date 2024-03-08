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
        
        //brain.CharacterInfo.controller.transform.position = brain.CharacterInfo.model.transform.position; 
        rigColliderManager.EnableAnimator();
        rigColliderManager.Animator.Play("Stand Up");
        //brain.Animator.SetBool("Standup", true);
        animTime = OGanimTime;
        
        brain.CharacterInfo.controller.enabled = false;         
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
        //Debug.Log(animTime);
        if(animTime <= 0)
        {
            //Debug.Log("StandUpExit");
            //brain.Animator.SetBool("Standup", false);
            brain.Agent.enabled = true;
            brain.Agent.isStopped = false;
            
            brain.CharacterInfo.controller.enabled = true;
            isStanding = true;
            
        }
    }

    public Color GizmoColor()
    {
        return Color.white;
    }
}
