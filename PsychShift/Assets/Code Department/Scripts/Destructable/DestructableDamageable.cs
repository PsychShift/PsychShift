using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class DestructableDamageable : MonoBehaviour, IDamageable
{
    public int CurrentHealth { get; set; }

    public int MaxHealth { get; set; }
    public bool IsWeakPoint { get; } = false;
    private TestBreakObjectCode implodeThing;
    private new Collider collider;
    #pragma warning disable 67
    public event IDamageable.TakeDamageEvent OnTakeDamage;
    public event IDamageable.DeathEvent OnDeath;
    #pragma warning restore 67
    public NavMeshSurface disObjectNav;

    void OnEnable()
    {
        implodeThing = gameObject.GetComponentInChildren<TestBreakObjectCode>();
        collider= gameObject.GetComponent<Collider>();
    }

    public void TakeDamage(int Damage, Guns.GunType gunType)
    {
        collider.enabled = false;
        if(disObjectNav!=null)
            StartCoroutine(DestroyNavmeshData());
        implodeThing.BreakTheThing();
        
    }
    IEnumerator DestroyNavmeshData()
    {
        yield return new WaitForSeconds(0.1f);
        disObjectNav.RemoveData();
    }

}
