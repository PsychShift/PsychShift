using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPuzzleKit : PuzzleKit
{
    [Header("All new Boss variables")]
    public FinalBossHealth BossRef;
    int currentGate;
    [Header("Gates*")]
    public GameObject[] phaseOneObjects;
    public GameObject[] phaseTwoObjects;
    public GameObject[] phaseThreeObejcts;
    private void Awake() 
    {
        currentGate = BossRef.currentHealthGateIndex;//could also be health gate index? Gonna ask later 
    }

    private void Update() 
    {  
        if(BossRef.currentHealthGateIndex >currentGate)
        {
            currentGate =BossRef.currentHealthGateIndex;
            PhaseEndSetUp(currentGate);
        }
        
        
    }
    private void PhaseEndSetUp(int Gate)
    {
        //Healthgate activates script finds out and calls this, this should spawn all necessary items on phase switch
        if(Gate == 1)
        {
            if(soundClip!=null)
                Beep.PlayOneShot(soundClip);
            for(int i = 0; i< phaseOneObjects.Length;i++)
            {
                if(effectForGod!=null)
                    {
                        Instantiate(effectForGod,phaseOneObjects[i].transform.position, Quaternion.identity);
                    }
                phaseOneObjects[i].SetActive(true);
            }
        } 
        else if(Gate == 3)
        {
            if(soundClip!=null)
                Beep.PlayOneShot(soundClip);
            for(int i = 0; i< phaseTwoObjects.Length;i++)
            {
                if(effectForGod!=null)
                    {
                        Instantiate(effectForGod,phaseTwoObjects[i].transform.position, Quaternion.identity);
                    }
                phaseTwoObjects[i].SetActive(true);
                if(phaseTwoObjects[i].GetComponent<PuzzleKit>()!=null)
                {
                    phaseTwoObjects[i].GetComponent<PuzzleKit>().ThisActivate();
                } 
                    
            }
        }
        else if(Gate == 4)
        {
            if(soundClip!=null)
                Beep.PlayOneShot(soundClip);
            for(int i = 0; i< phaseThreeObejcts.Length;i++)
            {
                if(effectForGod!=null)
                    {
                        Instantiate(effectForGod,phaseThreeObejcts[i].transform.position, Quaternion.identity);
                    }
                phaseThreeObejcts[i].SetActive(true);
            }
        }
        
    }
    
    // Start is called before the first frame update
    
}
