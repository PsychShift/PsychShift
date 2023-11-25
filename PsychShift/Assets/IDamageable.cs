using UnityEngine;

public interface IDamageable
{
    public int CurrenHealth {get; }
    public int MaxHealth {get; }

    public delegate void TakeDamageEvent(int Damage);
    public event TakeDamageEvent OnTakeDamage;

    public delegate void DeathEvent(Transform Position);
    public event DeathEvent OnDeath;

    public void TakeDamage(int Damage);
}
