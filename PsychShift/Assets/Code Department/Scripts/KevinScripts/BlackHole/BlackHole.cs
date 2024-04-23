using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    public PlayerStateMachine playerStateMachine;
    // Start is called before the first frame update
    //find out if player is in range 
    //then start gravity stuff
    private float oGGravVal;
    public float newGravVal;//set in inspector to tune pull 
    private bool activated;
    private bool outOfArea;
    public int secsToActivate;
    public int howLongIsItActivated;
    public GameObject effectBlackHole;
    public AudioClip BHsound;
    public AudioClip shutDownSound;
    public AudioSource BHsource;
    public float timeAfterWarningToActivate = 1f;
    public AudioClip Warning;
    private void Start()
    {
        oGGravVal = playerStateMachine.gravityValue;
        effectBlackHole.SetActive(false);    
    }
    public void StartTheBH() 
    {
        //Detects if player entered area
        if(activated== false)
        {
            StartCoroutine(BHAct());
            activated = true;  
        } 
          
    } 
    //do coroutine that activates and deactivate grav pull 
    private IEnumerator BHAct()
    {
        yield return new WaitForSeconds(secsToActivate - timeAfterWarningToActivate);
        BHsource.PlayOneShot(Warning);
        yield return new WaitForSeconds(timeAfterWarningToActivate);
        BHsource.PlayOneShot(BHsound);
        effectBlackHole.SetActive(true);
        playerStateMachine.gravityValue = newGravVal;
        //turn on blackhole effect
        //activate gravity
        yield return new WaitForSeconds(howLongIsItActivated);
        BHsource.Stop();
        BHsource.PlayOneShot(shutDownSound);
        effectBlackHole.SetActive(false);
        playerStateMachine.gravityValue = oGGravVal;
        //turn off blackhole effect
        //deactivate gravity
        StartCoroutine(BHAct());//keep calling this until player is out of area 
        //detect this with teleporter
    }
    public void EmergencyOff()
    {
        StopAllCoroutines();
        //deactivate animation here as well
        BHsource.Stop();
        BHsource.PlayOneShot(shutDownSound);
        effectBlackHole.SetActive(false);
        playerStateMachine.gravityValue = oGGravVal;
        activated = false;
        //TURN OFF BLACK HOLE 
    }
}
