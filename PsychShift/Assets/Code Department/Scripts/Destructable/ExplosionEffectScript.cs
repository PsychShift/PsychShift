using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffectScript : MonoBehaviour
{
    [SerializeField] public AudioSource explosionAudioSource;

    public IEnumerator Explode(float delay, float radius)
    {
        Vector3 scale = Vector3.one * radius;
        foreach(Transform child in transform)
        {
            child.localScale = scale;
        }
        //explosionAudioSource.minDistance *= radius / 5;
        //explosionAudioSource.maxDistance *= radius / 5;
        explosionAudioSource.Play();
        if(delay != -1)
        {
            yield return new WaitForSeconds(delay);
            Destroy(gameObject);
        }
    }
}
