using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateObject_BossPuzzle : AbstractBossPuzzle
{
    public GameObject GameObject;
    public override void OnHealthGateReached()
    {
        GameObject.SetActive(true);
    }
}
