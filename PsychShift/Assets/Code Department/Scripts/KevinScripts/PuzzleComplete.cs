using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleComplete : MonoBehaviour
{
    // Start is called before the first frame update
    public PuzzleKit[] puzzlesInLvl;
    private bool[] puzzlesComplt;
    private void Awake() {
        //subscribe
        //PuzzleComplete
        for(int i = 0; i<puzzlesInLvl.Length; i++)
        {
            puzzlesInLvl[i].puzzleIndex = i;
            puzzlesInLvl[i].OnPuzzleFinish += PuzzleFinish;
        }
    }
    public void PuzzleFinish(int num)
    {
        puzzlesComplt[num] = true;

    }
    public void PuzzleActivation(int num)
    {

    }
    private void OnDisable() {
        for(int i = 0; i<puzzlesInLvl.Length; i++)
        {
            puzzlesInLvl[i].puzzleIndex = i;
            puzzlesInLvl[i].OnPuzzleFinish -= PuzzleFinish;
        }
    }
}
