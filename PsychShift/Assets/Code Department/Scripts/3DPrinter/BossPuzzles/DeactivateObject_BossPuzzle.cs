using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateObject_BossPuzzle : AbstractBossPuzzle
{
    public override void OnHealthGateReached()
    {
        gameObject.SetActive(false);
    }
}
