using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapEnemySpawns_BossPuzzle : AbstractBossPuzzle
{
    [SerializeField] private HangingRobotController controller;

    [SerializeField] private List<Guns.GunType> guns;
    [SerializeField] private List<EEnemyModifier> modifiers;


    private void Start()
    {
        if(controller == null) controller = GetComponentInChildren<HangingRobotController>();
    }
    public override void OnHealthGateReached()
    {
        controller.guns = guns;
        controller.modifiers = modifiers;
    }
}
