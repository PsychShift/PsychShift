using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeActivateBoss : MonoBehaviour
{
    public void DeActivate()
    {
        StartCoroutine(BossBarFadeScript.Instance.FadeOut());
        HangingRobotController.Instance.IsActive = false;
    }
}
