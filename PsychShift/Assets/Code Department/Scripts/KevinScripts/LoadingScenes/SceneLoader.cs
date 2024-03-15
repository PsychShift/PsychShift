using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public bool isCutScene;
    public bool isLevelTransition;
    public void LevelChanger()
    {
        if(isCutScene)
        {
            //Load the cutscene scene

        }
        else if(isLevelTransition)
        {
            //load lvl after cutscene
        }
    }
    
}
