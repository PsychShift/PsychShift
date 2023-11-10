using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicControl : MonoBehaviour
{
    [SerializeField]
    AudioSource musicPlayer;
    [SerializeField]
    AudioClip Song;
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
        TrackChoice();    
    }
}
