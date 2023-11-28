using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Slider healthBar;
    public GameObject LoseUI;



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

        if(healthBar.value == 0)
        {
            LoseUI.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
    public void Enabled(bool enabled)
    {
        healthBar.enabled = enabled;
    }
    
}
