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
    public CanvasGroup RemappingMenuCG;
    public GameObject DiffMenu;
    public GameObject DiffMenuFirst;
    public CanvasGroup DiffMenuCG;
    public GameObject DiffMenu1;
    public GameObject DiffMenu1First;
    public CanvasGroup DiffMenu1CG;
    public GameObject DiffMenu2;
    public GameObject DiffMenu2First;
    public CanvasGroup DiffMenu2CG;
    public GameObject DiffMenu3;
    public GameObject DiffMenu3First;
    public CanvasGroup DiffMenu3CG;
    public GameObject DiffMenu4;
    public GameObject DiffMenu4First;
    public CanvasGroup DiffMenu4CG;
    public GameObject DiffMenu5;
    public GameObject DiffMenu5First;
    public CanvasGroup DiffMenu5CG;
    public GameObject DiffMenu6;
    public GameObject DiffMenu6First;
    public CanvasGroup DiffMenu6CG;
    public GameObject CutSceneMenu;
    public GameObject CutSceneMenuFirst;
    public CanvasGroup CutSceneMenuCG;
    public GameObject LevelMenu;
    public GameObject LevelMenuFirst;
    public CanvasGroup LevelMenuCG;
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
    [SerializeField] private bool fadeInDifficulty = false;
    [SerializeField] private bool fadeOutDifficulty = false;
    [SerializeField] private bool fadeInDifficulty1 = false;
    [SerializeField] private bool fadeOutDifficulty1 = false;
    [SerializeField] private bool fadeInDifficulty2 = false;
    [SerializeField] private bool fadeOutDifficulty2 = false;
    [SerializeField] private bool fadeInDifficulty3 = false;
    [SerializeField] private bool fadeOutDifficulty3 = false;
    [SerializeField] private bool fadeInDifficulty4 = false;
    [SerializeField] private bool fadeOutDifficulty4 = false;
    [SerializeField] private bool fadeInDifficulty5 = false;
    [SerializeField] private bool fadeOutDifficulty5 = false;
    [SerializeField] private bool fadeInDifficulty6 = false;
    [SerializeField] private bool fadeOutDifficulty6 = false;
    [SerializeField] private bool fadeInRemappingMenu = false;
    [SerializeField] private bool fadeOutRemappingMenu = false;
    [SerializeField] private bool fadeInCutSceneMenu = false;
    [SerializeField] private bool fadeOutCutSceneMenu = false;
    [SerializeField] private bool fadeInLevelMenu = false;
    [SerializeField] private bool fadeOutLevelMenu = false;
    #endregion
    #region UI Stuff
    #region  ON OFF MENU STUFF
    public GameObject settingsScreen; 
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        //Kevin added this 
        settingsScreen.SetActive(true);
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
        LevelMenuCG.alpha = 0;
        RemappingMenuCG.alpha = 0;
        DiffMenuCG.alpha = 0;
        DiffMenu1CG.alpha = 0;
        DiffMenu2CG.alpha = 0;
        DiffMenu3CG.alpha = 0;
        DiffMenu4CG.alpha = 0;
        DiffMenu5CG.alpha = 0;
        DiffMenu6CG.alpha = 0;
        CutSceneMenuCG.alpha = 0;
        MainMenuCG.alpha = 1;
        MainMenuCG.interactable = true;


        settingsScreen.SetActive(false);
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
        if (fadeInDifficulty)
        {
            if (DiffMenuCG.alpha < 1)
            {
                DiffMenuCG.alpha += Time.deltaTime;
                if (DiffMenuCG.alpha >= 1)
                {
                    fadeInDifficulty = false;
                }
            }
        }
        if (fadeOutDifficulty)
        {
            if (DiffMenuCG.alpha >= 0)
            {
                DiffMenuCG.alpha -= Time.deltaTime;
                if (DiffMenuCG.alpha == 0)
                {
                    fadeOutDifficulty = false;
                }
            }
        }
        if (fadeInDifficulty1)
        {
            if (DiffMenu1CG.alpha < 1)
            {
                DiffMenu1CG.alpha += Time.deltaTime;
                if (DiffMenu1CG.alpha >= 1)
                {
                    fadeInDifficulty1 = false;
                }
            }
        }
        if (fadeOutDifficulty1)
        {
            if (DiffMenu1CG.alpha >= 0)
            {
                DiffMenu1CG.alpha -= Time.deltaTime;
                if (DiffMenu1CG.alpha == 0)
                {
                    fadeOutDifficulty1 = false;
                }
            }
        }
        if (fadeInDifficulty2)
        {
            if (DiffMenu2CG.alpha < 1)
            {
                DiffMenu2CG.alpha += Time.deltaTime;
                if (DiffMenu2CG.alpha >= 1)
                {
                    fadeInDifficulty2 = false;
                }
            }
        }
        if (fadeOutDifficulty2)
        {
            if (DiffMenu2CG.alpha >= 0)
            {
                DiffMenu2CG.alpha -= Time.deltaTime;
                if (DiffMenu2CG.alpha == 0)
                {
                    fadeOutDifficulty2 = false;
                }
            }
        }
        if (fadeInDifficulty3)
        {
            if (DiffMenu3CG.alpha < 1)
            {
                DiffMenu3CG.alpha += Time.deltaTime;
                if (DiffMenu3CG.alpha >= 1)
                {
                    fadeInDifficulty3 = false;
                }
            }
        }
        if (fadeOutDifficulty3)
        {
            if (DiffMenu3CG.alpha >= 0)
            {
                DiffMenu3CG.alpha -= Time.deltaTime;
                if (DiffMenu3CG.alpha == 0)
                {
                    fadeOutDifficulty3 = false;
                }
            }
        }
        if (fadeInDifficulty4)
        {
            if (DiffMenu4CG.alpha < 1)
            {
                DiffMenu4CG.alpha += Time.deltaTime;
                if (DiffMenu4CG.alpha >= 1)
                {
                    fadeInDifficulty4 = false;
                }
            }
        }
        if (fadeOutDifficulty4)
        {
            if (DiffMenu4CG.alpha >= 0)
            {
                DiffMenu4CG.alpha -= Time.deltaTime;
                if (DiffMenu4CG.alpha == 0)
                {
                    fadeOutDifficulty4 = false;
                }
            }
        }
        if (fadeInDifficulty5)
        {
            if (DiffMenu5CG.alpha < 1)
            {
                DiffMenu5CG.alpha += Time.deltaTime;
                if (DiffMenu5CG.alpha >= 1)
                {
                    fadeInDifficulty5 = false;
                }
            }
        }
        if (fadeOutDifficulty5)
        {
            if (DiffMenu5CG.alpha >= 0)
            {
                DiffMenu5CG.alpha -= Time.deltaTime;
                if (DiffMenu5CG.alpha == 0)
                {
                    fadeOutDifficulty5 = false;
                }
            }
        }
        if (fadeInDifficulty6)
        {
            if (DiffMenu6CG.alpha < 1)
            {
                DiffMenu6CG.alpha += Time.deltaTime;
                if (DiffMenu6CG.alpha >= 1)
                {
                    fadeInDifficulty6 = false;
                }
            }
        }
        if (fadeOutDifficulty6)
        {
            if (DiffMenu6CG.alpha >= 0)
            {
                DiffMenu6CG.alpha -= Time.deltaTime;
                if (DiffMenu6CG.alpha == 0)
                {
                    fadeOutDifficulty6 = false;
                }
            }
        }
        if (fadeInRemappingMenu)
        {
            if (RemappingMenuCG.alpha < 1)
            {
                RemappingMenuCG.alpha += Time.deltaTime;
                if (RemappingMenuCG.alpha >= 1)
                {
                    fadeInRemappingMenu = false;
                }
            }
        }
        if (fadeOutRemappingMenu)
        {
            if (RemappingMenuCG.alpha >= 0)
            {
                RemappingMenuCG.alpha -= Time.deltaTime;
                if (RemappingMenuCG.alpha == 0)
                {
                    fadeOutRemappingMenu = false;
                }
            }
        }
        if (fadeInCutSceneMenu)
        {
            if (CutSceneMenuCG.alpha < 1)
            {
                CutSceneMenuCG.alpha += Time.deltaTime;
                if (CutSceneMenuCG.alpha >= 1)
                {
                    fadeInCutSceneMenu = false;
                }
            }
        }
        if (fadeOutCutSceneMenu)
        {
            if (CutSceneMenuCG.alpha >= 0)
            {
                CutSceneMenuCG.alpha -= Time.deltaTime;
                if (CutSceneMenuCG.alpha == 0)
                {
                    fadeOutCutSceneMenu = false;
                }
            }
        }
        if (fadeInLevelMenu)
        {
            if (LevelMenuCG.alpha < 1)
            {
                LevelMenuCG.alpha += Time.deltaTime;
                if (MainMenuCG.alpha >= 1)
                {
                    fadeInLevelMenu = false;
                }
            }
        }
        if (fadeOutLevelMenu)
        {
            if (LevelMenuCG.alpha >= 0)
            {
                LevelMenuCG.alpha -= Time.deltaTime;
                if (LevelMenuCG.alpha == 0)
                {
                    fadeOutLevelMenu = false;
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
        fadeInLevelMenu = true;
        fadeOutMenu = true;
        MainMenuCG.interactable = false;
        LevelMenuCG.interactable = true;
        EventSystem.current.SetSelectedGameObject(LevelMenuFirst);
        WaitforFade = StartCoroutine(FadeMenu());

    }

    public void CloseLevelMenu()
    {
        MainMenu.SetActive(true);
        fadeOutLevelMenu = true;
        fadeInMenu = true;
        MainMenuCG.interactable = true;
        LevelMenuCG.interactable = false;
        //LevelMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(MainMenuFirst);
        WaitforFade = StartCoroutine(FadeLevelMenu());
        
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
        CreditsMenuCG.interactable = true;
        MainMenuCG.interactable = false;
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
        CreditsMenuCG.interactable = false;
        MainMenuCG.interactable = true;
        EventSystem.current.SetSelectedGameObject(MainMenuFirst);
        WaitforFade = StartCoroutine(FadeCredits());
    }

    public void OpenSettings()
    {
        fadeInSettings = true;
        fadeOutMenu = true;
        SettingsMenu.SetActive(true);
        SettingsMenuCG.interactable = true;
        MainMenuCG.interactable = false;
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
        SettingsMenuCG.interactable = false;
        MainMenuCG.interactable = true;
        EventSystem.current.SetSelectedGameObject(MainMenuFirst);
        WaitforFade = StartCoroutine(FadeSettings());
    }

    public void OpenRemap()
    {
        fadeInRemappingMenu = true;
        fadeOutSettings = true;
        RemappingMenu.SetActive(true);
        SettingsMenuCG.interactable = false;
        RemappingMenuCG.interactable = true;
        EventSystem.current.SetSelectedGameObject(RemappingMenuFirst);
        WaitforFade = StartCoroutine(FadeSettings());
    }

    public void CloseRemap()
    {
        fadeOutRemappingMenu = true;
        fadeInSettings = true;
        SettingsMenu.SetActive(true);
        SettingsMenuCG.interactable = true;
        RemappingMenuCG.interactable = false;
        //RemappingMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(SettingsMenuFirst);
        WaitforFade = StartCoroutine(FadeRemappingMenu());
    }
    public void OpenDiff()
    {
        fadeInDifficulty = true;
        fadeOutMenu = true;
        DiffMenu.SetActive(true);
        DiffMenuCG.interactable = true;
        MainMenuCG.interactable = false;
        EventSystem.current.SetSelectedGameObject(DiffMenuFirst);
        WaitforFade = StartCoroutine(FadeMenu());
    }

    public void CloseDiff()
    {
        fadeOutDifficulty = true;
        fadeInMenu = true;
        MainMenu.SetActive(true);
        DiffMenuCG.interactable = false;
        MainMenuCG.interactable = true;
        //DiffMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(MainMenuFirst);
        WaitforFade = StartCoroutine(FadeDifficulty());
    }
    public void OpenDiff1()
    {
        fadeInDifficulty1 = true;
        fadeOutLevelMenu = true;
        DiffMenu1.SetActive(true);
        DiffMenu1CG.interactable = true;
        LevelMenuCG.interactable = false;
        EventSystem.current.SetSelectedGameObject(DiffMenu1First);
        WaitforFade = StartCoroutine(FadeLevelMenu());
    }

    public void CloseDiff1()
    {
        fadeOutDifficulty1 = true;
        fadeInLevelMenu = true;
        LevelMenu.SetActive(true);
        //DiffMenu1.SetActive(false);
        DiffMenu1CG.interactable = false;
        LevelMenuCG.interactable = true;
        EventSystem.current.SetSelectedGameObject(LevelMenuFirst);
        WaitforFade = StartCoroutine(FadeDifficulty1());
    }
    public void OpenDiff2()
    {
        fadeInDifficulty2 = true;
        fadeOutLevelMenu = true;
        DiffMenu2.SetActive(true);
        DiffMenu2CG.interactable = true;
        LevelMenuCG.interactable = false;
        EventSystem.current.SetSelectedGameObject(DiffMenu2First);
        WaitforFade = StartCoroutine(FadeLevelMenu());
    }

    public void CloseDiff2()
    {
        fadeOutDifficulty2 = true;
        fadeInLevelMenu = true;
        LevelMenu.SetActive(true);
        //DiffMenu2.SetActive(false);
        DiffMenu2CG.interactable = false;
        LevelMenuCG.interactable = true;
        EventSystem.current.SetSelectedGameObject(LevelMenuFirst);
        WaitforFade = StartCoroutine(FadeDifficulty2());
    }
    public void OpenDiff3()
    {
        fadeInDifficulty3 = true;
        fadeOutLevelMenu = true;
        DiffMenu3.SetActive(true);
        DiffMenu3CG.interactable = true;
        LevelMenuCG.interactable = false;
        EventSystem.current.SetSelectedGameObject(DiffMenu3First);
        WaitforFade = StartCoroutine(FadeLevelMenu());
    }

    public void CloseDiff3()
    {
        fadeOutDifficulty3 = true;
        fadeInLevelMenu = true;
        LevelMenu.SetActive(true);
        //DiffMenu3.SetActive(false);
        DiffMenu3CG.interactable = false;
        LevelMenuCG.interactable = true;
        EventSystem.current.SetSelectedGameObject(LevelMenuFirst);
        WaitforFade = StartCoroutine(FadeDifficulty3());
    }
    public void OpenDiff4()
    {
        fadeInDifficulty4 = true;
        fadeOutLevelMenu = true;
        DiffMenu4.SetActive(true);
        DiffMenu4CG.interactable = true;
        LevelMenuCG.interactable = false;
        EventSystem.current.SetSelectedGameObject(DiffMenu4First);
        WaitforFade = StartCoroutine(FadeLevelMenu());
    }

    public void CloseDiff4()
    {
        fadeOutDifficulty4 = true;
        fadeInLevelMenu = true;
        LevelMenu.SetActive(true);
        //DiffMenu4.SetActive(false);
        DiffMenu4CG.interactable = false;
        LevelMenuCG.interactable = true;
        EventSystem.current.SetSelectedGameObject(LevelMenuFirst);
        WaitforFade = StartCoroutine(FadeDifficulty4());
        
    }
    public void OpenDiff5()
    {
        fadeInDifficulty5 = true;
        fadeOutLevelMenu = true;
        DiffMenu5.SetActive(true);
        DiffMenu5CG.interactable = true;
        LevelMenuCG.interactable = false;
        EventSystem.current.SetSelectedGameObject(DiffMenu5First);
        WaitforFade = StartCoroutine(FadeLevelMenu());
    }

    public void CloseDiff5()
    {
        LevelMenu.SetActive(true);
        fadeOutDifficulty5 = true;
        fadeInLevelMenu = true;
        //DiffMenu5.SetActive(false);
        DiffMenu5CG.interactable = false;
        LevelMenuCG.interactable = true;
        EventSystem.current.SetSelectedGameObject(LevelMenuFirst);
        WaitforFade = StartCoroutine(FadeDifficulty5());
    }
    public void OpenDiff6()
    {
        DiffMenu6.SetActive(true);
        fadeInDifficulty6 = true;
        fadeOutLevelMenu = true;
        DiffMenu6CG.interactable = true;
        LevelMenuCG.interactable = false;
        EventSystem.current.SetSelectedGameObject(DiffMenu6First);
        WaitforFade = StartCoroutine(FadeLevelMenu());
    }

    public void CloseDiff6()
    {
        LevelMenu.SetActive(true);
        fadeOutDifficulty6 = true;
        fadeInLevelMenu = true;
        //DiffMenu6.SetActive(false);
        DiffMenu6CG.interactable = false;
        LevelMenuCG.interactable = true;
        EventSystem.current.SetSelectedGameObject(LevelMenuFirst);
        WaitforFade = StartCoroutine(FadeDifficulty6());
    }
    public void OpenCutMenu()
    {
        CutSceneMenu.SetActive(true);
        fadeInCutSceneMenu = true;
        fadeOutMenu = true;
        CutSceneMenuCG.interactable = true;
        MainMenuCG.interactable = false;
        EventSystem.current.SetSelectedGameObject(CutSceneMenuFirst);
        WaitforFade = StartCoroutine(FadeMenu());
    }

    public void CloseCutMenu()
    {
        MainMenu.SetActive(true);
        fadeOutCutSceneMenu = true;
        fadeInMenu = true;
        //CutSceneMenu.SetActive(false);
        CutSceneMenuCG.interactable = false;
        MainMenuCG.interactable = true;
        EventSystem.current.SetSelectedGameObject(MainMenuFirst);
        WaitforFade = StartCoroutine(FadeCutSceneMenu());
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
        CutScene6.SetActive(false);
        EventSystem.current.SetSelectedGameObject(CutSceneMenuFirst);
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
    private IEnumerator FadeDifficulty()
    {
        yield return new WaitForSeconds(1);
        DiffMenu.SetActive(false);
        
    }
    private IEnumerator FadeDifficulty1()
    {
        yield return new WaitForSeconds(1);
        DiffMenu1.SetActive(false);
        
    }
    private IEnumerator FadeDifficulty2()
    {
        yield return new WaitForSeconds(1);
        DiffMenu2.SetActive(false);
        
    }
    private IEnumerator FadeDifficulty3()
    {
        yield return new WaitForSeconds(1);
        DiffMenu3.SetActive(false);
        
    }
    private IEnumerator FadeDifficulty4()
    {
        yield return new WaitForSeconds(1);
        DiffMenu4.SetActive(false);
        
    }
    private IEnumerator FadeDifficulty5()
    {
        yield return new WaitForSeconds(1);
        DiffMenu5.SetActive(false);
        
    }
    private IEnumerator FadeDifficulty6()
    {
        yield return new WaitForSeconds(1);
        DiffMenu6.SetActive(false);
        
    }
    private IEnumerator FadeLevelMenu()
    {
        yield return new WaitForSeconds(1);
        LevelMenu.SetActive(false);
        
    }
    private IEnumerator FadeRemappingMenu()
    {
        yield return new WaitForSeconds(1);
        RemappingMenu.SetActive(false);
        
    }
    private IEnumerator FadeCutSceneMenu()
    {
        yield return new WaitForSeconds(1);
        CutSceneMenu.SetActive(false);
        
    }
}
#endregion