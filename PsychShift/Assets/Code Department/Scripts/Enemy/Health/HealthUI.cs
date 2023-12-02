using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HealthUI : MonoBehaviour
{
    public Slider healthBar;
    public GameObject LoseUI;
    public GameObject LoseUIFirst;
    public GameObject DamageUI;
    public float timeDamaged = 2.0f;

    bool isDamaged;
    float DamagedTimer;

    void Start()
    {
        DamageUI.SetActive(false);
    }
    void Update()
    {
        if (isDamaged)
        {
            DamagedTimer -= Time.deltaTime;
            DamageUI.SetActive(true);
            if (DamagedTimer < 0)
            {
                isDamaged = false;
                DamageUI.SetActive(false);
            }
        }
    }

    public void SetHealthBarOnSwap(int currentHealth, int maxHealth)
    {
        healthBar.maxValue = maxHealth;
        healthBar.minValue = 0;
        healthBar.value = currentHealth;
    }

    public void UpdateHealthBar(int damage)
    {
        if(healthBar.value - damage < 0) damage = 0;
        {
        healthBar.value -= damage;
        isDamaged = true;
            DamagedTimer = timeDamaged;
        }

        if(healthBar.value == 0)
        {
            LoseUI.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            EventSystem.current.SetSelectedGameObject(LoseUIFirst);
            Time.timeScale = 0f;
        }
    }
    public void Enabled(bool enabled)
    {
        healthBar.enabled = enabled;
    }
    
}
