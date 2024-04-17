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
    [Tooltip("The parent of the hitmarker sprites")]
    public GameObject hitMarker;
    private GameObject hitMarkerChild;
    private GameObject critHitMarkerChild;
    public AudioClip hitMarkerSound;
    public AudioClip critHitMarkerSound;
    public AudioSource hitSource;
    /* public void HitReaction(bool crit)
    {
        Debug.Log("Hit");
        hitMarker.SetActive(true);
        hitMarker.SetActive(false);
    } */
    void Awake()
    {
        hitMarkerChild = hitMarker.transform.GetChild(0).gameObject;
        critHitMarkerChild = hitMarker.transform.GetChild(1).gameObject;
    }
    public IEnumerator HitReaction(bool crit)
    {
        if (crit)
        {
            critHitMarkerChild.SetActive(true);
            hitSource.PlayOneShot(critHitMarkerSound);
            yield return new WaitForSeconds(.05f); 
            critHitMarkerChild.SetActive(false);
            Debug.Log("crit");
        }
        else
        {
            hitMarkerChild.SetActive(true);
            hitSource.PlayOneShot(hitMarkerSound);
            yield return new WaitForSeconds(.05f); 
            hitMarkerChild.SetActive(false);
            Debug.Log("no crit");
        }
    }

}
