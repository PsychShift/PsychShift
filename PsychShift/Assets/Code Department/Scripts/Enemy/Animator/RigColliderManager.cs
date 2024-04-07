using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigColliderManager : MonoBehaviour
{
    //class added by Kevin for anim transition
    public class BoneTransform
    {
        public Vector3 Position {get; set;} 
        public Quaternion Rotation {get; set;}
    }
    public List<ChildCollider> childColliders;
    [SerializeField]
    public Animator Animator;//changed to public to access in standup state
    private Transform RagdollRoot;
    [HideInInspector] public Rigidbody[] Rigidbodies;
    private CharacterJoint[] Joints;
    private Collider[] Colliders;
/*     private float _timeToWakeUp;
   // private float animationTimeBack;
    private float animationTime; */
    [HideInInspector] public bool isDoneStanding;
    //all dis for anim transition
    public BoneTransform[] _standUpBoneTransforms;
    public BoneTransform[] _ragdollBoneTransforms;
    [HideInInspector] public Transform[] _bones;
    
    //private const string _standUpClipName = "Stand Up";
    [SerializeField]
    private float _timeToResetBones= .5f;
    [HideInInspector] public float _elapsedResetBonesTime;
    public void SetUp(IDamageable parentDamageable)
    {
        RagdollRoot = transform.GetChild(0);
        if(Animator == null)
            Animator = RagdollRoot.GetComponent<Animator>();
            
        Colliders = RagdollRoot.GetComponentsInChildren<Collider>();
        Rigidbodies = RagdollRoot.GetComponentsInChildren<Rigidbody>();
        Joints = RagdollRoot.GetComponentsInChildren<CharacterJoint>();
        //everything for ragdoll stuff below
        /* _hipBones = Animator.GetBoneTransform(HumanBodyBones.Hips);
        _bones = _hipBones.GetComponentsInChildren<Transform>();
        _standUpBoneTransforms = new BoneTransform[_bones.Length];
        _ragdollBoneTransforms = new BoneTransform[_bones.Length];
        for(int boneIndex = 0; boneIndex < _bones.Length; boneIndex++)
        {
            _standUpBoneTransforms[boneIndex] = new BoneTransform();
            _ragdollBoneTransforms[boneIndex] = new BoneTransform();
        }
        PopulateAnimationStartBoneTransfroms(_standUpClipName, _standUpBoneTransforms); */

        if(childColliders.Count == 0)
            childColliders = new List<ChildCollider>();
        
        foreach (Collider collider in Colliders)
        {
            if(collider.gameObject == gameObject) continue;
            if(collider.gameObject.TryGetComponent(out ChildCollider premadeScript))
            {
                premadeScript.SetUp(parentDamageable);
                childColliders.Add(premadeScript);
            }
            else
            {
                ChildCollider childCollider = collider.gameObject.AddComponent<ChildCollider>();
                childCollider.SetUp(parentDamageable);
                childColliders.Add(childCollider);
            }
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
            joint.enableProjection = true;
        }
        /* foreach (Collider collider in Colliders)
        {
            collider.enabled = true;
        } */
        //_timeToWakeUp = Random.Range(5,10);
        foreach (Rigidbody rigidbody in Rigidbodies)
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.useGravity = true;
            rigidbody.isKinematic = false;
        }
        //if not ded 
        //StartCoroutine(StandUp());
    }

    public void EnableAnimator()
    {
        transform.tag = "Swapable";
        isDoneStanding = false;
        Animator.enabled = true;
        foreach (CharacterJoint joint in Joints)
        {
            joint.enableCollision = false;
            joint.enableProjection = false;
        }
        foreach (Rigidbody rigidbody in Rigidbodies)
        {
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;
        }
        //StartCoroutine(WaitForStandUpAnim());
    }

    /* public void PopulateBoneTransforms(BoneTransform[] boneTransforms)//snap shot of current postion of bones
    {
        for(int boneIndex = 0; boneIndex <_bones.Length; boneIndex++)
        {
            boneTransforms[boneIndex].Position = _bones[boneIndex].localPosition;
            boneTransforms[boneIndex].Rotation = _bones[boneIndex].localRotation;
        }
    } */
    /* public void PopulateAnimationStartBoneTransfroms(string clipName, BoneTransform[] boneTransforms)
    {
        Vector3 positionBeforeSampling = transform.position;
        Quaternion rotationBeforeSampling = transform.rotation;
       foreach(AnimationClip clip in Animator.runtimeAnimatorController.animationClips)
       {
        if(clip.name == clipName)
        {
            clip.SampleAnimation(gameObject, 0);
            PopulateBoneTransforms(_standUpBoneTransforms);
            break;
        }
        
       }
       transform.position = positionBeforeSampling;
       transform.rotation = rotationBeforeSampling;
    }
 */
    public bool ResettingBonesBehavior()
    {
        _elapsedResetBonesTime += Time.deltaTime;
        float elapsedPercantage = _elapsedResetBonesTime / _timeToResetBones;

        for(int boneIndex = 0; boneIndex < _bones.Length; boneIndex ++)
        {
            //Another LOOP???????????????
            _bones[boneIndex].localPosition = Vector3.Lerp(
                _ragdollBoneTransforms[boneIndex].Position,
                _standUpBoneTransforms[boneIndex].Position,
                elapsedPercantage
            );
            _bones[boneIndex].localRotation = Quaternion.Lerp(
                _ragdollBoneTransforms[boneIndex].Rotation,
                _standUpBoneTransforms[boneIndex].Rotation,
                elapsedPercantage
            );

            if(elapsedPercantage >=1)
            {
                return true;
            }
        }
        return false;
    }
    //Coroutine 
    //
    /* private IEnumerator StandUp()
    {

        yield return new WaitForSeconds(_timeToWakeUp);
        StartCoroutine(WaitForStandUpAnim());

    } */
    /* private IEnumerator WaitForStandUpAnim()
    {
        //playAnim stand up
        yield return new WaitForSeconds(animationTime);
        isDoneStanding= true;
        //activate nonragdoll state
    } */
}
