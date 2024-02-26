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

    public event IDamageable.TakeDamageEvent OnTakeDamage;
    public event IDamageable.DeathEvent OnDeath;
    public NavMeshSurface disObjectNav;

    void OnEnable()
    {
        implodeThing = gameObject.GetComponentInChildren<TestBreakObjectCode>();
        collider= gameObject.GetComponent<Collider>();
    }

    public void TakeDamage(int Damage)
    {
        collider.enabled = false;
        if(disObjectNav!=null)
            DeactivateNavMesh();
        implodeThing.BreakTheThing();
        
    }
    private void DeactivateNavMesh()
    {
        Debug.Log("removed navmesh data?");
        //disObjectNav.enabled = false;
        disObjectNav.RemoveData();
        //NavMesh.RemoveNavMeshData(disObjectNav.cl);
    }

}
