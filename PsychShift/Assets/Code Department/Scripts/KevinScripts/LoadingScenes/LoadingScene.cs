using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.InputSystem;

public class LoadingScene : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject LoadingScreen;
    public Image LoadingBarFill;
    public GameObject cutSceneCanvas;
    private bool cutSceneSkipped;
    private int sceneIDRef;
    public PlayerInput playerInput;
    bool sceneIdReceived;
    //static int NextScene;
    // Start is called before the first frame update
    void Update() 
    {

        if(Input.GetKeyDown(KeyCode.K) && sceneIdReceived)
        {
            SkipCutscene();
        }



    }
    public void LoadScene(int sceneId)
    {
        sceneIDRef=sceneId;
        Time.timeScale = 0f;
        AudioListener.volume = 0;
        playerInput.enabled= true;
        PlayerMaster.Instance.SetCheckPoint(transform);
        PlayerMaster.Instance.isLoadingNewSceneTransition = true;
        if(videoPlayer!=null)
        {
            cutSceneCanvas.SetActive(true);
            StartCoroutine(LoadSceneVideo(sceneId));
        }
            
        else
            StartCoroutine(LoadSceneAsync(sceneId));
    }
    IEnumerator LoadSceneAsync(int sceneId)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);
        LoadingScreen.SetActive(true);

        while(!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);

            //LoadingBarFill.fillAmount = progressValue;
            yield return null;
        }
    }

    public VideoPlayer videoPlayer;
    
    //public bool isCutScene;
   
    IEnumerator LoadSceneVideo(int sceneId)
    {
        videoPlayer.Play();
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);
        operation.allowSceneActivation = false;
        sceneIdReceived = true;
        

        while(videoPlayer.isPlaying)
        {
            //float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            if(cutSceneSkipped)
            {
                videoPlayer.Stop();
                operation.allowSceneActivation = true;
            }
            //LoadingBarFill.fillAmount = progressValue;
            yield return null;
        }
        while(!operation.isDone)
        {
            yield return null;
        }
        operation.allowSceneActivation = true;
    }
    public void SkipCutscene()
    {
        /* if(videoPlayer!=null)
            SceneManager.LoadScene(sceneIDRef); */
        cutSceneSkipped = true;
        
    }
}
