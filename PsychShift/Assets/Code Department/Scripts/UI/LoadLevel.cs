using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    public bool loadLevel;
    public string ArtScene;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (loadLevel = true)
        {
            loadLevel = false;
            StartCoroutine(LoadLevelAsync());
        }
    }
    private IEnumerator LoadLevelAsync()
    {
        var progress = SceneManager.LoadSceneAsync(ArtScene, LoadSceneMode.Additive);

        while (!progress.isDone)
        {
            yield return null;
        }
        Debug.Log("LevelLoaded");
    }
}
