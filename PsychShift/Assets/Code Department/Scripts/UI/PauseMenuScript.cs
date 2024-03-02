using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Player;

public class PauseMenuScript : MonoBehaviour
{
    public GameObject PauseMenu;
    public GameObject PauseMenuFirst;
    public GameObject PauseMenuSettings;
    public GameObject PauseMenuSettingsFirst;
    public GameObject TutorialMenu;
    public GameObject MovementVideo;
    public GameObject NeuroNetworkVideo;
    public GameObject RemappingMenu;
    public static bool GameIsPaused = false;
    // Start is called before the first frame update
    void Start()
    {
        //Time.fixedDeltaTime = .02f;
        
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

    }
    public void Retry()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
    }

    public void CloseLoad()
    {
        TutorialMenu.SetActive(false);
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
        InputManager.Instance.OnPausePressed+= PausePressed;
    }
    private void OnDisable() 
    {
        InputManager.Instance.OnPausePressed-= PausePressed;
    }
    public void OpenMovement()
    {
        MovementVideo.SetActive(true);
    }
    public void CloseMovement()
    {
        MovementVideo.SetActive(false);
    }
    public void OpenNeuro()
    {
        NeuroNetworkVideo.SetActive(true);
    }
    public void CloseNeuro()
    {
        NeuroNetworkVideo.SetActive(false);
    }
    public void OpenRemap()
    {
        RemappingMenu.SetActive(true);
    }

    public void CloseRemap()
    {
        RemappingMenu.SetActive(false);
    }
}
