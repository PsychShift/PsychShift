using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damagetest : MonoBehaviour, IDamageable
{
    public int CurrentHealth { get; set; }

    public int MaxHealth { get; set; } = 100;

    public bool IsWeakPoint { get; set; } = false;

    public event IDamageable.TakeDamageEvent OnTakeDamage;
    public event IDamageable.DeathEvent OnDeath;
    void Awake()
    {
        CurrentHealth = MaxHealth;
    }
    public void TakeDamage(int Damage)
    {
        CurrentHealth -= Damage;
        if(CurrentHealth <= 0) Destroy(gameObject);
    }
}
