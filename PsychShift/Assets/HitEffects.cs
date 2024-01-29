using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitEffects : MonoBehaviour
{
    /* Image disUIelem;
    SpriteRenderer disSprite; */

    // Start is called before the first frame update

    // Update is called once per frame
    //function
    //Plays anim
    public Animator hitMarker;
    public void HitReaction(bool crit)
    {
        hitMarker.SetBool("Hit", true);
        hitMarker.SetBool("Hit", false);
    }

}
