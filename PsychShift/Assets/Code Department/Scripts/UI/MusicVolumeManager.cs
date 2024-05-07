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
    // Start is called before the first frame update
    void Awake()
    {
        Load();
        ChangeVolume();
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
