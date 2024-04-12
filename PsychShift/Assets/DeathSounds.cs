using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSounds : MonoBehaviour
{
    public AudioSource enemyAudio;
    //public AudioClip[] enemyClips;
    public EnemyDialouge enemyDialouge;
    public void EnemyDeathSound()
    {
        enemyAudio.Stop();
        enemyAudio.PlayOneShot
        (AudioEnemy.AudioManager.Instance.enemyClips
        [UnityEngine.Random.Range(0, AudioEnemy.AudioManager.Instance.enemyClips.Length)]);//grab clip from clip pool in main scene somehow 
        //ref to dialouge player if it has one
        if(enemyDialouge!=null)
        {
            enemyDialouge.dead = true;
        }
    }
}
