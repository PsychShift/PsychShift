using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SFXVolmumeManager : MonoBehaviour
{
    [SerializeField] Slider SFXvolumeSlider;
    public AudioMixer AllSFX;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!PlayerPrefs.HasKey("SFXVolume"))
        {
            PlayerPrefs.SetFloat("SFXVolume",1);
            Load();
        }
        else
        {
            Load();
        }
    }
    public void ChangeVolume()
    {
        //AllSFX.volume = SFXvolumeSlider.value;
        float volume = SFXvolumeSlider.value;
        AllSFX.SetFloat("SFXVolume", Mathf.Log10(volume)*20);
        Save();
    }
    private void Save()
    {
        PlayerPrefs.SetFloat("SFXVolume", SFXvolumeSlider.value);
    }
    private void Load()
    {
        SFXvolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume");
    }
}
