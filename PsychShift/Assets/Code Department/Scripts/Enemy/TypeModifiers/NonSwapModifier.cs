using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonSwapModifier : AbstractEnemyModifier
{
    EnemyBrain brain;
    public override void ApplyModifier(EnemyBrain brain)
    {
        this.brain = brain;
        StartCoroutine(SwapTags());
    }

    private IEnumerator SwapTags()
    {
        yield return new WaitForSeconds(0.1f);
        RigColliderManager RCM = GetComponent<RigColliderManager>();
        gameObject.tag = "NonSwap";
        RCM.SwapTag("NonSwap");
    }
}
