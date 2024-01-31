using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Guns.Health
{
    public class EnemyHealth : MonoBehaviour, IDamageable
    {
        [SerializeField]
        private int _MaxHealth = 100;
        [SerializeField]
        private int _Health;

        public int CurrentHealth {get => _Health; private set => _Health = value; }

        public int MaxHealth {get => _MaxHealth; private set=> _MaxHealth = value; }
        public bool IsWeakPoint { get; } = false;
        public event IDamageable.TakeDamageEvent OnTakeDamage;
        public event IDamageable.DeathEvent OnDeath;

        private RigColliderManager rigColliderManager;
        
        private void OnEnable()
        {
            CurrentHealth = MaxHealth;
            rigColliderManager = gameObject.AddComponent<RigColliderManager>();
            rigColliderManager.SetUp(this);
        }
        

        public void TakeDamage(int Damage)
        {
            if (gameObject.layer == 15 && GodModeScript.Instance.GodMode) return;
            int damageTaken = Mathf.Clamp(Damage, 0, CurrentHealth);
            

            CurrentHealth -= damageTaken;
            //healthBar.value = CurrenHealth;
            if(damageTaken !=0)
            {
                OnTakeDamage?.Invoke(damageTaken);
        
            }

            if(CurrentHealth == 0 && damageTaken != 0)
            {
                if(this.gameObject.layer == 6)//EDIT IF LAYER ORDER IS CHANGED
                {
                    OnDeath?.Invoke(transform);
                    if(gameObject.tag == "Boss")
                    {
                        SceneManager.LoadScene("Outro");
                    }
                    //CURRENT SOLUTION NOT FINAL
                    Destroy(gameObject);
                    //gameObject.SetActive(false); 
                }
                else if(gameObject.layer == 15)
                {
                    
                    OnDeath?.Invoke(transform);
                    //CURRENT SOLUTION NOT FINAL
                    //SceneManager.LoadScene(SceneManager.GetActiveScene().name); MIGHT BE A PROBLEM
                    //Destroy(gameObject);
                    //gameObject.SetActive(false);
                }
            }
        }

        private IEnumerator DestroyDelay()
        {
            // Do ragdoll
            // wait
            
            // play effect
            // wait

            return null;
        }

    }

}
