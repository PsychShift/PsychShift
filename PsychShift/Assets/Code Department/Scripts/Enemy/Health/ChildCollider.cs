using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class will allow for individual components to be broken  off an enemy
/// </summary>
public class ChildCollider : MonoBehaviour, IDamageable
{
    [HideInInspector] public IDamageable parentDamageable;

    public float CurrentHealth { get; set; }

    public float MaxHealth { get; set; }
    [SerializeField] private bool isWeakPoint;
    public bool IsWeakPoint { get { return isWeakPoint; } set { isWeakPoint = value; } }
    private int critModifier = 1;

    #pragma warning disable 67
    public event IDamageable.TakeDamageEvent OnTakeDamage;
    public event IDamageable.DeathEvent OnDeath;
    #pragma warning restore 67

    public void SetUp(IDamageable parentDamageable)
    {
        this.parentDamageable = parentDamageable;
    }

    public void TakeDamage(float Damage, Guns.GunType gunType)
    {
        parentDamageable.TakeDamage(Damage, gunType);
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
