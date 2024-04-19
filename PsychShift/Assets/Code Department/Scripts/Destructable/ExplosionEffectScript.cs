using System.Collections;
using System.Collections.Generic;
using Guns;
using UnityEngine;

public class ExplosionEffectScript : MonoBehaviour
{
    [SerializeField] public AudioSource explosionAudioSource;
    [SerializeField] public AudioClip clip;
    [SerializeField] private bool explodeOnStart = false;
    [SerializeField] GunScriptableObject rocket;

    void Start()
    {
        if(explodeOnStart)
            StartCoroutine(Explode(1f, rocket.DamageConfig.Radius));
    }
    public IEnumerator Explode(float delay, float radius)
    {
        Vector3 scale = Vector3.one * radius;
        foreach(Transform child in transform)
        {
            child.localScale = scale;
        }
        //explosionAudioSource.minDistance *= radius / 5;
        //explosionAudioSource.maxDistance *= radius / 5;
        explosionAudioSource.PlayOneShot(clip);
        if(delay != -1)
        {
            yield return new WaitForSeconds(delay);
            Destroy(gameObject);
        }
    }
}
