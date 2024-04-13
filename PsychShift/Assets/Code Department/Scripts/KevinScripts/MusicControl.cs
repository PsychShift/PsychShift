using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicControl : MonoBehaviour
{
    [SerializeField]
    AudioSource musicPlayer;
    [SerializeField]
    AudioClip Song;
    public bool isVA;
    public bool activated;
    //public AudioMixer music;
    //public AudioMixer dialouge;
    float currentVal;
    float newVal;
    public float numberDividedBy=2;
    public MusicVolumeManager stopTheSlide;
    public GameObject previousDialouge;

    //divide in half 
    //save ref to OG value 

    // Start is called before the first frame update
    private void TrackChoice()
    {
        this.gameObject.GetComponent<Collider>().enabled = false;
        ChangeTrack(Song);
        if(isVA)
        {
            //Debug.Log("ISVA HERE");

            currentVal = PlayerPrefs.GetFloat("musicVolume");
            //newVal = currentVal/numberDividedBy;
            //music.SetFloat("SFXVolume", currentVal/numberDividedBy);
            //PlayerPrefs.SetFloat("musicVolume", currentVal/numberDividedBy);
            //this.gameObject.GetComponent<Collider>().enabled = false;
            stopTheSlide.musicvolumeSlider.value = currentVal/numberDividedBy;
            stopTheSlide.playingDialouge = true;
            if(previousDialouge!=null)
            {
                Destroy(previousDialouge);
            }
            StartCoroutine(waitForSound());
        }
        
    }
    private void ChangeTrack(AudioClip song)
    {
        musicPlayer.clip = song;
        musicPlayer.Play();
    }
    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.layer == 15)
        {
            TrackChoice();
            /* if(isVA)
                Destroy(gameObject); */
        }   
    }

    IEnumerator waitForSound()
    {
        //Debug.Log("InCoRoutine");
        stopTheSlide.ChangeVolume();
        yield return new WaitWhile (()=> musicPlayer.isPlaying);
        //music.SetFloat("SFXVolume", currentVal);
        //PlayerPrefs.SetFloat("musicVolume", currentVal);
        stopTheSlide.musicvolumeSlider.value = currentVal;
        stopTheSlide.ChangeVolume();
        stopTheSlide.playingDialouge = false;
        Debug.Log("Out");
    }
}
