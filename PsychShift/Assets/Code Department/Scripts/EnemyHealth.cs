using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Guns.Health
{
    public class EnemyHealth : MonoBehaviour, IDamageable
    {
        [SerializeField] private bool ManualHealthValue;
        [SerializeField]
        private float _MaxHealth = 100;
        [SerializeField]
        private float _Health;
        //xtra var I added for enemyDialouge to stop it from playing
        [SerializeField]

        public float CurrentHealth {get => _Health; private set => _Health = value; }

        public float MaxHealth {get => _MaxHealth; private set=> _MaxHealth = value; }
        public bool IsWeakPoint { get; } = false;
        public event IDamageable.TakeDamageEvent OnTakeDamage;
        public event IDamageable.DeathEvent OnDeath;

        private RigColliderManager rigColliderManager;
        
        private void OnEnable()
        {
            CurrentHealth = MaxHealth;
            rigColliderManager = gameObject.GetComponent<RigColliderManager>();
            rigColliderManager.SetUp(this);
        }

        public void TakeDamage(float Damage, GunType gunType)
        {
            Debug.Log(Damage);
            if (gameObject.layer == 15 && GodModeScript.Instance.GodMode) return;
            float damageTaken = Mathf.Clamp(Damage, 0, CurrentHealth);
            
            CurrentHealth -= damageTaken;
            //healthBar.value = CurrenHealth;
            if(damageTaken !=0)
            {
                OnTakeDamage?.Invoke(damageTaken);
        
            }

            if(CurrentHealth == 0 && damageTaken != 0)
            {
                OnDeath?.Invoke(transform);
                /* if(this.gameObject.layer == 6)//EDIT IF LAYER ORDER IS CHANGED
                {
                    if(gameObject.tag == "Boss")
                    {
                        
                        SceneManager.LoadScene("Outro");
                    }
                } */
            }
        }

        public void ResetHealth()
        {
            _Health = _MaxHealth;
        }

        public void SetMaxHealth(GunScriptableObject gun)
        {
            if(ManualHealthValue) return;
            _MaxHealth = gun.CharacterConfig.Health;
            _Health = _MaxHealth;
        }
    }

}
