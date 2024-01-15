using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTest : MonoBehaviour
{
    public SaveObject saveObject;

    public void SaveData()
    {
        SaveManager.Save(saveObject);
    }
    public void LoadData()
    {
        saveObject = SaveManager.Load();
    }
}
