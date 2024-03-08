using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateShield_BossPuzzle : AbstractBossPuzzle
{
    [SerializeField] private GameObject Shield;
    public override void OnHealthGateReached()
    {
        Shield.SetActive(true);
    }
}
