using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class HealthUI : MonoBehaviour
{
    public static HealthUI Instance { get; private set; }
    public Slider healthBar;
    public GameObject LoseUI;
    public GameObject LoseUIFirst;
    public GameObject DamageUI;
    public float timeDamaged = 2.0f;

    bool isDamaged;
    float DamagedTimer;
    static float currentHealth;
    private Coroutine damageCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // If another instance already exists, destroy this one
        }

        DamageUI.SetActive(false);
    }

    public void SetHealthBarOnSwap(float currentHealth, float maxHealth)
    {
        healthBar.maxValue = maxHealth;
        healthBar.minValue = 0;
        healthBar.value = currentHealth;
        
    }

    public void UpdateHealthBar(float damage)
    {
        if(healthBar.value - damage <  0) damage =  0;
        healthBar.value -= damage;
        isDamaged = true;
        DamagedTimer = timeDamaged;

        // Stop any existing coroutine to prevent overlap
        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
        }

        // Start the new coroutine
        damageCoroutine = StartCoroutine(UpdateUICoroutine());
    }
    IEnumerator UpdateUICoroutine()
    {
        while (isDamaged)
        {
            DamagedTimer -= Time.deltaTime;
            DamageUI.SetActive(true);
            if (DamagedTimer <=  0)
            {
                isDamaged = false;
                DamageUI.SetActive(false);
            }
            yield return null; // Wait until the next frame
        }
    }
    public void Enabled(bool enabled)
    {
        healthBar.enabled = enabled;
    }

    /// <summary>
    /// I have to take in the transform because of how the event is set up in EnemyHealth.cs, I don't actually need it though.
    /// </summary>
    /// <param name="transform"></param>
    public void HandleDeath(Transform transform)
    {
        TimeManager.Instance.UndoSlowmotion();
        InputManager.Instance.SwapControlMap(ActionMapEnum.ui);
        LoseUI.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        EventSystem.current.SetSelectedGameObject(LoseUIFirst);
        TimeManager.Instance.Pause();
    }

    void OnDestroy()
    {
        Instance = null;
    }
    
}
