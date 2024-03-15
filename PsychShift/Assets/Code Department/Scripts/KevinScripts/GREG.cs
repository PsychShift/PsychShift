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
    public Material[] colors;//current color gets placed on this object
    public MeshRenderer currentColor;//current index
    public MeshRenderer nextColor; //+1 index and then reset to 0 when reached length
    private int nextColorIndex;
    private int currentRoomIndex;

    private void Awake() 
    {
        CurrentHealth = MaxHealth;
        currentColor.material = colors[0];
        nextColor.material = colors[1];

    }
    public void TakeDamage(int Damage)
    {
        ShootHitScan();
    }

     public void ShootHitScan()
    { 

        rooms[currentRoomIndex].SetActive(false);
        currentRoomIndex = (currentRoomIndex+1)%rooms.Length;
        nextColorIndex = currentRoomIndex+1;
        if(nextColorIndex >rooms.Length)
            nextColorIndex = 0;
        rooms[currentRoomIndex].SetActive(enabled);
        currentColor.material = colors[currentRoomIndex];
        nextColor.material = colors[nextColorIndex];
    } 
    


}
