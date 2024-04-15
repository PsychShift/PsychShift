using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateBoss : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 15)
        {
            GetComponent<Collider>().enabled = false;
            StartCoroutine(BossBarFadeScript.Instance.FadeIn());
            HangingRobotController.Instance.IsActive = true;
        }
    }
}
