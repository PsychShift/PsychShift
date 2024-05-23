using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MusicVolumeManager : MonoBehaviour
{
    [SerializeField] 
    public Slider musicvolumeSlider;
    public AudioSource MusicSource;
    public bool playingDialouge;
    private float defaultValue = 0.2f;
    // Start is called before the first frame update
    void Awake()
    {
        if(PlayerPrefs.HasKey("musicVolume"))
        {
            musicvolumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
        }
        else
        {
            musicvolumeSlider.value = defaultValue;
            Save();
        }
        //Load();
        //ChangeVolume();
    }

    // Update is called once per frame
    void Update()
    {
        if(!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume",1);
            Load();
        }
        else
        {
            if(!playingDialouge)
                Load();
        }
    }
    public void ChangeVolume()
    {
        MusicSource.volume = musicvolumeSlider.value;
        Save();
    }
    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", musicvolumeSlider.value);
    }
    private void Load()
    {
        musicvolumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }
}
