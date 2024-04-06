using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class MainMenuScript : MonoBehaviour
{
    #region References
    public LoadingScene loadingSceneScript;
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
    public GameObject DiffMenu;
    public GameObject DiffMenuFirst;
    public GameObject DiffMenu1;
    public GameObject DiffMenu1First;
    public GameObject DiffMenu2;
    public GameObject DiffMenu2First;
    public GameObject DiffMenu3;
    public GameObject DiffMenu3First;
    public GameObject DiffMenu4;
    public GameObject DiffMenu4First;
    public GameObject DiffMenu5;
    public GameObject DiffMenu5First;
    public GameObject DiffMenu6;
    public GameObject DiffMenu6First;
    public GameObject CutSceneMenu;
    public GameObject CutSceneMenuFirst;
    public GameObject LevelMenu;
    public GameObject LevelMenuFirst;
    public GameObject CutScene1;
    public GameObject CutScene1First;
    public GameObject CutScene2;
    public GameObject CutScene2First;
    public GameObject CutScene3;
    public GameObject CutScene3First;
    public GameObject CutScene4;
    public GameObject CutScene4First;
    public GameObject CutScene5;
    public GameObject CutScene5First;
    public GameObject CutScene6;
    public GameObject CutScene6First;
    private WaitForSeconds MenuTick = new WaitForSeconds(0.1f);
    private Coroutine WaitforFade;
    [SerializeField] private bool fadeInCredits = false;
    [SerializeField] private bool fadeOutCredits = false;
    [SerializeField] private bool fadeInSettings = false;
    [SerializeField] private bool fadeOutSettings = false;
    [SerializeField] private bool fadeInMenu = false;
    [SerializeField] private bool fadeOutMenu = false;
    #endregion
    #region UI Stuff
    // Start is called before the first frame update
    void Start()
    {
        SettingsMenu.SetActive(false);
        LoadMenu.SetActive(false);
        LevelMenu.SetActive(false);
        CreditsMenu.SetActive(false);
        MovementVideo.SetActive(false);
        NeuroNetworkVideo.SetActive(false);
        RemappingMenu.SetActive(false);
        DiffMenu.SetActive(false);
        DiffMenu1.SetActive(false);
        DiffMenu2.SetActive(false);
        DiffMenu3.SetActive(false);
        DiffMenu4.SetActive(false);
        DiffMenu5.SetActive(false);
        DiffMenu6.SetActive(false);
        CutSceneMenu.SetActive(false);

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
    #endregion
    #region Button Voids
    public void labCenterGREG()
    {
        SceneManager.LoadScene("LAB CENTER");
    }

    public void Train()
    {
        SceneManager.LoadScene("Level 2");
    }
    public void LoadArtScene()
    {
        SceneManager.LoadScene("LOBBY INTRO LEVEL");
    }
    public void GameplayScene()
    {
        SceneManager.LoadScene("FINAL BOSS");
    }
    public void VSScene()
    {
        SceneManager.LoadScene("Intro");
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
    public void OpenLevelMenu()
    {
        LevelMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(LevelMenuFirst);
        SettingsMenu.SetActive(false);
    }

    public void CloseLevelMenu()
    {
        LevelMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(MainMenuFirst);
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
    public void Level1()
    {
        loadingSceneScript.LoadScene(2);
    }
    public void Level2()
    {
        loadingSceneScript.LoadScene(3);
    }
    public void IntroLobby()
    {
        //SceneManager.LoadScene("LOBBY INTRO LEVEL");
        loadingSceneScript.LoadScene(4);
    }
    public void Trevor()
    {
        loadingSceneScript.LoadScene(5);
    }
    public void CLab()
    {
        loadingSceneScript.LoadScene(6);
    }
    public void BossLab()
    {
        loadingSceneScript.LoadScene(7);
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
    public void OpenDiff()
    {
        DiffMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(DiffMenuFirst);
    }

    public void CloseDiff()
    {
        DiffMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(SettingsMenuFirst);
    }
    public void OpenDiff1()
    {
        DiffMenu1.SetActive(true);
        EventSystem.current.SetSelectedGameObject(DiffMenu1First);
    }

    public void CloseDiff1()
    {
        DiffMenu1.SetActive(false);
        EventSystem.current.SetSelectedGameObject(SettingsMenuFirst);
    }
    public void OpenDiff2()
    {
        DiffMenu2.SetActive(true);
        EventSystem.current.SetSelectedGameObject(DiffMenu2First);
    }

    public void CloseDiff2()
    {
        DiffMenu2.SetActive(false);
        EventSystem.current.SetSelectedGameObject(SettingsMenuFirst);
    }
    public void OpenDiff3()
    {
        DiffMenu3.SetActive(true);
        EventSystem.current.SetSelectedGameObject(DiffMenu3First);
    }

    public void CloseDiff3()
    {
        DiffMenu3.SetActive(false);
        EventSystem.current.SetSelectedGameObject(SettingsMenuFirst);
    }
    public void OpenDiff4()
    {
        DiffMenu4.SetActive(true);
        EventSystem.current.SetSelectedGameObject(DiffMenu4First);
    }

    public void CloseDiff4()
    {
        DiffMenu4.SetActive(false);
        EventSystem.current.SetSelectedGameObject(SettingsMenuFirst);
    }
    public void OpenDiff5()
    {
        DiffMenu5.SetActive(true);
        EventSystem.current.SetSelectedGameObject(DiffMenu5First);
    }

    public void CloseDiff5()
    {
        DiffMenu5.SetActive(false);
        EventSystem.current.SetSelectedGameObject(SettingsMenuFirst);
    }
    public void OpenDiff6()
    {
        DiffMenu6.SetActive(true);
        EventSystem.current.SetSelectedGameObject(DiffMenu6First);
    }

    public void CloseDiff6()
    {
        DiffMenu6.SetActive(false);
        EventSystem.current.SetSelectedGameObject(SettingsMenuFirst);
    }
    public void OpenCutMenu()
    {
        CutSceneMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(CutSceneMenuFirst);
    }

    public void CloseCutMenu()
    {
        CutSceneMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(MainMenuFirst);
    }
    public void OpenCut1()
    {
        CutScene1.SetActive(true);
        EventSystem.current.SetSelectedGameObject(CutScene1First);
    }
    public void CloseCut1()
    {
        CutScene1.SetActive(false);
        EventSystem.current.SetSelectedGameObject(CutSceneMenuFirst);
    }
    public void OpenCut2()
    {
        CutScene2.SetActive(true);
        EventSystem.current.SetSelectedGameObject(CutScene2First);
    }
    public void CloseCut2()
    {
        CutScene2.SetActive(false);
        EventSystem.current.SetSelectedGameObject(CutSceneMenuFirst);
    }
    public void OpenCut3()
    {
        CutScene3.SetActive(true);
        EventSystem.current.SetSelectedGameObject(CutScene3First);
    }
    public void CloseCut3()
    {
        CutScene3.SetActive(false);
        EventSystem.current.SetSelectedGameObject(CutSceneMenuFirst);
    }
    public void OpenCut4()
    {
        CutScene4.SetActive(true);
        EventSystem.current.SetSelectedGameObject(CutScene4First);
    }
    public void CloseCut4()
    {
        CutScene4.SetActive(false);
        EventSystem.current.SetSelectedGameObject(CutSceneMenuFirst);
    }
    public void OpenCut5()
    {
        CutScene5.SetActive(true);
        EventSystem.current.SetSelectedGameObject(CutScene5First);
    }
    public void CloseCut5()
    {
        CutScene5.SetActive(false);
        EventSystem.current.SetSelectedGameObject(CutSceneMenuFirst);
    }
    public void OpenCut6()
    {
        CutScene6.SetActive(true);
        EventSystem.current.SetSelectedGameObject(CutScene6First);
    }
    public void CloseCut6()
    {
        CutScene1.SetActive(false);
        EventSystem.current.SetSelectedGameObject(CutSceneMenuFirst);
    }
    #endregion
      #region Fade Stuff
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
}
#endregion