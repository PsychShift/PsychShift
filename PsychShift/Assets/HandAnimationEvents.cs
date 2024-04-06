using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAnimationEvents : MonoBehaviour
{
    public GameObject leftHand;
    public GameObject sword;
    public GameObject holster;//where the sword is to be placed 
    //public GameObject gunParent;
    [SerializeField] Animator GunParentAnimator;
    Vector3 swordRotation;
    Vector3 swordLocation;
    Vector3 swordonHandLocal;
    public Transform parentrefSword;
    private void Start() 
    {
        if(sword!=null)
        {
            swordRotation = sword.transform.localEulerAngles;    
            swordLocation = sword.transform.position;
        }
        
    }
    //Grab sword
    public void SwordGrab()
    {
        //parents sword
        //Debug.Log("fgjhikidgsfjhklasdgjklhdfgaskljh");
        sword.transform.SetParent(leftHand.transform);
        sword.transform.localPosition = parentrefSword.transform.localPosition;
        sword.transform.localEulerAngles = parentrefSword.transform.localEulerAngles;
        //sword.transform.localPosition = Vector3(-0.0123966224,0.0029108529,0.000422136916);
        //sword.transform.position = swordLocation;
    }
    public void SwordAway()
    {
        //unparents sword 
        
        sword.transform.SetParent(holster.transform);
        sword.transform.localPosition = Vector3.zero;
        sword.transform.localEulerAngles = swordRotation;
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
}
