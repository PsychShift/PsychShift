using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using Player;
using Guns.Demo;

public class PauseMenuScript : MonoBehaviour
{
    public GameObject PauseMenu;
    public GameObject PauseMenuFirst;
    public GameObject PauseMenuSettings;
    public GameObject TutorialButton;
    public GameObject RebindButton;
    public GameObject PauseMenuSettingsFirst;
    public GameObject TutorialMenu;
    public GameObject TutorialMenuFirst;
    public GameObject MovementVideo;
    public GameObject MovementVideoFirst;
    public GameObject NeuroNetworkVideo;
    public GameObject NeuroNetworkVideoFirst;
    public GameObject RemappingMenu;
    public GameObject RemappingMenuFirst;
    public PlayerAction playerAction;

    public static bool GameIsPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        GameIsPaused = false;
        playerAction.enabled = true;
        PauseMenu.SetActive(false);
        PauseMenuSettings.SetActive(false);
        MovementVideo.SetActive(false);
        NeuroNetworkVideo.SetActive(false);
        TutorialMenu.SetActive(false);
        RemappingMenu.SetActive(false);
    }

    public void Resume()
    {
        PauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        GameIsPaused = false;
        Cursor.visible = false;
        EventSystem.current.SetSelectedGameObject(null);
        playerAction.enabled = true;
    }
    public void Retry()
    {

        Debug.Log("why");
        PlayerMaster.Instance.Load();
    }
    public void Quit()
    {
        Time.timeScale = 1f;
        PlayerStateMachine.Instance.SetLocation(null);
        SceneManager.LoadScene("Main Menu VS");
    }
    public void OpenPauseSettings()
    {
        PauseMenuSettings.SetActive(true);
        PauseMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(PauseMenuSettingsFirst);

    }
    public void ClosePauseSettings()
    {
        PauseMenuSettings.SetActive(false);
        PauseMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(PauseMenuFirst);
    }
    public void OpenLoad()
    {
        TutorialMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(TutorialMenuFirst);
        TutorialButton.SetActive(false);
    }

    public void CloseLoad()
    {
        TutorialMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(PauseMenuSettingsFirst);
        TutorialButton.SetActive(true);
    }
    private void PausePressed()
    {
        if(PauseMenu.activeSelf)
        {
            Resume();
        }
        else
        {
            TimeManager.Instance.UndoSlowmotion();
            PauseMenu.SetActive(true);
            PauseMenuSettings.SetActive(false);
            Cursor.lockState = CursorLockMode.Confined;
            Time.timeScale = 0f;
            GameIsPaused = true;
            Cursor.visible = true;
            EventSystem.current.SetSelectedGameObject(PauseMenuFirst);

        }
        
    }
    private void OnEnable() 
    {
        playerAction.enabled = true;
        InputManager.Instance.OnPausePressed+= PausePressed;
    }
    private void OnDisable() 
    {
        playerAction.enabled = false;
        InputManager.Instance.OnPausePressed-= PausePressed;
    }
    public void OpenMovement()
    {
        MovementVideo.SetActive(true);
        EventSystem.current.SetSelectedGameObject(MovementVideoFirst);
    }
    public void CloseMovement()
    {
        MovementVideo.SetActive(false);
        EventSystem.current.SetSelectedGameObject(TutorialMenuFirst);
    }
    public void OpenNeuro()
    {
        NeuroNetworkVideo.SetActive(true);
        EventSystem.current.SetSelectedGameObject(NeuroNetworkVideoFirst);
    }
    public void CloseNeuro()
    {
        NeuroNetworkVideo.SetActive(false);
        EventSystem.current.SetSelectedGameObject(TutorialMenuFirst);
    }
    public void OpenRemap()
    {
        RemappingMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(RemappingMenuFirst);
        TutorialButton.SetActive(false);
    }

    public void CloseRemap()
    {
        RemappingMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(PauseMenuSettingsFirst);
        TutorialButton.SetActive(true);
    }
}
