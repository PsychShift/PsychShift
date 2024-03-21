using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damagetest : MonoBehaviour, IDamageable
{
    public int CurrentHealth { get; set; }

    public int MaxHealth { get; set; } = 100;

    public bool IsWeakPoint { get; set; } = false;

    #pragma warning disable 67
    public event IDamageable.TakeDamageEvent OnTakeDamage;
    public event IDamageable.DeathEvent OnDeath;
    #pragma warning restore 67
    void Awake()
    {
        CurrentHealth = MaxHealth;
    }
    public void TakeDamage(int Damage, Guns.GunType gunType)
    {
        CurrentHealth -= Damage;
        if(CurrentHealth <= 0) Destroy(gameObject);
    }
}
