using UnityEngine;

public class FollowDebugBrain : EnemyBrain
{
    public Transform target;
    protected override void SetUp()
    {
        VariableSetup();
        StateMachineSetup();
    }

    void Update()
    {
        if(CharacterInfo.agent.enabled && IsActive)
            stateMachine.Tick();
    }

    public override void StateMachineSetup()
    {
        stateMachine = new StateMachine.StateMachine();


        var followState = new FollowState(this, agression, target);
        
        stateMachine.SetState(followState, true);
    }

    void OnDrawGizmos()
    {
        if (stateMachine == null) return;
        Gizmos.color = stateMachine.GetGizmoColor();
        Gizmos.DrawSphere(transform.position + Vector3.up * 6, 0.4f);     
    }
}

public class FollowState : IState
{
    private EnemyBrain brain;
    private AIAgression agression;

    Transform target;

    public FollowState(EnemyBrain enemy, AIAgression agression, Transform target)
    {
        brain = enemy;
        this.agression = agression;
        this.target = target;
    }

    public void Tick()
    {
        brain.CharacterInfo.agent.SetDestination(target.position);
        brain.Animator.SetFloat("speed", brain.CharacterInfo.agent.velocity.magnitude);
    }

    public void OnEnter()
    {
        
    }

    public void OnExit()
    {
        
    }

    public Color GizmoColor()
    {
        return Color.green;
    }
}
