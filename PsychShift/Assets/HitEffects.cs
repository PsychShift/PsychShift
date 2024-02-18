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
    public GameObject hitMarker;
    public AudioClip hitMarkerSound;
    public AudioSource hitSource;
    /* public void HitReaction(bool crit)
    {
        Debug.Log("Hit");
        hitMarker.SetActive(true);
        hitMarker.SetActive(false);
    } */
    public IEnumerator HitReaction()
    {
        hitMarker.SetActive(true);
        hitSource.PlayOneShot(hitMarkerSound);
        yield return new WaitForSeconds(.05f); 
        hitMarker.SetActive(false);
    }

}
