using System;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Collections;

public class FinalBossHealth : MonoBehaviour, IDamageable
{
    public Slider healthBar;
    private int currentHealth;
    public int CurrentHealth { get { return currentHealth; } }

    [SerializeField] private int maxHealth = 1000;
    public int MaxHealth { get { return maxHealth; } private set { maxHealth = value; } }

    public bool IsWeakPoint { get; set; } = false;

    public event IDamageable.TakeDamageEvent OnTakeDamage;
    public event IDamageable.DeathEvent OnDeath;
    public Phase[] phases;
    int currentPhase = 0;

    
    int currentHealthGateIndex = 0;
    int currentHealthGateNumber;
    AbstractBossPuzzle currentHealthGatePuzzle;

    public void TakeDamage(int Damage)
    {
        int damageTaken = Mathf.Clamp(Damage, 0, CurrentHealth);
        currentHealth -= damageTaken;
        UpdateHealthBar(damageTaken);

        if(damageTaken != 0)
        {
            OnTakeDamage?.Invoke(damageTaken);
        }
        
        if(currentHealth > 0)
        {
            if(currentHealth <= currentHealthGateNumber)
            {
                HealthGateSwitch();
            }
        }
        else
        {
            currentPhase++;
            if(currentPhase < phases.Length)
            {
                // still has more phases, just switch phase
                SwitchPhase();
            }
            else
            {
                // die
                OnDeath?.Invoke(transform);
            }
        }
    }

    private void HealthGateSwitch()
    {
        currentHealthGatePuzzle?.OnHealthGateReached();
        currentHealthGateIndex++;
        Debug.Log("HealthgateSwitch old index" + (currentHealthGateIndex - 1) + " new index " + currentHealthGateIndex);
        if(currentHealthGateIndex < phases[currentPhase].Gates.Length)
        {
            HealthGate gate = phases[currentPhase].Gates[currentHealthGateIndex];
            currentHealthGateNumber = gate.Health;
            currentHealthGatePuzzle = gate.Puzzle;
        }
    }

    private void SwitchPhase()
    {
        currentHealth = GetPhaseHealth(currentPhase);

        currentHealthGateIndex = -1;
        HealthGateSwitch();

        SetHealthBarPhaseChange(currentHealth);
    }

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 0;
        int len = phases.Length;
        Debug.Log(len);
        for(int i = 0; i < len; i++)
        {
            maxHealth += GetPhaseHealth(i);
        }
        currentPhase = 0;
        SwitchPhase();
    }

    private int GetPhaseHealth(int phase)
    {
        return phases[phase].TotalHealth;
    }

    public void SetHealthBarPhaseChange(int maxHealth)
    {
        healthBar.maxValue = maxHealth;
        healthBar.minValue = 0;
        healthBar.value = maxHealth;
        
    }
    public void UpdateHealthBar(int damage)
    {
        healthBar.value -= damage;
    }

}

[Serializable]
public struct Phase
{
    public int TotalHealth;
    public HealthGate[] Gates;
}

[Serializable]
public struct HealthGate
{
    public int Health;
    public AbstractBossPuzzle Puzzle;
}

public abstract class AbstractBossPuzzle : MonoBehaviour
{
    public abstract void OnHealthGateReached();
}
