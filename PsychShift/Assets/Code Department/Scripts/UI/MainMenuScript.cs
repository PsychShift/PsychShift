using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class MainMenuScript : MonoBehaviour
{
    public GameObject LoadMenu;
    public GameObject MainMenu;
    public GameObject MainMenuFirst;
    public GameObject SettingsMenu;
    public GameObject SettingsMenuFirst;
    public GameObject CreditsMenu;
    public GameObject CreditsMenuFirst;
    // Start is called before the first frame update
    void Start()
    {
        SettingsMenu.SetActive(false);
        LoadMenu.SetActive(false);
        CreditsMenu.SetActive(false);

        // Jonathan plased this in the update state, dont know why. Undo it if I broke something.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
        EventSystem.current.SetSelectedGameObject(MainMenuFirst);
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void NewGame()
    {
        //Here is where the first intended scene of the game will be called
    }

    public void Continue()
    {
        //Here is where the game will continue from where the player last left off
    }
    public void LoadArtScene()
    {
        SceneManager.LoadScene("Art Prototype");
    }
    public void GameplayScene()
    {
        SceneManager.LoadScene("GameplayTest");
    }
    public void VSScene()
    {
        SceneManager.LoadScene("Intro");
    }
    public void IntroLobby()
    {
        SceneManager.LoadScene("LOBBY INTRO LEVEL");
    }
    public void LobbyBoss()
    {
        SceneManager.LoadScene("TREVOR ROOM");
    }
  

    public void OpenLoad()
    {
        LoadMenu.SetActive(true);
    }

    public void CloseLoad()
    {
        LoadMenu.SetActive(false);
    }
    public void OpenCredits()
    {
        CreditsMenu.SetActive(true);
        MainMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(CreditsMenuFirst);
    }
    public void CloseCredits()
    {
        CreditsMenu.SetActive(false);
        MainMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(MainMenuFirst);
    }

    public void OpenSettings()
    {
        SettingsMenu.SetActive(true);
        MainMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(SettingsMenuFirst);
    }

    public void CloseSettings()
    {
        SettingsMenu.SetActive(false);
        MainMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(MainMenuFirst);
    }
    public void ILab()
    {
        SceneManager.LoadScene("TOMAS");
    }
    public void DLab()
    {
        SceneManager.LoadScene("Tesla_Coil");
    }
    public void TLab()
    {
        SceneManager.LoadScene("LAB CENTER");
    }
    public void CLab()
    {
        SceneManager.LoadScene("FINAL BOSS");
    }

    public void Quit()
    {
        Application.Quit();
    }
    
}
