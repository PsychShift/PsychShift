using System;
using ImpactSystem;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PlayThud : MonoBehaviour, IDamageable
{
    public ImpactType impactType;

    public int CurrentHealth  { get; }

    public int MaxHealth { get; }
    public bool IsWeakPoint { get; } = false;

    #pragma warning disable 67
    public event IDamageable.TakeDamageEvent OnTakeDamage;
    public event IDamageable.DeathEvent OnDeath;
    #pragma warning restore 67

    public void TakeDamage(int Damage)
    {
        SurfaceManager.Instance.HandleImpact(gameObject, Vector3.zero, Vector3.up, impactType, 0);
    }

    void OnCollisionEnter(Collision other)
    {
        SurfaceManager.Instance.HandleImpact(other.gameObject, Vector3.zero, Vector3.up, impactType, 0);
    }
    void OnTriggerEnter(Collider other)
    {
        SurfaceManager.Instance.HandleImpact(other.gameObject, Vector3.zero, Vector3.up, impactType, 0);
    }
}