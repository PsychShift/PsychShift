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
    public GameObject LoadMenuFirst;
    public GameObject MainMenu;
    public CanvasGroup MainMenuCG;
    public GameObject MainMenuFirst;
    public GameObject SettingsMenu;
    public CanvasGroup SettingsMenuCG;
    public GameObject SettingsMenuFirst;
    public GameObject CreditsMenu;
    public CanvasGroup CreditsMenuCG;
    public GameObject CreditsMenuFirst;
    public GameObject MovementVideo;
    public GameObject MovementVideoFirst;
    public GameObject NeuroNetworkVideo;
    public GameObject NeuroNetworkVideoFirst;
    public GameObject RemappingMenu;
    public GameObject RemappingMenuFirst;
    private WaitForSeconds MenuTick = new WaitForSeconds(0.1f);
    private Coroutine WaitforFade;
    [SerializeField] private bool fadeInCredits = false;
    [SerializeField] private bool fadeOutCredits = false;
    [SerializeField] private bool fadeInSettings = false;
    [SerializeField] private bool fadeOutSettings = false;
    [SerializeField] private bool fadeInMenu = false;
    [SerializeField] private bool fadeOutMenu = false;
    // Start is called before the first frame update
    void Start()
    {
        SettingsMenu.SetActive(false);
        LoadMenu.SetActive(false);
        CreditsMenu.SetActive(false);
        MovementVideo.SetActive(false);
        NeuroNetworkVideo.SetActive(false);
        RemappingMenu.SetActive(false);

        // Jonathan plased this in the update state, dont know why. Undo it if I broke something.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
        EventSystem.current.SetSelectedGameObject(MainMenuFirst);
        CreditsMenuCG.alpha = 0;
        SettingsMenuCG.alpha = 0;
        MainMenuCG.alpha = 1;
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        if (fadeInCredits)
        {
            if (CreditsMenuCG.alpha < 1)
            {
                CreditsMenuCG.alpha += Time.deltaTime;
                if (CreditsMenuCG.alpha >= 1)
                {
                    fadeInCredits = false;
                }
            }
        }
        if (fadeOutCredits)
        {
            if (CreditsMenuCG.alpha >= 0)
            {
                CreditsMenuCG.alpha -= Time.deltaTime;
                if (CreditsMenuCG.alpha == 0)
                {
                    fadeOutCredits = false;
                }
            }
        }
        if (fadeInMenu)
        {
            if (MainMenuCG.alpha < 1)
            {
                MainMenuCG.alpha += Time.deltaTime;
                if (MainMenuCG.alpha >= 1)
                {
                    fadeInMenu = false;
                }
            }
        }
        if (fadeOutMenu)
        {
            if (MainMenuCG.alpha >= 0)
            {
                MainMenuCG.alpha -= Time.deltaTime;
                if (MainMenuCG.alpha == 0)
                {
                    fadeOutMenu = false;
                }
            }
        }
        if (fadeInSettings)
        {
            if (SettingsMenuCG.alpha < 1)
            {
                SettingsMenuCG.alpha += Time.deltaTime;
                if (SettingsMenuCG.alpha >= 1)
                {
                    fadeInSettings = false;
                }
            }
        }
        if (fadeOutSettings)
        {
            if (SettingsMenuCG.alpha >= 0)
            {
                SettingsMenuCG.alpha -= Time.deltaTime;
                if (SettingsMenuCG.alpha == 0)
                {
                    fadeOutSettings = false;
                }
            }
        }
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
        EventSystem.current.SetSelectedGameObject(LoadMenuFirst);
        SettingsMenu.SetActive(false);
    }

    public void CloseLoad()
    {
        LoadMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(SettingsMenuFirst);
        SettingsMenu.SetActive(true);
    }
    public void OpenMovement()
    {
        MovementVideo.SetActive(true);
        EventSystem.current.SetSelectedGameObject(MovementVideoFirst);
    }
    public void CloseMovement()
    {
        MovementVideo.SetActive(false);
        EventSystem.current.SetSelectedGameObject(LoadMenuFirst);
    }
    public void OpenNeuro()
    {
        NeuroNetworkVideo.SetActive(true);
        EventSystem.current.SetSelectedGameObject(NeuroNetworkVideoFirst);
    }
    public void CloseNeuro()
    {
        NeuroNetworkVideo.SetActive(false);
        EventSystem.current.SetSelectedGameObject(LoadMenuFirst);
    }
    public void OpenCredits()
    {
        fadeInCredits = true;
        fadeOutMenu = true;
        CreditsMenu.SetActive(true);
        //CreditsMenu.alpha = 1;
        //MainMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(CreditsMenuFirst);
        WaitforFade = StartCoroutine(FadeMenu());
    }
    public void CloseCredits()
    {
        fadeOutCredits = true;
        fadeInMenu = true;
        //CreditsMenuCG.alpha = 0;
        //CreditsMenu.SetActive(false);
        MainMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(MainMenuFirst);
        WaitforFade = StartCoroutine(FadeCredits());
    }

    public void OpenSettings()
    {
        fadeInSettings = true;
        fadeOutMenu = true;
        SettingsMenu.SetActive(true);
        //MainMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(SettingsMenuFirst);
        WaitforFade = StartCoroutine(FadeMenu());
    }

    public void CloseSettings()
    {
        fadeOutSettings = true;
        fadeInMenu = true;
        //SettingsMenu.SetActive(false);
        MainMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(MainMenuFirst);
        WaitforFade = StartCoroutine(FadeSettings());
    }
    private IEnumerator FadeMenu()
    {
        yield return new WaitForSeconds(1);
        MainMenu.SetActive(false);
        
    }
    private IEnumerator FadeCredits()
    {
        yield return new WaitForSeconds(1);
        CreditsMenu.SetActive(false);
        
    }
    private IEnumerator FadeSettings()
    {
        yield return new WaitForSeconds(1);
        SettingsMenu.SetActive(false);
        
    }
    public void ILab()
    {
        SceneManager.LoadScene("GREG 2");
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
    public void OpenRemap()
    {
        RemappingMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(RemappingMenuFirst);
    }

    public void CloseRemap()
    {
        RemappingMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(SettingsMenuFirst);
    }
    
}
