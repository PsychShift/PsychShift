using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class WinLoseButtons : MonoBehaviour
{
    // Start is called before the first frame update
    //public GameObject WinUI;
    public GameObject LoseUI;

    void Start()
    {
        //WinUI.SetActive(false);

        LoseUI.SetActive(false);
        if(SceneManager.GetActiveScene().name == "WinScreen")
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
        
        

        //Time.fixedDeltaTime = .02f;
    }

    // Update is called once per frame
    void Update()
    {
        /* if (Input.GetKey(KeyCode.L))
        {
            LoseUI.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
        if (Input.GetKey(KeyCode.I))
        {
            WinUI.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        } */
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
    public void CloseGame()
    {
        Application.Quit();
    }
}
