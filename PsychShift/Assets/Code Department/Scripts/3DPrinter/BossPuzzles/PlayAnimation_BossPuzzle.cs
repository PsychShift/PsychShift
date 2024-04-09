using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimation_BossPuzzle : AbstractBossPuzzle
{
    [SerializeField] private Animator animator;
    [SerializeField] private string animationName;

    public override void OnHealthGateReached()
    {
        animator.Play(animationName);
    }
}
