using System;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Collections;

public class FinalBossHealth : MonoBehaviour, IDamageable
{
    [Header("REMOVE GATE AFTER SPRINT 7 IT'S NOT GOOD")]
    public GameObject GATE;
    public ChildCollider childCollider;
    [SerializeField] private HangingRobotController BossController;
    [SerializeField] private Slider healthBar;
    [SerializeField] private float currentHealth;
    public float CurrentHealth { get { return currentHealth; } }

    [SerializeField] private float maxHealth = 1000;
    public float MaxHealth { get { return maxHealth; } private set { maxHealth = value; } }

    public bool IsWeakPoint { get; set; } = false;

    public event IDamageable.TakeDamageEvent OnTakeDamage;
    public event IDamageable.DeathEvent OnDeath;
    public Phase[] phases;
    [HideInInspector]
    public int currentPhase = 0;

    [HideInInspector]
    public int currentHealthGateIndex = 0;
    int currentHealthGateNumber;
    EBossStates nextBossState = EBossStates.None;
    AbstractBossPuzzle currentHealthGatePuzzle;
    public bool invincible;

    public void TakeDamage(float Damage, Guns.GunType gunType)
    {
        if(invincible) return;
        float damageTaken = Mathf.Clamp(Damage, 0, CurrentHealth);
        currentHealth = Mathf.Clamp(currentHealth - damageTaken, currentHealthGateNumber - 1, currentHealth);
        UpdateHealthBar(damageTaken);

        if(damageTaken != 0)
        {
            OnTakeDamage?.Invoke(damageTaken);
        }
        
        if(currentHealth > 0)//is this supposed to go until 0? 
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
                GATE.SetActive(false);
                OnDeath?.Invoke(transform);
                Debug.Log("DESTROY");
                healthBar.enabled = false;
                Destroy(gameObject);
            }
        }
    }

    private void HealthGateSwitch()
    {
        currentHealthGatePuzzle?.OnHealthGateReached();
        BossController.SwitchState(nextBossState);
        currentHealthGateIndex++;
        Debug.Log("HEALTHGATE " +currentHealthGateIndex);
        if(currentHealthGateIndex < phases[currentPhase].Gates.Length)
        {
            HealthGate gate = phases[currentPhase].Gates[currentHealthGateIndex];
            currentHealthGateNumber = gate.Health;
            currentHealthGatePuzzle = gate.Puzzle;
            nextBossState = gate.BossState;
        }
        else
        {
            currentHealthGateNumber = 0;
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
        for(int i = 0; i < len; i++)
        {
            maxHealth += GetPhaseHealth(i);
        }
        currentPhase = 0;
        SwitchPhase();
        childCollider.SetUp(this);
    }

    private int GetPhaseHealth(int phase)
    {
        return phases[phase].TotalHealth;
    }

    public void SetHealthBarPhaseChange(float maxHealth)
    {
        healthBar.maxValue = maxHealth;
        healthBar.minValue = 0;
        healthBar.value = maxHealth;
        
    }
    public void UpdateHealthBar(float damage)
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
    public EBossStates BossState;
}
