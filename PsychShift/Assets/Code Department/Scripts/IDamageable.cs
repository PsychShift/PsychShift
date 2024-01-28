using UnityEngine;

public interface IDamageable
{
    public int CurrentHealth { get; }
    public int MaxHealth { get; }

    public bool IsWeakPoint { get; }

    public delegate void TakeDamageEvent(int Damage);
    public event TakeDamageEvent OnTakeDamage;

    public delegate void DeathEvent(Transform Position);
    public event DeathEvent OnDeath;

    public void TakeDamage(int Damage);
}
