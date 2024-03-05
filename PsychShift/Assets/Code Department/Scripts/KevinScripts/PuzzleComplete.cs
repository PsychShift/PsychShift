using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleComplete : MonoBehaviour
{
    [Tooltip("PUZZLE ACT INDEX NEEDS TO MATCH THE ORDER THEY ARE PLACED INTO HERE")]
    public PuzzleKit[] actionsInLvl;
    private int[] puzzlesComplt;
    //public int[] test= {1};
    private void Start() 
    {

        puzzlesComplt= new int[actionsInLvl.Length];
        //subscribe
        //PuzzleComplete
        for(int i = 0; i<actionsInLvl.Length; i++)
        {
            Debug.Log("Adding");
            actionsInLvl[i].puzzleIndex = i;
            actionsInLvl[i].OnActivated += ActivatedBox;//invoked event
        }
        //PuzzleActivation(test);//testing

    }
    public void ActivatedBox(int num)
    {
        Debug.Log("SAVEDDDDDDD YESSSS");
        puzzlesComplt[num] = 1;
        Debug.Log("I think");

    }
    public void PuzzleActivation(int[] num)
    {
        for(int i = 0; i<actionsInLvl.Length; i++)
        {

            if(num[i]== 1)
            {
                //Debug.Log("ofduhofjhias");
                actionsInLvl[i].ThisActivate();
            }
        }
        
    }
    private void OnDisable() {
        for(int i = 0; i<actionsInLvl.Length; i++)
        {
            actionsInLvl[i].puzzleIndex = i;
            actionsInLvl[i].OnActivated -= ActivatedBox;
        }
    }
    //public funct that returns string 
}
