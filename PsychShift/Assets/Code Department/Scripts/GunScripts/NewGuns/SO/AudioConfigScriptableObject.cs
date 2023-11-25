using UnityEngine;

namespace Guns
{
    [CreateAssetMenu(fileName = "Audio Config", menuName = "Guns/Audio Config", order = 5)]
    public class AudioConfigScriptableObject : ScriptableObject, System.ICloneable
    {
        [Range(0, 1f)]
        public float Volume = 1f;
        [Range(0, 1f)]
        public float HitVolume = 1f;
        public AudioClip[] FireClips;
        public AudioClip EmptyClip;
        public AudioClip ReloadClip;
        public AudioClip LastBulletClip;
        public AudioClip HitEnemy;
        public AudioClip playerHit;

        public void PlayShootingClip(AudioSource AudioSource, bool IsLastBullet = false)
        {
            if (IsLastBullet && LastBulletClip != null)
            {
                AudioSource.PlayOneShot(LastBulletClip, Volume);
            }
            else
            {
                AudioSource.PlayOneShot(FireClips[Random.Range(0, FireClips.Length)], Volume);
            }
        }

        public void PlayOutOfAmmoClip(AudioSource AudioSource)
        {
            if (EmptyClip != null)
            {
                AudioSource.PlayOneShot(EmptyClip, Volume);
            }
        }

        public void PlayReloadClip(AudioSource AudioSource)
        {
            if (ReloadClip != null)
            {
                AudioSource.PlayOneShot(ReloadClip, Volume);
            }
        }
        public void PlayHitClip(AudioSource AudioSource)
        {
            AudioSource.PlayOneShot(HitEnemy, HitVolume);
        }
        public void PlayHitClipEnemy(AudioSource AudioSource)
        {
            AudioSource.PlayOneShot(playerHit, HitVolume);
        }

        public object Clone()
        {
            AudioConfigScriptableObject config = CreateInstance<AudioConfigScriptableObject>();

            Utilities.CopyValues(this, config);

            return config;
        }
    }
}