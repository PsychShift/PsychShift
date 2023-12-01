using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicControl : MonoBehaviour
{
    [SerializeField]
    AudioSource musicPlayer;
    [SerializeField]
    AudioClip Song;
    public bool isVA;
    public bool activated;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void TrackChoice()
    {
        ChangeTrack(Song);
    }
    private void ChangeTrack(AudioClip song)
    {
        musicPlayer.clip = song;
        musicPlayer.Play();
    }
    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "Player")
        {
            TrackChoice();
            if(isVA)
                Destroy(gameObject);
        }
            

            
    }
}
