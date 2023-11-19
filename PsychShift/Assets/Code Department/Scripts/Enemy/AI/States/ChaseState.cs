using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : ShootingSuperState, ICoroutineRestarter
{
    private Player.CharacterInfo currentCharacterInfo;
    Transform gunParent;

    public ChaseState(EnemyBrain brain, AIAgression agression)
    {
        this.brain = brain;
        this.agression = agression;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        brain.CharacterInfo.agent.stoppingDistance = brain.agression.PlayerStoppingDistance;
        currentCharacterInfo = brain.CharacterInfo;
        brain.StartCoroutine(ChasePlayer());
    }

    public override void OnExit()
    {
        base.OnExit();
        brain.CharacterInfo.agent.stoppingDistance = brain.agression.DestinationStoppingDistance;
        brain.StopCoroutine(ChasePlayer());
    }

    public void RestartCoroutine()
    {
        brain.StartCoroutine(ChasePlayer());
    }

    public override void Tick()
    {
        base.Tick();
        // aim at the player based on the gun forward
        /* Vector3 direction = brain.player.position - gunParent.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        gunParent.rotation = Quaternion.Lerp(gunParent.rotation, lookRotation, Time.deltaTime * 10f); */

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
