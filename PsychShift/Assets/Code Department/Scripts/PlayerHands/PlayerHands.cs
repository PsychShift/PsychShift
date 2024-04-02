using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerHands : MonoBehaviour
{
    public static PlayerHands Instance;
    [SerializeField] private RigBuilder rigBuilder;
    [SerializeField] private TwoBoneIKConstraint leftIK;
    [SerializeField] private TwoBoneIKConstraint rightIK;
    // Start is called before the first frame update
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void SetHandPositions(HandsOrientation handsOrientation)
    {
        leftIK.data.target = handsOrientation.leftHand.transform;
        leftIK.data.hint = handsOrientation.leftElbow.transform;
        rightIK.data.target = handsOrientation.rightHand.transform;
        rightIK.data.hint = handsOrientation.rightElbow.transform;

        rigBuilder.Build();
    }
}
