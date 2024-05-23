using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    public float defaultValue = 0.2f;

    // Start is called before the first frame update
    void Awake()
    {
       if(PlayerPrefs.HasKey("masterVolume"))
        {
        volumeSlider.value = PlayerPrefs.GetFloat("masterVolume");
        }
       else
       {
        volumeSlider.value = defaultValue;
        Save();
       }
        AudioListener.volume = volumeSlider.value;

    }

    // Update is called once per frame
    //save volume data
    void Update()
    {
        if(!PlayerPrefs.HasKey("masterVolume"))
        {
            PlayerPrefs.SetFloat("masterVolume",0.2f);
            Load();
        }
        else
        {
            Load();
        }
    }
    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        Save();
    }
    private void Save()
    {
        PlayerPrefs.SetFloat("masterVolume", volumeSlider.value);
    }
    private void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("masterVolume");
    }
}
