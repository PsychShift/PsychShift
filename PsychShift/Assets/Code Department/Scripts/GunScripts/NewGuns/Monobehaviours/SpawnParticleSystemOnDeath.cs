using System.Collections;
using System.Collections.Generic;
using ImpactSystem.Effects;
using UnityEngine;
using UnityEngine.Pool;
namespace Guns.Demo
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(IDamageable))]
    public class SpawnParticleSystemOnDeath : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem DeathSystem;

        [SerializeField]
        private PlayAudioEffect DeathAudioEffect;

        private Dictionary<GameObject, ObjectPool<GameObject>> ObjectPools = new();


        public IDamageable Damageable;

        private void Awake()
        {
            Damageable = GetComponent<IDamageable>();
        }

        private void OnEnable()
        {
            Damageable.OnDeath += Damageable_OnDeath;
        }

        public Vector3 particleOffset;
        private void Damageable_OnDeath(Transform Position)
        {
            Instantiate(DeathSystem, Position.position + particleOffset, Quaternion.identity);
            gameObject.SetActive(false);
        }

        private void PlayAudioEffectFromList()
        {
            AudioClip clip = DeathAudioEffect.AudioClips[Random.Range(0, DeathAudioEffect.AudioClips.Count)];
            GameObject instance = Instantiate(DeathAudioEffect.AudioSourcePrefab.gameObject);
            AudioSource audioSource = instance.GetComponent<AudioSource>();

            audioSource.transform.position = transform.position;
            audioSource.PlayOneShot(clip, Random.Range(DeathAudioEffect.VolumeRange.x, DeathAudioEffect.VolumeRange.y));
        }
    }
}