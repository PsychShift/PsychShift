using System.Collections;
using System.Collections.Generic;
using Guns.Health;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyDialouge : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource enemyVoice;
    public AudioClip[] enemyLines;
    private bool startedPlaying;
    int enemyHealth;
    public bool dead;
    //Make a script holding all of the death sounds that gives it to this script to ref
    //public float lineCoolDown;

    private void Start() 
    {
        StartCoroutine(VoiceLineSpam());

    } 
    /* void OnBecameVisible()
    {
        if(startedPlaying)
            return;
        startedPlaying = true;
        StartCoroutine(VoiceLineSpam());
    } */
    
    IEnumerator VoiceLineSpam()
    {
        yield return new WaitForSeconds(Random.Range(4,10));
        if(dead == false)
            enemyVoice.PlayOneShot(enemyLines[Random.Range(0, enemyLines.Length)]);
        //yield return new WaitWhile (()=> enemyVoice.isPlaying);
        while(enemyVoice.isPlaying)
        {
            yield return new WaitForSeconds(4);
        }
        //line cooldown
        StartCoroutine(VoiceLineSpam());
        
        
    }
}
