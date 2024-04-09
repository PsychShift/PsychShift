using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LaunchedRagdollState : IState
{
    public Vector3 force;
    EnemyBrain brain;
    Rigidbody[] rigidbodies;
    RigColliderManager rigColliderManager;
    TempGravity tempGravity;
    public bool IsDead;
    private bool hitGround;
    private const float onGroundForSeconds = 3f;
    
    float elapsedTime = 0;

    public bool isDone = false;
    public System.Func<bool> IsDone => () => isDone;
    private readonly int len;
    public LaunchedRagdollState(EnemyBrain brain, RigColliderManager rigColliderManager, TempGravity tempGravity, Rigidbody[] rigidbodies)
    {
        this.brain = brain;
        this.rigidbodies = rigidbodies;
        this.rigColliderManager = rigColliderManager;
        this.tempGravity = tempGravity;
        len = rigidbodies.Length;
    }
    public Color GizmoColor()
    {
        return Color.red;
    }

    public void OnEnter()
    {
        hitGround = false;
        isDone = false;
        brain.Agent.enabled = false;
        tempGravity.enabled = true;
        rigColliderManager._elapsedResetBonesTime = 0;
        rigColliderManager.EnableRagdoll();

        brain.StartCoroutine(ApplyForceGradually());
        //rigidbodies[0].AddExplosionForce(100, brain.Model.position - brain.Model.transform.forward * 2, 5, 10);
        //rigidbodies[0].AddForce(force, ForceMode.Impulse);
        /* for(int i = 0; i < len; i++)
        {
            rigidbodies[i].AddForce(force, ForceMode.Impulse);
        } */
    }

    public void OnExit()
    {
        //Debug.Log("Ragdoll exit");
        brain.Model.transform.parent = null;
        brain.CharacterInfo.controller.enabled = false;
        brain.Agent.enabled = true;
        brain.Agent.isStopped = false;
        
        
        /* AlignRotationToHips();
        AlignPositionToHips(); */
        brain.transform.position = brain.Model.transform.GetChild(1).position;
        brain.CharacterInfo.controller.enabled = true;
        brain.Model.transform.parent = brain.transform;
        tempGravity.enabled = false;
    }

    public void Tick()
    {
        if(!hitGround && GroundedCheck())
        {
            hitGround = true;
            elapsedTime = 0;
        }
        if(hitGround)
        {
            hitGround = GroundedCheck();
            if(elapsedTime > onGroundForSeconds)
            {

                brain.CharacterInfo.controller.enabled = false;
                isDone = true;
            }
            elapsedTime += Time.deltaTime;
        }
        else if(elapsedTime > 20)
        {
            brain.EnemyHealth.TakeDamage(99999, Guns.GunType.None);
        }
    }
    IEnumerator ApplyForceGradually()
    {
        float duration = 0.5f; // Duration over which the force will be applied
        float elapsed = 0;
        Vector3 startForce = Vector3.zero; // Start with no force
        Vector3 endForce = force; // End with the desired force

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration; // Calculate the interpolation factor

            // Interpolate between startForce and endForce based on t
            Vector3 currentForce = Vector3.Lerp(endForce, startForce, t);

            for (int i = 0; i < len; i++)
            {
                rigidbodies[i].AddForce(currentForce, ForceMode.Impulse);
            }

            yield return null; // Wait for the next frame
        }
    }
    private static readonly Vector3 castDirection = Vector3.down;
    private const float castDistance = 3f;
    private static readonly Vector3 boxSize = new Vector3(1f, 0.1f, 1f);
    public bool GroundedCheck()
    {
        //Ragdoll=>turn off groundcheck=> standup => turn on check again
        RaycastHit[] hits = Physics.BoxCastAll(rigidbodies[0].transform.position, boxSize, castDirection, Quaternion.identity, castDistance, EnemyBrain.groundLayer, QueryTriggerInteraction.Ignore);
        if(hits.Any(hit => hit.collider != null))
            return true;
        
        return false;
    }
}
