using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneManage : MonoBehaviour
{
    public float introScene;
    public float outroScene;
    string sceneName;
    static bool played;
    // Start is called before the first frame update
    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("n"))
        {
            if(sceneName=="Intro")
                SceneManager.LoadScene("Final_VS_Prototype");
            else if(sceneName=="Outro")
                SceneManager.LoadScene("WinScreen");

        }
            
        if(sceneName == "Intro")
        {
            introScene-=Time.deltaTime;
            if(introScene <= 0||played == true)
            {
                played = true;
                SceneManager.LoadScene("Final_VS_Prototype");
            }
        }
        else if(sceneName== "Outro")
        {
            outroScene-=Time.deltaTime;
            if(outroScene <= 0)
                SceneManager.LoadScene("WinScreen");
        }
    }
}
