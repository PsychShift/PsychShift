using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableDamageable : MonoBehaviour, IDamageable
{
    public int CurrentHealth { get; set; }

    public int MaxHealth { get; set; }
    public bool IsWeakPoint { get; } = false;
    private TestBreakObjectCode implodeThing;
    private new Collider collider;

    public event IDamageable.TakeDamageEvent OnTakeDamage;
    public event IDamageable.DeathEvent OnDeath;

    void OnEnable()
    {
        implodeThing = gameObject.GetComponentInChildren<TestBreakObjectCode>();
        collider= gameObject.GetComponent<Collider>();
    }

    public void TakeDamage(int Damage)
    {
        collider.enabled = false;
        implodeThing.BreakTheThing();
    }
}
