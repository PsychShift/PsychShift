using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;

    // Start is called before the first frame update
    void Awake()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("masterVolume", 0.2f);
        AudioListener.volume = volumeSlider.value;
    }

    // Update is called once per frame
    //save volume data
    void Update()
    {
        if(!PlayerPrefs.HasKey("masterVolume"))
        {
            PlayerPrefs.SetFloat("masterVolume",1);
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
