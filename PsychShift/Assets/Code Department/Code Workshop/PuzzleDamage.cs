using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleDamage : MonoBehaviour, IDamageable
{
    [SerializeField]
    private int _MaxHealth = 100;
    [SerializeField]
    private int _Health;

    public int CurrentHealth {get => _Health; private set => _Health = value; }

    public int MaxHealth {get => _MaxHealth; private set=> _MaxHealth = value; }
    public bool IsWeakPoint { get; } = false;

    public float rotationSpeed = 1f;

    #pragma warning disable 67
    public event IDamageable.TakeDamageEvent OnTakeDamage;
    public event IDamageable.DeathEvent OnDeath;
    #pragma warning restore 67

    InteractWithMeImLonely please;
    float direction = 1f;

    bool forward = true;
    public void TakeDamage(int Damage)
    {
        forward = !forward;
        int damageTaken = Mathf.Clamp(Damage, 0, CurrentHealth);
            
        CurrentHealth -= damageTaken;

        if(CurrentHealth <= 0)
        {
            // DIE

        }

    }

    private void FixedUpdate() {
        
        direction = forward ? 1f : -1f;
        direction *= rotationSpeed;
        transform.Rotate(0f, 1 * direction, 0f, Space.Self);
    }
}


public class InteractWithMeImLonely : MonoBehaviour
{
    public void dothis()
    {

    }
    public void YAY(int damage)
    {
        Debug.Log("Im not desperate I swear: " + damage);
    }
}