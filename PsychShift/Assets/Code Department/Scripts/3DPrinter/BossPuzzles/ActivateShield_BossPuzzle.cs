using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateShield_BossPuzzle : AbstractBossPuzzle
{
    [SerializeField] private GameObject Shield;

    [SerializeField] private List<ShieldGenerator> shieldGenerators;
    private int shieldGeneratorsLength = 0;
    void Start()
    {
        shieldGeneratorsLength = shieldGenerators.Count;
        DeactivateShieldGenerators();
    }
    public override void OnHealthGateReached()
    {
        StartCoroutine(EnableShield());
        Shield.SetActive(true);
    }

    private IEnumerator EnableShield()
    {
        if(shieldGeneratorsLength <= 0) yield break;
        for(int i = 0; i < shieldGeneratorsLength; i++)
        {
            shieldGenerators[i].Activate();
        }
        yield return new WaitForSeconds(0.5f);
        for(int i = 0; i < shieldGeneratorsLength; i++)
        {
            shieldGenerators[i].beam.Play();
        }
        Shield.SetActive(true);
    }

    public void GeneratorDestroyed(ShieldGenerator shieldGenerator)
    {
        shieldGenerators.Remove(shieldGenerator);
        shieldGeneratorsLength = shieldGenerators.Count;

        DeactivateShieldGenerators();
    }

    private void DeactivateShieldGenerators()
    {
        Shield.SetActive(false);
        for(int i = 0; i < shieldGeneratorsLength; i++)
        {
            shieldGenerators[i].beam.Stop();
            shieldGenerators[i].Disable();
        }
    }
}
