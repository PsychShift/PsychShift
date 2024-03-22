using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Assertions;
using StateMachine;

/// <summary>
/// <para>SpineControllerIK is a State machine that tells the spine chain ik how to behave. </para>
/// <para>It recieves neccesary information from the HangingAnimatorController master script through events.</para>
/// </summary> 
[RequireComponent(typeof(ChainIKConstraint), typeof(MultiRotationConstraint))]
public class SpineControllerIK : MonoBehaviour
{
    [SerializeField] private HangingAnimatorController animController;
    [SerializeField] public float leanHeightOffset = 1f;
    [SerializeField] private float maxLean = 1f;
    [SerializeField] private float leanTime = 0.1f; // You can experiment with different values
    [SerializeField] private float lookDownBodyRotationAmount = 60f;
    [SerializeField] private float lookDownRotationSpeed = 10f;

    [SerializeField] private float playerDistanceToLookDown = 20f;
    [SerializeField] private Transform chest;
    [SerializeField] private Transform neck;

    private ChainIKConstraint chainIK;
    private MultiRotationConstraint rotationConstraint;
    [SerializeField] private MultiAimConstraint neckLookAtIK;
    private Rig spineRig;
    private Transform spineTarget;
    private Vector3 localHomePos;

    private Transform playerTarget => animController.playerTarget;
    private bool Moving => animController.Velocity.magnitude > 0.01f;

    public Action onSpineRotateRequest;
    public Action resetSpineRotationRequest;

    
    void Awake()
    {
        
        SetUpComponents();
        ValidateConstraints();
        BuildStateMachine();
    }

    void Update()
    {
        //animStateMachine.Tick();
    }
    StateMachine.StateMachine animStateMachine;
    private void BuildStateMachine()
    {
        animStateMachine = new StateMachine.StateMachine();

        var idleSpine = new HangBossSpine_Idle_State(spineRig, animController, spineTarget, localHomePos, leanTime);
        var chaseSpine = new HangBossSpine_Chase_State(this, animController, spineTarget, localHomePos, maxLean, leanTime, leanHeightOffset);
        var lookDown = new HangBossSpine_LookDown_State(animController, spineTarget, lookDownBodyRotationAmount, lookDownRotationSpeed);
        var rotateChest = new HangBossSpine_RotateChest_State(animController, chest, neck);
        var doNothing = new DoNothing();
        //var resetChestRotation = new HangBossSpine_ResetChestRotation_State(animController, chest, neck);

        void AT(IState from, IState to, Func<bool> condition) => animStateMachine.AddTransition(from, to, condition);
        //void AAT(IState to, Func<bool> condition) => animStateMachine.AddAnyTransition(to, condition);
        //void AET(Action transitionRequest, IState to) => animStateMachine.AddTransitionOnEvent(ref transitionRequest, to);
        
        //AAT(lookDown, TargetClose());
        AT(chaseSpine, idleSpine, NotChasing());
        AT(idleSpine, chaseSpine, Chasing());
        AT(rotateChest, doNothing, rotateChest.IsDone);

        //AET(onSpineRotateRequest, rotateChest);
        onSpineRotateRequest = () => animStateMachine.EventTransition(rotateChest);
        //Debug.Log("hfuiiecwonfeuiown");       



        //Func<bool> TargetClose() => () => CloseEnoughToLookDown();
        Func<bool> Chasing() => () => Moving;
        Func<bool> NotChasing() => () => !Moving;
        //Func<bool> NotChasingAndNotClose() => () => !Moving && !CloseEnoughToLookDown();

        //animStateMachine.SetState(idleSpine);
    }    

    private bool CloseEnoughToLookDown()
    {
        Vector3 playerPos = playerTarget.position;
        playerPos.y = 0;
        Vector3 bossPos = animController.transform.position;
        bossPos.y = 0;

        return Vector3.Distance(playerPos, bossPos) < playerDistanceToLookDown;
    }

