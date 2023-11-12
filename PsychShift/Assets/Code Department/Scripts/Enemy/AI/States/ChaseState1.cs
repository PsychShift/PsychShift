using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState1 : IState
{
    private EnemyBrain brain;
    private AIAgression agression;

    private Player.CharacterInfo currentCharacterInfo;
    public ChaseState1(EnemyBrain brain, AIAgression agression)
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
        currentCharacterInfo = brain.CharacterInfo1;
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
            currentCharacterInfo.agent.SetDestination(brain.player.transform.position);
            yield return new WaitForSeconds(0.1f);
        }
    }

    
    

}
