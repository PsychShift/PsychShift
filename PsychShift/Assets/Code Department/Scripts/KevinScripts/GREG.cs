using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GREG : MonoBehaviour, IDamageable
{
    // Start is called before the first frame update
    
    private bool wasHit => CurrentHealth == 0;

    public int CurrentHealth {get;set;}

    public int MaxHealth { get { return 1; } }

    public bool IsWeakPoint {get;set;}

    #pragma warning disable 67
    public event IDamageable.TakeDamageEvent OnTakeDamage;
    public event IDamageable.DeathEvent OnDeath;
    #pragma warning restore 67
    public GameObject[] rooms; 
    private int currentRoomIndex;

    private void Awake() {
        CurrentHealth = MaxHealth;
    }
    public void TakeDamage(int Damage)
    {
        ShootHitScan();
    }

     public void ShootHitScan()
    { 

        rooms[currentRoomIndex].SetActive(false);
        currentRoomIndex = (currentRoomIndex+1)%rooms.Length;
        rooms[currentRoomIndex].SetActive(enabled);
    } 
    


}
