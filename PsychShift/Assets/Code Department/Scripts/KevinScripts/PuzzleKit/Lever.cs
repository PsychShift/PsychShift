using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Lever : PuzzleKit
{
    [Header("THIS IS THE LEVER VARS IGNORE EVERYTHING ABOVE IT except god box/and audio stuff")]
    //public AudioClip leverSound;
    [SerializeField]
    private GameObject beforeSwitch;
    [SerializeField]
    private GameObject afterSwitch;
    [SerializeField]
    private GameObject cutscene;
    public float cutsceneLenght;
    [SerializeField]
    private GameObject greenButton;
    public override void ThisActivate()
    {
        doNotReact = true; 
        base.ThisActivate();
        //LeverAction();

    }

    private void LeverAction()
    {
        //Turn off panel
        //swap to camera/panel objects
        //play coroutine
        //When done turn back on other panel
        //turn on green button 
        //Before -> cutscene -> after/greenbutton
        //leverChildren = leverParent.transform.GetChild();
        beforeSwitch.SetActive(false);
        StartCoroutine(Cutscene());

    }
    private IEnumerator Cutscene()
    {
        cutscene.SetActive(true);
        Beep.PlayOneShot(soundClip);
        yield return new WaitForSeconds(cutsceneLenght);
        cutscene.SetActive(false);
        afterSwitch.SetActive(true);
        greenButton.SetActive(true);
        ThisActivate();
    }
    private void OnTriggerEnter(Collider other) 
    {
        if(other.GetComponent<Collider>().gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                LeverAction();
            }   
    }
}
