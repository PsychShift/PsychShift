using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class DeActivateBoss : MonoBehaviour
{
    public void DeActivate()
    {
        StartCoroutine(BossBarFadeScript.Instance.FadeOut());
        HangingRobotController.Instance.animController.enabled = false;
        HangingRobotController.Instance.enabled = false;
        RigBuilder rigs = HangingRobotController.Instance.animController.GetComponent<RigBuilder>();
        foreach (var rig in rigs.layers)
            rig.active = false;
    }
}
