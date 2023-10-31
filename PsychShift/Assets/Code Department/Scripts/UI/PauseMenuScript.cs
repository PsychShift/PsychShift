using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PauseMenuScript : MonoBehaviour
{
    public GameObject PauseMenu;
    public GameObject PauseMenuSettings;
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
            PauseMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            Time.timeScale = 0f;
            GameIsPaused = true;
            Cursor.visible = true;
        }
    }
    public void Resume()
    {
        PauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        GameIsPaused = false;
        Cursor.visible = false;

    }
    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Quit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }
    public void OpenPauseSettings()
    {
        PauseMenuSettings.SetActive(true);

    }
    public void ClosePauseSettings()
    {
        PauseMenuSettings.SetActive(false);
    }
}
