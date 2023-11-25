using UnityEngine;
namespace Guns.Demo
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(IDamageable))]
    public class SpawnParticleSystemOnDeath : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem DeathSystem;
        public IDamageable Damageable;

        private void Awake()
        {
            Damageable = GetComponent<IDamageable>();
        }

        private void OnEnable()
        {
            Damageable.OnDeath += Damageable_OnDeath;
        }

        private void Damageable_OnDeath(Transform Position)
        {
            Instantiate(DeathSystem, Position.position, Quaternion.identity);
            gameObject.SetActive(false);
        }
    }
}