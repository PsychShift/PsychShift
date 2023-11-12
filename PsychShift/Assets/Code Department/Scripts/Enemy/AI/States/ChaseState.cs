using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : ShootingSuperState
{
    private Player.CharacterInfo currentCharacterInfo;
    public ChaseState(EnemyBrain brain, AIAgression agression)
    {
        this.brain = brain;
        this.agression = agression;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        currentCharacterInfo = brain.CharacterInfo;
        brain.StartCoroutine(ChasePlayer());
    }

    public override void OnExit()
    {
        base.OnExit();
        brain.StopCoroutine(ChasePlayer());
    }

    public override void Tick()
    {
        base.Tick();
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
