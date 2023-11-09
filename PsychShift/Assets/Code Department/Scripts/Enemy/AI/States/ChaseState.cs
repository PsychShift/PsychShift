using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IState
{
    private EnemyBrain brain;
    private AIAgression agression;
    public ChaseState(EnemyBrain brain, AIAgression agression)
    {
        this.brain = brain;
        this.agression = agression;
    }

    public Color GizmoColor()
    {
        return Color.red;
    }

    public void OnEnter()
    {
        brain.StartCoroutine(ChasePlayer());
    }

    public void OnExit()
    {
        brain.StopCoroutine(ChasePlayer());
    }

    public void Tick()
    {
        
    }

    private IEnumerator ChasePlayer()
    {
        while (true)
        {
            brain.CharacterInfo.agent.SetDestination(brain.player.transform.position);
            yield return new WaitForSeconds(0.5f);
        }
    }

}
