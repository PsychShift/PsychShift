using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class will allow for individual components to be broken  off an enemy
/// </summary>
public class ChildCollider : MonoBehaviour, IDamageable
{
    IDamageable parentDamageable;

    public int CurrentHealth { get; set; }

    public int MaxHealth { get; set; }
    private int critModifier = 1;

    public event IDamageable.TakeDamageEvent OnTakeDamage;
    public event IDamageable.DeathEvent OnDeath;

    public void SetUp(IDamageable parentDamageable)
    {
        this.parentDamageable = parentDamageable;
    }

    public void TakeDamage(int Damage)
    {
        Damage *= critModifier;
        parentDamageable.TakeDamage(Damage);
    }

    public void SwapTag(string tag)
    {
        gameObject.tag = tag;
    }

    public void SwapLayer(string layer)
    {
        gameObject.layer = LayerMask.NameToLayer(layer);
    }


    public void CritCollider(int modifier)
    {
        critModifier = modifier;
    }
}
