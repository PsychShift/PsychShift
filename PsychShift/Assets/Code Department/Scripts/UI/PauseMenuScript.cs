using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class PauseMenuScript : MonoBehaviour
{
    public GameObject PauseMenu;
    public GameObject PauseMenuFirst;
    public GameObject PauseMenuSettings;
    public GameObject PauseMenuSettingsFirst;
    public static bool GameIsPaused = false;
    // Start is called before the first frame update
    void Start()
    {
        Time.fixedDeltaTime = .02f;
        
        PauseMenu.SetActive(false);
        PauseMenuSettings.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Quit()
    {
        Time.timeScale = 1f;
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
    private void PausePressed()
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
    private void OnEnable() 
    {
        InputManager.Instance.OnPausePressed+= PausePressed;
    }
    private void OnDisable() 
    {
        InputManager.Instance.OnPausePressed-= PausePressed;
    }
}
