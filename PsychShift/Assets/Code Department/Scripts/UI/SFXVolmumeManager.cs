using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SFXVolmumeManager : MonoBehaviour
{
    [SerializeField] Slider SFXvolumeSlider;
    public AudioMixer AllSFX;
    private float defaultValue = 1f;
    [HideInInspector]
    
    // Start is called before the first frame update
    void Awake()
    {
        if(PlayerPrefs.HasKey("SFXVolume"))
        {
            SFXvolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        }
        else
        {
            SFXvolumeSlider.value = defaultValue;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!PlayerPrefs.HasKey("SFXVolume"))
        {
            PlayerPrefs.SetFloat("SFXVolume",0.2f);
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
