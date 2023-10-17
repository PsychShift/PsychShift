using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField]
    private int _MaxHealth = 100;
    [SerializeField]
    private int _Health;

    public int CurrenHealth {get => _Health; private set => _Health = value; }

    public int MaxHealth {get => _MaxHealth; private set=> _MaxHealth = value; }
    public event IDamageable.TakeDamageEvent OnTakeDamage;
    public event IDamageable.DeathEvent OnDeath;

    
    private void OnEnable()
    {
        CurrenHealth = MaxHealth;
    }

    public void TakeDamage(int Damage)
    {
        int damageTaken = Mathf.Clamp(Damage, 0, CurrenHealth);

        CurrenHealth -= damageTaken;
        if(damageTaken !=0)
        {
            OnTakeDamage?.Invoke(damageTaken);
        }

        if(CurrenHealth == 0 && damageTaken != 0)
        {
           OnDeath?.Invoke(transform.position);
           //CURRENT SOLUTION NOT FINAL
           Destroy(gameObject); 
        }
    }

}

