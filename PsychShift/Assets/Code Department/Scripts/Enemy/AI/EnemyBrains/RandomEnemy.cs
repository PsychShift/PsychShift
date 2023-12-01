using UnityEngine;

public class RandomEnemy : EnemyBrain
{

    protected override void SetUp()
    {
        VariableSetup();
        StateMachineSetup();
    }

    public override void StateMachineSetup()
    {
        stateMachine = new StateMachine.StateMachine();


        var pickRandom = new RandomLocationState(this, agression);
        var lookAround = new LookAroundState(this, agression);
        var chaseState = new ChaseState(this, agression);

        stateMachine.AddTransition(pickRandom, lookAround, pickRandom.IsDone());
        stateMachine.AddTransition(lookAround, pickRandom, lookAround.IsDone());

        stateMachine.AddTransition(pickRandom, chaseState, PlayerInSight());
        stateMachine.AddTransition(lookAround, chaseState, PlayerInSight());

        stateMachine.AddAnyTransition(chaseState, WasDamaged());

        stateMachine.SetState(pickRandom, true);
    }

    void Update()
    {
        if(IsActive)
            stateMachine.Tick();
    }

    void OnDrawGizmos()
    {
        if (stateMachine == null) return;
        Gizmos.color = stateMachine.GetGizmoColor();
        Gizmos.DrawSphere(transform.position + Vector3.up * 6, 0.4f);     
    }
}
