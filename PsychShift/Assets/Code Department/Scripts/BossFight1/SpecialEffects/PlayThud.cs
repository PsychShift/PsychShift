using System;
using ImpactSystem;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PlayThud : MonoBehaviour, IDamageable
{
    public ImpactType impactType;

    public int CurrentHealth => throw new NotImplementedException();

    public int MaxHealth => throw new NotImplementedException();

    public event IDamageable.TakeDamageEvent OnTakeDamage;
    public event IDamageable.DeathEvent OnDeath;

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