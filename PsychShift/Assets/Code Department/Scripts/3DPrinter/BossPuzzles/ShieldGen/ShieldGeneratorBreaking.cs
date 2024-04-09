using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldGeneratorBreaking : MonoBehaviour
{

    [SerializeField] private ShieldGenerator[] shieldGenerators;
    [SerializeField] private AbstractBossPuzzle[] reactions;

    private int shieldGenIndex = 0;
    
    void OnEnable()
    {
        for (int i = 0; i < shieldGenerators.Length; i++)
            shieldGenerators[i].OnDeath += ShieldGeneratorDestroyed;
    }
    private void ShieldGeneratorDestroyed(Transform transform)
    {
        ShieldGenerator gen = transform.GetComponent<ShieldGenerator>();
        gen.OnDeath -= ShieldGeneratorDestroyed;
        reactions[shieldGenIndex].OnHealthGateReached();
        shieldGenIndex++;
    }
    void OnDisable()
    {
        for (int i = 0; i < shieldGenerators.Length; i++)
            if(shieldGenerators[i] != null)
                shieldGenerators[i].OnDeath -= ShieldGeneratorDestroyed;
    }
}