    private void ValidateConstraints()
    {
        Assert.IsNotNull(chainIK, "Spine Chain IK Constraint is not assigned.");
        Assert.IsNotNull(rotationConstraint, "Spine Rotation Constraint is not assigned.");
        Assert.IsNotNull(animController, "Master Animation Controller is not assigned");
    }
    private void SetUpComponents()
    {
        spineRig = GetComponentInParent<Rig>();
        chainIK = GetComponent<ChainIKConstraint>();
        rotationConstraint = GetComponent<MultiRotationConstraint>();
        spineTarget = chainIK.data.target;
        localHomePos = spineTarget.localPosition;
    }

    public void TurnOnNeckLookAtIK(bool on, float transitionTime)
    {
        if (on)
        {
            StartCoroutine(LerpFloatOverTime(0, 1, transitionTime, (result) => neckLookAtIK.weight = result));
        }
        else
        {
            StartCoroutine(LerpFloatOverTime(1, 0, transitionTime, (result) => neckLookAtIK.weight = result));
        }
    }

    IEnumerator LerpFloatOverTime(float startValue, float endValue, float duration, System.Action<float> resultCallback)
    {
        float startTime = Time.time;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float currentLerp = Mathf.Lerp(startValue, endValue, elapsedTime / duration);
            resultCallback.Invoke(currentLerp);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        resultCallback.Invoke(endValue);
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if(!Application.isPlaying) return;
        Gizmos.color = animStateMachine.GetGizmoColor();

        Gizmos.DrawSphere(spineTarget.position, 1f);
    }
    #endif

}

public class DoNothing : IState
{
    public Color GizmoColor()
    {
        return Color.white;
    }

    public void OnEnter()
    {
        
    }

    public void OnExit()
    {
        
    }

    public void Tick()
    {
        
    }
}

public class HangBossSpine_RotateChest_State : IState
{
    private HangingAnimatorController animController;
    private Transform chest;
    private Transform neck;

    private float rotateAmount => animController.SpineRotationAmount;
    public HangBossSpine_RotateChest_State(HangingAnimatorController animController, Transform chest, Transform neck)
    {
        this.animController = animController;
        this.chest = chest;
        this.neck = neck;
    }

    public Color GizmoColor()
    {
        return Color.yellow;
    }

    public void OnEnter()
    {
        isDone = false;
        animController.StartCoroutine(RotateChestOverTime(rotateAmount, 0.1f));
    }

    public void OnExit()
    {
        
    }

    public void Tick()
    {
        
    }

    IEnumerator RotateChestOverTime(float endValue, float duration)
    {
        float elapsedTime = 0f;
        Quaternion startRot = chest.rotation;
        Quaternion endRot = startRot * Quaternion.Euler(0f, endValue, 0f);
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            Quaternion rotation = Quaternion.Lerp(startRot, endRot, t);
            //chest.rotation = rotation;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        //chest.rotation = endRot;
        isDone = true;
    }

    bool isDone = false;
    public Func<bool> IsDone => () => isDone;
}

// Produce a state machine ? Behavior Tree
// If depending on the distance between the player and enemy, change the orientation strategy of the spine (ignore then height difference)
// If the player is far and the enemy is traveling towards the player, make it lean into the direction its moveing
// If the player is close, arch the back, rotate the neck to allow the head to look straight down at the player


// idle state

public class HangBossSpine_Idle_State : IState
{
    private Rig spineRig;
    private HangingAnimatorController animController;
    private Transform target;
    private Vector3 localHome;
    private float leanTime;
    public HangBossSpine_Idle_State(Rig spineRig, HangingAnimatorController animController, Transform target, Vector3 localHome, float leanTime)
    {
        // What do we need?
        this.spineRig = spineRig;
        // animator controller, this is where we can get most of the important context
        this.animController = animController;
        // target transform
        this.target = target;
        // Home position
        this.localHome = localHome;
        // any vars that control idle stuff
        this.leanTime = leanTime;
    }
    public Color GizmoColor()
    {
        return Color.white;
    }
    bool stateIsActive = false;
    public void OnEnter()
    {
        // Start co-routine to tween back to the home position
        stateIsActive = true;
        animController.StartCoroutine(ResetPosition());
    }

