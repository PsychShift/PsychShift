using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootState : IState
{
    private EnemyBrain brain;
    private AIAgression agression;
    public ShootState(EnemyBrain brain, AIAgression agression)
    {
        this.brain = brain;
        this.agression = agression;
    }

    public void OnEnter()
    {
        Debug.Log("Enter ShootState");
    }

    public void OnExit()
    {
        Debug.Log("Exit ShootState");
        brain.CharacterInfo.animator.SetBool("shooting", false);
    }

    public void Tick()
    {
        if(brain.player == null) return;

        Vector3 lookPos = brain.player.transform.position - brain.transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        brain.transform.rotation = Quaternion.Slerp(brain.transform.rotation, rotation, Time.deltaTime * 5f);

        brain.CharacterInfo.gunHandler.EnemyShoot();
        brain.CharacterInfo.animator.SetBool("shooting", true);
    }
    public Color GizmoColor()
    {
        return Color.red;
    }
}
