using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    //private Scene currentScene;
    void Start()
    {
        //currentScene = SceneManager.GetActiveScene();
    }
    private void OnTriggerEnter(Collider other) 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //if(other.CompareTag("Player"))
        //{
            //if(SceneManager.GetActiveScene().name == "Level 1")
            //{
                //SceneManager.LoadScene("Level 2");
            //}
            //if(SceneManager.GetActiveScene().name == "Level 2")
            //{
                //SceneManager.LoadScene("LOBBY INTRO LEVEL");
            //}
            //if(SceneManager.GetActiveScene().name == "LOBBY INTRO LEVEL")
            //{
                //SceneManager.LoadScene("TREVOR ROOM");
            //}
            //if(SceneManager.GetActiveScene().name == "TREVOR ROOM")
            //{
                //SceneManager.LoadScene("LAB CENTER");
            //}
            //if(SceneManager.GetActiveScene().name == "LAB CENTER")
            //{
                //SceneManager.LoadScene("FINAL BOSS");
            //}
            //if(SceneManager.GetActiveScene().name == "FINAL BOSS")
            //{
                //SceneManager.LoadScene("WinScreen");
            //}
        //}
    }
}