    public void OnExit()
    {
        stateIsActive = false;
        animController.StopCoroutine(ResetPosition());
        spineRig.weight = 1;
        
    }

    public void Tick()
    {
        // DO NOTHING
    }

    private IEnumerator ResetPosition()
    {
        float elapsedTime = 0f;
        Vector3 localStartPos = target.localPosition;

        while (elapsedTime < leanTime)
        {
            // Use Mathf.Sin to create an arc motion
            float t = elapsedTime / leanTime;
            float arcFactor = Mathf.Sin(t * Mathf.PI); // Adjust the factor based on your preference
            //target.localPosition = Vector3.Lerp(localStartPos, localHome, arcFactor);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        //target.localPosition = localHome;
        if (stateIsActive)
            yield return new WaitForSeconds(0.1f);

        spineRig.weight = 0;
    }
}

// Chase state

public class HangBossSpine_Chase_State : IState
{
    private SpineControllerIK spineController;
    private HangingAnimatorController animController;
    private Transform target;
    private Transform playerTarget => animController.playerTarget;
    private Vector3 localHome;
    private float maxLean, leanTime, leanHeightOffset;

    private bool inLeanCoroutine = false;
    public HangBossSpine_Chase_State(SpineControllerIK spineController, HangingAnimatorController animController, Transform target, Vector3 localHome, float maxLean, float leanTime, float leanHeightOffset)
    {
        this.spineController = spineController;
        this.animController = animController;
        this.target = target;
        this.localHome = localHome;
        this.maxLean = maxLean;
        this.leanTime = leanTime;
        this.leanHeightOffset = leanHeightOffset;
    }
    public Color GizmoColor()
    {
        return Color.red;
    }

    public void OnEnter()
    {
        // start lean process??
        inLeanCoroutine = true;
        animController.StartCoroutine(StartLean());
    }

    public void OnExit()
    {
        // reset some values?
        animController.StopCoroutine(StartLean());
        inLeanCoroutine = false;
    }

    public void Tick()
    {
        if(inLeanCoroutine) return;

        Vector3 targetPos = GetLeanPosition();
        Vector3 smoothedPos = Vector3.Lerp(target.localPosition, targetPos, 10f);

        // Update the spine target position
        target.localPosition = smoothedPos;
    }

    private Vector3 GetLeanPosition()
    {
        Vector3 playerPos = playerTarget.position;
        playerPos.y = 0;
        Vector3 bossPos = animController.transform.position;
        bossPos.y = 0;
        // Get direction to lean
        Vector3 dir = playerPos - bossPos;
        
        float leanAmount = Mathf.Clamp(animController.Velocity.magnitude, 0, maxLean);
        Vector3 leanPos = localHome + (-animController.transform.forward * leanAmount);
        leanPos.z = -spineController.leanHeightOffset;
        return leanPos;
    }

    private IEnumerator StartLean()
    {
        float elapsedTime = 0f;
        Vector3 localStartPos = target.localPosition;
        
        while(elapsedTime < leanTime)
        {
            //target.localPosition = Vector3.Lerp(localStartPos, GetLeanPosition(), elapsedTime / leanTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        inLeanCoroutine = false;
    }
}

// Look Down State

public class HangBossSpine_LookDown_State : IState
{
    private HangingAnimatorController animController;
    private Transform target;
    private float rotAmount, rotSpeed;
    public HangBossSpine_LookDown_State(HangingAnimatorController animController, Transform target, float rotAmount, float rotSpeed)
    {
        this.animController = animController;
        this.target = target;
        this.rotAmount = rotAmount;
        this.rotSpeed = rotSpeed;
    }
    public Color GizmoColor()
    {
        return Color.green;
    }

    public void OnEnter()
    {
        // calculate final position
    }

    public void OnExit()
    {
        // idk
    }

    public void Tick()
    {
        // tween to final position
    }
}