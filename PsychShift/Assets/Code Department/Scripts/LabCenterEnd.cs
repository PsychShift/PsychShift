using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LabCenterEnd : MonoBehaviour
{
    public GameObject MidasVideo;
    public GameObject OFCanvas;
    public GameObject Player;
    private Coroutine WaitforEnd;
    // Start is called before the first frame update
    void Start()
    {
        MidasVideo.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Player")
        {
            PlayerMaster.Instance.StartNew();
            //SceneManager.LoadScene("FINAL BOSS");
            WaitforEnd = StartCoroutine(CutScene());
            MidasVideo.SetActive(true);
            OFCanvas.SetActive(false);
            //Player.SetActive(false);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            Time.timeScale = 0f;


        }
    }
    private IEnumerator CutScene()
    {
        yield return new WaitForSeconds(45);
        Time.timeScale = 1f;
        //SceneManager.LoadScene("FINAL BOSS");
        OFCanvas.SetActive(true);
        //SceneManager.LoadScene("FINAL BOSS");
        
    }
    public void Skip()
    {
        Time.timeScale = 1f;
        OFCanvas.SetActive(true);
        SceneManager.LoadScene("WinScreen");
    }
}
