using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigColliderManager : MonoBehaviour
{
    public List<ChildCollider> childColliders;

    private Animator Animator;
    private Transform RagdollRoot;
    private Rigidbody[] Rigidbodies;
    private CharacterJoint[] Joints;
    private Collider[] Colliders;
    private float _timeToWakeUp;
   // private float animationTimeBack;
    private float animationTime;
    public bool isDoneStanding;
    public void SetUp(IDamageable parentDamageable)
    {
        RagdollRoot = transform.GetChild(0);
        Animator = RagdollRoot.GetComponent<Animator>();
        childColliders = new List<ChildCollider>();
        Colliders = RagdollRoot.GetComponentsInChildren<Collider>();
        Rigidbodies = RagdollRoot.GetComponentsInChildren<Rigidbody>();
        Joints = RagdollRoot.GetComponentsInChildren<CharacterJoint>();

        foreach (Collider collider in Colliders)
        {
            if(collider.gameObject == gameObject) continue;
            if(collider.gameObject.TryGetComponent(out ChildCollider premadeScript))
            {
                premadeScript.SetUp(premadeScript);
                childColliders.Add(premadeScript);
                continue;
            }
            ChildCollider childCollider = collider.gameObject.AddComponent<ChildCollider>();
            childCollider.SetUp(parentDamageable);
            childColliders.Add(childCollider);
        }

        EnableAnimator();
    }

    public void SwapTag(string tag)
    {

        foreach (var collider in childColliders)
        {
            collider.SwapTag(tag);
        }
        SwapLayer("Enemy");
    }

    public void SwapLayer(string layer)
    {
        foreach (var collider in childColliders)
        {
            collider.SwapLayer(layer);
        }
    }

    public void EnableRagdoll()
    {
        //check health to see if ded 
        transform.tag = "Untagged";
        Animator.enabled = false;
        foreach (CharacterJoint joint in Joints)
        {
            joint.enableCollision = true;
        }
        /* foreach (Collider collider in Colliders)
        {
            collider.enabled = true;
        } */
        //_timeToWakeUp = Random.Range(5,10);
        foreach (Rigidbody rigidbody in Rigidbodies)
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.detectCollisions = true;
            rigidbody.useGravity = true;
        }
        //if not ded 
        //StartCoroutine(StandUp());
    }

    public void EnableAnimator()
    {
        isDoneStanding = false;
        Animator.enabled = true;
        foreach (CharacterJoint joint in Joints)
        {
            joint.enableCollision = false;
        }
        /* foreach (Collider collider in Colliders)
        {
            collider.enabled = false;
        } */
        foreach (Rigidbody rigidbody in Rigidbodies)
        {
            //rigidbody.detectCollisions = false;
            rigidbody.useGravity = false;
        }
        StartCoroutine(WaitForStandUpAnim());
    }
    //Coroutine 
    //
    /* private IEnumerator StandUp()
    {

        yield return new WaitForSeconds(_timeToWakeUp);
        StartCoroutine(WaitForStandUpAnim());

    } */
    private IEnumerator WaitForStandUpAnim()
    {
        //playAnim stand up
        yield return new WaitForSeconds(animationTime);
        isDoneStanding= true;
        //activate nonragdoll state
    }
}
