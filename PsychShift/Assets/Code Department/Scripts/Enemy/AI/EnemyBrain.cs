using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using System;

using CharacterInfo = Player.CharacterInfo;
[RequireComponent(typeof(CharacterInfoReference))]
public abstract class EnemyBrain : MonoBehaviour
{
    [Tooltip("If this is true, the enemy will stand in one spot, if false, it will require a patrol path")]
    [SerializeField] protected bool isGaurd;

    public bool isActive = true;
    [SerializeField] protected LayerMask playerLayer;
    protected StateMachine.StateMachine stateMachine;
    public AIAgression agression;
    public CharacterInfo characterInfo;
    public bool isMelee;
    protected float attackRange;


    [HideInInspector] public GameObject player;
    /// <summary>
    /// Any variables that require initialization before a Func<bool> is used should be initialized here.
    /// Call this function in the Awake() method of the inheriting class.
    /// </summary> 
    protected void VariableSetup()
    {
        attackRange = isMelee ? 1f : 40f;
        characterInfo.agent.speed = characterInfo.movementStats.moveSpeed;
    }

    protected void AT(IState from, IState to, Func<bool> condition) => stateMachine.AddTransition(from, to, condition);

    public abstract void StateMachineSetup();

    protected Func<bool> PlayerInSight() => () => IsPlayerInSight();
    protected Func<bool> PlayerInSightWide() => () => IsPlayerInSightWideView();
    protected Func<bool> PlayerInAttackRange() => () => IsPlayerInRange();
    protected Func<bool> OutOfRangeForTooLong(float maxTimeOutOfSight) => () => IsPlayerOutOfRangeForTooLong(maxTimeOutOfSight);
    protected Func<bool> OutOfRangeForTooLongAndIsGuard(float maxTimeOutOfSight) => () => IsPlayerOutOfRangeForTooLong(maxTimeOutOfSight) && isGaurd;
    protected Func<bool> CanGuard() => () => !IsPlayerInSight() && isGaurd;

    float time = 0f;
    private bool IsPlayerOutOfRangeForTooLong(float maxTimeOutOfSight)
    {
        if(!IsPlayerInSight())
        {
            time += Time.deltaTime;
            if(time >= maxTimeOutOfSight)
                return true;

            return false;
        }
        else
        {
            time = 0f;
            return false;
        }
    }

    private bool IsPlayerInSight()
    {
        Physics.SphereCast(characterInfo.cameraRoot.position, agression.SphereCastDetectionRadius, 
        characterInfo.cameraRoot.forward, out RaycastHit hit, agression.DetectionRange, layerMask: playerLayer);
        if(hit.collider == null) return false;
        if(hit.collider.tag == "Player")
        {
            player = hit.collider.gameObject;
            return true;
        }
        return false;
    }
    private bool IsPlayerInSightWideView()
    {
        // Cast a sphere slightly to the left of transform.forward
        Vector3 leftDirection = Quaternion.Euler(0, -20, 0) * transform.forward;


        // Cast a sphere slightly to the right of transform.forward
        Vector3 rightDirection = Quaternion.Euler(0, 20, 0) * transform.forward;

        Physics.SphereCast(characterInfo.cameraRoot.position, agression.SphereCastDetectionRadius,
        characterInfo.cameraRoot.forward, out RaycastHit hit, agression.DetectionRange, layerMask: playerLayer);
        if (hit.collider == null) return false;
        if (hit.collider.tag == "Player")
        {
            player = hit.collider.gameObject;
            return true;
        }

        Physics.SphereCast(characterInfo.cameraRoot.position, agression.SphereCastDetectionRadius,
        leftDirection, out hit, agression.DetectionRange, layerMask: playerLayer);
        if (hit.collider == null) return false;
        if (hit.collider.tag == "Player")
        {
            player = hit.collider.gameObject;
            return true;
        }

        Physics.SphereCast(characterInfo.cameraRoot.position, agression.SphereCastDetectionRadius,
        rightDirection, out hit, agression.DetectionRange, layerMask: playerLayer);
        if (hit.collider == null) return false;
        if (hit.collider.tag == "Player")
        {
            player = hit.collider.gameObject;
            return true;
        }

        return false;
    }


    private bool IsPlayerInRange()
    {
        float distance = Vector3.Distance(transform.position, characterInfo.characterContainer.transform.position);
        return distance <= attackRange;
    }



    void OnDrawGizmos()
    {
        if(!Application.isPlaying || !isActive) return;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(characterInfo.cameraRoot.position, characterInfo.cameraRoot.position + (characterInfo.cameraRoot.forward * agression.SphereCastDetectionRadius));
        Gizmos.color = Color.blue;

    }

    public void CreateDebugSphere()
    {
        // create a sphere at the transform position
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = transform.position;
    }
}
