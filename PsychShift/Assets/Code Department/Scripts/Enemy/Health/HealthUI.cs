using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Slider healthBar;



    public void SetHealthBarOnSwap(int currentHealth, int maxHealth)
    {
        healthBar.maxValue = maxHealth;
        healthBar.minValue = 0;
        healthBar.value = currentHealth;
    }

    public void UpdateHealthBar(int damage)
    {
        if(healthBar.value - damage < 0) damage = 0;
        healthBar.value -= damage;
    }
    public void Enabled(bool enabled)
    {
        healthBar.enabled = enabled;
    }
}
