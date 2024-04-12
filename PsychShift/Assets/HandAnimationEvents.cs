using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAnimationEvents : MonoBehaviour
{
    public GameObject leftHand;
    public GameObject sword;
    public GameObject holster;//where the sword is to be placed 
    public GameObject swordHitBox;
    private Collider swordCollider;
    private Rigidbody swordRigidBody;

    //public GameObject gunParent;
    [SerializeField] Animator GunParentAnimator;
    Vector3 swordRotation;
    Vector3 swordLocation;
    Vector3 swordonHandLocal;
    public Transform parentrefSword;
    public AudioSource swordAudio;
    public AudioClip swordClip;
    public GameObject swordTrail;
    private void Start() 
    {
        if(sword!=null)
        {
            swordRotation = sword.transform.localEulerAngles;    
            swordLocation = sword.transform.position;
            swordCollider = swordHitBox.GetComponent<Collider>();
            swordRigidBody = swordHitBox.GetComponent<Rigidbody>();
            swordCollider.enabled = false;
            swordTrail.SetActive(false);
        }
        
    }
    //Grab sword
    public void SwordGrab()
    {
        //parents sword
        //Debug.Log("fgjhikidgsfjhklasdgjklhdfgaskljh");
        sword.transform.SetParent(leftHand.transform);
        swordAudio.Stop();
        swordAudio.PlayOneShot(swordClip);
        sword.transform.localPosition = parentrefSword.transform.localPosition;
        sword.transform.localEulerAngles = parentrefSword.transform.localEulerAngles;
        swordTrail.SetActive(true);
        //sword.transform.localPosition = Vector3(-0.0123966224,0.0029108529,0.000422136916);
        //sword.transform.position = swordLocation;
    }
    public void SwordAway()
    {
        //unparents sword 
        
        sword.transform.SetParent(holster.transform);
        sword.transform.localPosition = Vector3.zero;
        sword.transform.localEulerAngles = swordRotation;
        swordTrail.SetActive(false);
        //Debug.Log("HOLSTER FOR THE LOVE OF EVERYTHING [P]");
        
    }
    public void PutGunAway()
    {
        //Dis one on sword anim
        GunParentAnimator.SetBool("Move", true);
    }
    public void BringGunBack()
    {
        //Dis one on GunParent
        GunParentAnimator.SetBool("Move", false);
    }
    public void ActivateCollider()
    {
        swordCollider.enabled = true;
        swordRigidBody.isKinematic = false;

    }
    public void DeActivateCollider()
    {
        swordCollider.enabled = false;
        swordRigidBody.isKinematic = true;
    }
}
