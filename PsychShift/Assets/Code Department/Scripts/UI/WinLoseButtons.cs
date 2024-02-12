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
        PlayerMaster.Instance.SetIsMenu(true);
        SceneManager.LoadScene("Main Menu VS");
    }
    public void CloseGame()
    {
        Application.Quit();
    }
}


/* public void Retry()
    {
        StartCoroutine(ReloadSceneCoroutine());
    }
    private IEnumerator ReloadSceneCoroutine()
    {
        Scene oldScene = SceneManager.GetActiveScene();

        // Load the scene additively
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(oldScene.buildIndex, LoadSceneMode.Additive);

        // Wait for the scene to load
        while (!loadOperation.isDone)
        {
            Debug.Log("loading");
            yield return new WaitForEndOfFrame();
        }

        // Unload the old scene
        AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(oldScene);

        // Optionally, you can wait for the unload operation to complete
        while (!unloadOperation.isDone)
        {
            Debug.Log("unloading");
            yield return new WaitForEndOfFrame();
        }
    } */