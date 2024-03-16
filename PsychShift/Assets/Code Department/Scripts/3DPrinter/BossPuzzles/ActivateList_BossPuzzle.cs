using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateList_BossPuzzle : AbstractBossPuzzle
{
    [SerializeField] private List<AbstractBossPuzzle> BossPuzzleList;

    public override void OnHealthGateReached()
    {
        for(int i = 0; i < BossPuzzleList.Count; ++i)
        {
            BossPuzzleList[i].OnHealthGateReached();
        }
    }
}
