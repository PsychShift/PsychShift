using Player;
using System.Collections;
using System.Collections.Generic;
//using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.SceneManagement;
 

public class SaveTest : MonoBehaviour
{
    public SaveObject saveObject;
    public int CurrentSceneIndex;
    public int sceneToContinue;

    public void SaveData()
    {
        Player.CharacterInfo charInfo = PlayerStateMachine.Instance.currentCharacter;
        int health = charInfo.enemyHealth.CurrentHealth;

        saveObject = new SaveObject();

        saveObject.Savepoint = charInfo.characterContainer.transform;
        saveObject.Character = charInfo;
        SaveManager.Save(saveObject);
        CurrentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetInt("SavedScene", CurrentSceneIndex);
    }
    
    public void LoadData()
    {
        saveObject = SaveManager.Load();
        sceneToContinue = PlayerPrefs.GetInt("SavedScene");
        if (sceneToContinue != 0)
            SceneManager.LoadScene(sceneToContinue);
        else
            return;
    }

    // Save the current character game object
    // the current character health characterInfoReference.enemyHealth.CurrentHealth
}
